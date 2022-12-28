using UnityEngine;

namespace Entity.Enemy
{
    public interface IEnemyBase
    { 
        int CurrencyValue { get; set; }
        int HealthMultiplier { get; set; }
        int DamageMultiplier { get; set; }
        int RateMultiplier { get; set; }
        GameObject SectionObject { get; set; }
    }
}