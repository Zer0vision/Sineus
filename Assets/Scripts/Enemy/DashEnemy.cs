using UnityEngine;

namespace BeatMayhem.Enemy
{
    /// <summary>
    /// Enemy that performs a short dash towards the player every beat.
    /// </summary>
    public class DashEnemy : EnemyBase
    {
        public Transform target;
        public float dashForce = 20f;
        public float windupSeconds = 0.25f;

        private Rigidbody _rigidbody;
        private float _windupTimer;

        protected override void Awake()
        {
            base.Awake();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (_windupTimer > 0f)
            {
                _windupTimer -= Time.deltaTime;
                if (_windupTimer <= 0f)
                {
                    PerformDash();
                }
            }
        }

        protected override void HandleBeat(int index)
        {
            _windupTimer = windupSeconds;
        }

        private void PerformDash()
        {
            if (target == null)
            {
                return;
            }

            Vector3 direction = (target.position - transform.position).normalized;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(direction * dashForce, ForceMode.VelocityChange);
        }
    }
}
