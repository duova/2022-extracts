using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Common.Enums.SlotableIdentifiers;
using Game.Scripts.Common.Structs;
using Game.Scripts.Server.Slotables.System;
using Game.Scripts.Server.Structs;
using UnityEngine;

namespace Game.Scripts.Utility
{
    public static class SlotableInfoUtility
    {
        public static void JsonToSlotableInfoList(string json, ref SlotableInfoList list)
        {
            JsonUtility.FromJsonOverwrite(json, list);
        }

        public static string GetTypeAssemblyName<T>()
        {
            return typeof(T).AssemblyQualifiedName;
        }

        public static SlotableInfo GetSlotableInfoSkill(SlotableSkill slotableSkill, SlotableInfoList list)
        {
            return list.slotableSkillInfos.First(slotableInfo => slotableInfo.idString == slotableSkill.ToString());
        }
        
        public static SlotableInfo GetSlotableInfoAbility(SlotableAbility slotableAbility, SlotableInfoList list)
        {
            return list.slotableAbilityInfos.First(slotableInfo => slotableInfo.idString == slotableAbility.ToString());
        }
        
        public static SlotableInfo GetSlotableInfoItem(SlotableItem slotableItem, SlotableInfoList list)
        {
            return list.slotableItemInfos.First(slotableInfo => slotableInfo.idString == slotableItem.ToString());
        }
        
        public static SlotableInfo GetSlotableInfoEffect(SlotableEffect slotableEffect, SlotableInfoList list)
        {
            return list.slotableEffectInfos.First(slotableInfo => slotableInfo.idString == slotableEffect.ToString());
        }

        public static StatBlock CreateStats(StatComponentInfo info)
        {
            var statInfos = info.statInfos;
            var statBlock = new StatBlock();
            for (var i = 0; i < statInfos.Length; i++)
            {
                statBlock.AssignStat(statInfos[i].statTypeEnum, statInfos[i].statValue);
            }

            return statBlock;
        }

        public static Slotable CreateSlotable(SlotableInfo info)
        {
            var slotableComponents = new List<SlotableComponent>();

            HandleActiveComponents(info.activeComponent, ref slotableComponents);
            HandlePassiveComponents(info, ref slotableComponents);
            HandleStatComponents(info, ref slotableComponents);

            return info switch
            {
                SlotableAbilityInfo when Enum.TryParse(info.idString, out SlotableAbility ability) => new Slotable(
                    slotableComponents, SlotableType.Ability, ability: ability),
                SlotableSkillInfo when Enum.TryParse(info.idString, out SlotableSkill skill) => new Slotable(
                    slotableComponents, SlotableType.Skill, skill: skill),
                SlotableItemInfo when Enum.TryParse(info.idString, out SlotableItem item) => new Slotable(
                    slotableComponents, SlotableType.Item, item: item),
                SlotableEffectInfo when Enum.TryParse(info.idString, out SlotableEffect effect) => new Slotable(
                    slotableComponents, SlotableType.Effect, effect: effect),
                _ => throw new Exception("Unable to create slotable")
            };
        }

        private static void HandleStatComponents(SlotableInfo info, ref List<SlotableComponent> slotableComponents)
        {
            for (var i = 0; i < info.statComponents.Length; i++)
            {
                var statComponentInfo = info.statComponents[i];
                var statBlock = StatUtility.StatInfoArrayToStatBlock(statComponentInfo.statInfos);
                var component = new StatComponent(statBlock, statComponentInfo.multiplicative);
                slotableComponents.Add(component);
            }
        }

        private static void HandlePassiveComponents(SlotableInfo info, ref List<SlotableComponent> slotableComponents)
        {
            if (info.passiveComponents == null || info.passiveComponents.Length == 0) return;
            foreach (var passiveComponentInfo in info.passiveComponents)
            {
                if (string.IsNullOrEmpty(passiveComponentInfo.typeAssembly)) continue;
                var T = Type.GetType(passiveComponentInfo.typeAssembly);
                if (T == null) continue;
                var instance = Activator.CreateInstance(T);
                if (instance is not PassiveComponent component) continue;
                component.Values = passiveComponentInfo.values;
                component.AnimationClipStrings = passiveComponentInfo.animationClipStrings;
                component.FxPrefabStrings = passiveComponentInfo.fxPrefabStrings;
                slotableComponents.Add(component);
            }
        }

        private static void HandleActiveComponents(ActiveComponentInfo activeComponentInfo, ref List<SlotableComponent> list)
        {
            if (string.IsNullOrEmpty(activeComponentInfo.typeAssembly)) return;
            var T = Type.GetType(activeComponentInfo.typeAssembly);
            if (T == null) return;
            var instance = Activator.CreateInstance(T);
            if (instance is not ActiveComponent component) return;
            component.LocalCooldown = activeComponentInfo.localCooldown;
            component.EnergyCost = activeComponentInfo.energyCost;
            component.Values = activeComponentInfo.values;
            component.AnimationClipStrings = activeComponentInfo.animationClipStrings;
            component.FxPrefabStrings = activeComponentInfo.fxPrefabStrings;
            list.Add(component);
        }
    }
}
