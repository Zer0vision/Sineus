using BeatMayhem.Combat;
using BeatMayhem.Game;
using UnityEngine;

namespace BeatMayhem.Enemy
{
    /// <summary>
    /// Boss orb that alternates between ordered and chaotic projectile bursts.
    /// </summary>
    public class OrbBoss : EnemyBase
    {
        [System.Serializable]
        public class Phase
        {
            public string name = "Phase";
            public int bars = 2;
            public int bulletsPerBeat = 8;
            public float rotationStep = 10f;
            public bool scrambleBeats;
        }

        public Projectile projectilePrefab;
        public Phase[] phases;
        public BeatClock chaosClock;

        private int _currentPhaseIndex;
        private bool _useChaosClock;

        protected override void Awake()
        {
            base.Awake();
            if (chaosClock != null)
            {
                chaosClock.OnBeat += HandleChaosBeat;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (chaosClock != null)
            {
                chaosClock.OnBeat -= HandleChaosBeat;
            }
        }

        protected override void HandleBeat(int index)
        {
            FirePattern(index, false);
        }

        private void HandleChaosBeat(int index)
        {
            if (!_useChaosClock)
            {
                return;
            }

            FirePattern(index, true);
        }

        private void FirePattern(int beatIndex, bool chaotic)
        {
            if (projectilePrefab == null || phases == null || phases.Length == 0)
            {
                return;
            }

            Phase phase = phases[_currentPhaseIndex];
            float beatProgress = (beatIndex % (phase.bars * 4)) / (float)(phase.bars * 4);
            if (beatProgress >= 0.999f)
            {
                _currentPhaseIndex = (_currentPhaseIndex + 1) % phases.Length;
                phase = phases[_currentPhaseIndex];
                _useChaosClock = phase.scrambleBeats && chaosClock != null;
            }

            int bulletCount = phase.bulletsPerBeat;
            float step = 360f / bulletCount;
            float angleOffset = chaotic ? Random.Range(0f, 360f) : beatIndex * phase.rotationStep;

            for (int i = 0; i < bulletCount; i++)
            {
                float angle = angleOffset + i * step;
                Vector3 direction = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
                Projectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.LookRotation(direction));
                projectile.Fire(direction);
            }
        }
    }
}
