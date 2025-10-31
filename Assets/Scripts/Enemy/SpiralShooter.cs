using BeatMayhem.Combat;
using UnityEngine;

namespace BeatMayhem.Enemy
{
    /// <summary>
    /// Enemy that emits spiral bullets on every beat.
    /// </summary>
    public class SpiralShooter : EnemyBase
    {
        public Projectile projectilePrefab;
        public int bulletsPerBeat = 6;
        public float spreadDegrees = 360f;
        public float rotationSpeed = 45f;
        public Transform muzzle;

        private float _angleOffset;

        private void Update()
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }

        protected override void HandleBeat(int index)
        {
            if (projectilePrefab == null)
            {
                return;
            }

            float step = spreadDegrees / bulletsPerBeat;
            for (int i = 0; i < bulletsPerBeat; i++)
            {
                float angle = _angleOffset + i * step;
                Vector3 direction = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
                Vector3 spawnPos = muzzle != null ? muzzle.position : transform.position;
                Projectile projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.LookRotation(direction));
                projectile.Fire(direction);
            }

            _angleOffset += step * 0.5f;
        }
    }
}
