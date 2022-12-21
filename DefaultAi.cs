using System;
using System.Collections.Generic;
using System.Linq;
using Server.Core.Battle.Component;
using Server.Util;
using Server.Util.Event;

namespace Server.Core.Battle.Ai;

public enum Elements
{
    Fire,
    Water,
    Nature,
    Lightning,
    Wind,
    Earth
}

public class DefaultAi : IActor
{
    private readonly Random _random = new SafeRandom();
    private bool _madeInitialAttack;

    private static IList<Ability> FilterAbilitiesByComponentType<T>(IEnumerable<Ability> abilities)
        where T : AbilityComponent
    {
        return abilities.Where(ability => ability.AbilityComponents.Any(component => component is T)).ToList();
    }

    private static void FailAction(Ability ability, BattleEvolit selected) =>
        throw new Exception($"RandomAi failed to use special ability: {ability.Name}. On Evolit: {selected}");

    public void PerformAction(Lobby lobby)
    {
        var actorBattleData = lobby.SideHandler.CurrentActorData();
        var selectedEvolit = actorBattleData.BattleTeam.SelectedEvolit();
        //Filter out abilities which are on cooldown.
        var abilities =
            selectedEvolit.Original.Abilities.Where(ability => !selectedEvolit.AbilityTracker.IsTracked(ability))
                .ToList();

        //Get opponent evolit element.
        Elements opponentElement = lobby.SideHandler.EnemyActorData().BattleTeam.SelectedEvolit().Element;

        //Checks if it is the first attack made.
        if (!_madeInitialAttack)
        {
            var specialAbilities = FilterAbilitiesByComponentType<SpecialComponent>(abilities);
            // Use special abilities if avaliable
            var finalAbilities = specialAbilities.Count > 0 ? specialAbilities : abilities;
            ExecuteRandomAbility(finalAbilities, selectedEvolit, lobby.SideHandler, lobby.EventOrchestrator);
            _madeInitialAttack = true;
            return;
        }

        //Gather damage abilities.
        var damageAbilities = new List<Ability>();
        var damageComponents = new List<DamageComponent>();
        foreach (var ability in abilities)
        {
            var filteredComponents = ability.AbilityComponents
                .Where(abilityComponent => abilityComponent is DamageComponent)
                .Cast<DamageComponent>().ToList();
            if (filteredComponents.Count <= 0)
            {
                continue;
            }

            damageComponents.AddRange(filteredComponents);
            damageAbilities.Add(ability);
        }

        //Determine the probability of a damaging ability being used based on how many there are vs non-damaging abilities.
        var damageAbilityRatio = damageAbilities.Count > 0 ? (float)damageAbilities.Count / abilities.Count : 0;

        var randomFloat = (float)_random.NextDouble();
        if (randomFloat >= damageAbilityRatio)
        {
            //Use a random non-damage ability.
            var otherAbilities = abilities.Where(ability => !damageAbilities.Contains(ability)).ToList();
            ExecuteRandomAbility(otherAbilities, selectedEvolit, lobby.SideHandler, lobby.EventOrchestrator);
            return;
        }

        var counterType = FindCounterType(opponentElement);
        var perferredAbilities =
            damageAbilities.Where(ability => damageComponents.Any(comp =>
                comp.DamageType == counterType && ability.AbilityComponents.Contains(comp))).ToList();
        //If there are none attempt to find ones that do true damage.
        if (perferredAbilities.Count == 0)
        {
            perferredAbilities.AddRange(damageAbilities.Where(ability =>
                damageComponents.Any(comp =>
                    comp.DamageType == DamageType.True && ability.AbilityComponents.Contains(comp))));
        }

        //If there are still none then execute a random damaging ability.
        var abilityArray = perferredAbilities.Count == 0 ? damageAbilities : perferredAbilities;
        ExecuteRandomAbility(abilityArray, selectedEvolit, lobby.SideHandler, lobby.EventOrchestrator);
    }

    private void ExecuteRandomAbility(
        IList<Ability> abilities,
        BattleEvolit source,
        ISideHandler sideHandler,
        EventOrchestrator orchestrator
    )
    {
        var ability = abilities[_random.Next(0, abilities.Count - 1)];
        if (!BattleUtils.ExecuteAbility(source, ability, sideHandler, orchestrator))
        {
            FailAction(ability, source);
        }
    }

    private static DamageType FindCounterType(Elements type)
    {
        //Find countering damage ability.
        return (int)type switch
        {
            0 => DamageType.Water,
            1 => DamageType.Nature,
            2 => DamageType.Fire,
            3 => DamageType.Earth,
            4 => DamageType.Lightning,
            5 => DamageType.Wind,
            _ => default
        };
    }
}
