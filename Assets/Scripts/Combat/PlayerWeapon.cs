using BeatMayhem.Game;
using UnityEngine;

namespace BeatMayhem.Combat
{
    /// <summary>
    /// Handles firing logic for the player's rifle. Only allows shots during the active beat window and informs the chaos meter on misses.
    /// </summary>
    public class PlayerWeapon : MonoBehaviour
    {
        [Tooltip("Projectile prefab spawned when the weapon fires.")]
        public Projectile projectilePrefab;

        [Tooltip("Transform that defines the muzzle position and direction.")]
        public Transform muzzle;

        [Tooltip("Beat clock used to validate timing windows.")]
        public BeatClock beatClock;

        [Tooltip("Chaos meter affected by off-beat shots.")]
        public ChaosMeter chaosMeter;

        [Tooltip("Amount of chaos added when firing outside the timing window.")]
        public float chaosPenalty = 12f;

        [Tooltip("Chaos reduced on perfect hits.")]
        public float chaosReward = 6f;

        [Tooltip("Combo counter for order bursts.")]
        public OrderCombo combo;

        [Tooltip("Optional perk manager for special effects.")]
        public BeatMayhem.Game.PerkManager perkManager;

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                TryFire();
            }
        }

        private void TryFire()
        {
            bool inWindow = beatClock != null && beatClock.IsInBeatWindow();

            if (!inWindow)
            {
                chaosMeter?.AddChaos(chaosPenalty);
                combo?.ResetCombo();
                return;
            }

            SpawnProjectile();
            combo?.RegisterHit();
            chaosMeter?.ReduceChaos(chaosReward);
            perkManager?.OnPerfectHit();
        }

        private void SpawnProjectile()
        {
            if (projectilePrefab == null || muzzle == null)
            {
                return;
            }

            Projectile projectile = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
            if (_camera != null)
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                Vector3 direction = ray.direction.normalized;
                projectile.Fire(direction);
            }
            else
            {
                projectile.Fire(muzzle.forward);
            }
        }
    }
}
