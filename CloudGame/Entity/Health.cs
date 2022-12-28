using UnityEngine;

namespace Entity
{
    public class Health : MonoBehaviour
    {
        public float maxHealth;
        public float CurrentHealth { get; private set; }

        private IDeathHandler _deathHandler;

        [SerializeField]
        private float damageCooldown;

        private float _damageCooldownTimer;

        private void Start()
        {
            CurrentHealth = maxHealth;
            //instantiate death handler in object managers.
        }

        private void Update()
        {
            if (CurrentHealth <= 0f)
            {
                _deathHandler.OnDeath();
            }
        }
    
        public void Damage(float value)
        {
            if (!(_damageCooldownTimer < 0f)) return;
            CurrentHealth = Mathf.Clamp(CurrentHealth - value, 0, maxHealth);
            _damageCooldownTimer = damageCooldown;
        }

        public void Heal(float value)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + value, 0, maxHealth);
        }
    }
}