using UnityEngine;

namespace Entity.Enemy
{
    [RequireComponent(typeof(Health))]
    public class Balloon : MonoBehaviour, IEnemyBase
    {
        public int CurrencyValue { get; set; }
        public int HealthMultiplier { get; set; }
        public int DamageMultiplier { get; set; }
        public int RateMultiplier { get; set; }
        public GameObject SectionObject { get; set; }

        [SerializeField]
        private float defaultHealth;
        private Health _health;

        private void Start()
        {
            _health = GetComponent<Health>();
            _health.maxHealth = defaultHealth * HealthMultiplier;
        }
    }
}