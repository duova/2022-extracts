using UnityEngine;

namespace Entity.Turtle
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField]
        private GameObject projectile, firePoint;

        [SerializeField]
        private float projectileVelocity;

        [SerializeField]
        private GameObject tailTarget;

        private Transform _cachedTransform;
        private Transform _cachedTailTargetTransform;
        private LayerMask _notTurtleMask;

        private void Start()
        {
            _cachedTransform = transform;
            _notTurtleMask = ~LayerMask.GetMask("Turtle");
            _cachedTailTargetTransform = tailTarget.transform;
        }

        private void Update()
        {
            _cachedTailTargetTransform.position = _cachedTransform.position + _cachedTransform.forward * 100f;
        }
    
        public void Fire()
        {
            Vector3 direction;
            //Play firing effects.
            direction = Physics.Raycast(_cachedTransform.position, _cachedTransform.forward, out var hit, 100f)
                ? (hit.point - firePoint.transform.position).normalized
                : _cachedTransform.forward;

            var instance = Instantiate(projectile, firePoint.transform.position,
                Quaternion.LookRotation(direction));
            instance.GetComponent<Rigidbody>().velocity = direction * projectileVelocity;
        }
    }
}
