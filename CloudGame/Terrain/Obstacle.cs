using Entity;
using UnityEngine;

namespace Terrain
{
    [RequireComponent(typeof(Collider), typeof(Health))]
    public class Obstacle : MonoBehaviour
    {
        [SerializeField]
        private float damage = 9999f;

        private void Start()
        {
            //instantiate death handler.
        }
    
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Health>(out var health)) return;
            health.Damage(damage);
        }
    }
}
