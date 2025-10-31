using UnityEngine;

namespace BeatMayhem.Combat
{
    /// <summary>
    /// Simple projectile that travels forward and deals damage on the first collider hit.
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        public float speed = 35f;
        public float lifetime = 3f;
        public int damage = 1;

        private Vector3 _direction;
        private float _age;

        public void Fire(Vector3 direction)
        {
            _direction = direction.normalized;
        }

        private void Update()
        {
            transform.position += _direction * (speed * Time.deltaTime);
            _age += Time.deltaTime;
            if (_age >= lifetime)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
