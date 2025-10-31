using System.Collections.Generic;
using BeatMayhem.Game;
using UnityEngine;

namespace BeatMayhem.Enemy
{
    /// <summary>
    /// Spawns enemy waves according to the supplied schedule and scales spawn cadence with chaos intensity.
    /// </summary>
    public class ChaosSpawner : MonoBehaviour
    {
        [System.Serializable]
        public class SpawnEntry
        {
            public float time;
            public EnemyBase prefab;
            public Vector3 position;
            public Vector3 rotation;
        }

        [Tooltip("Static wave entries in seconds.")]
        public List<SpawnEntry> entries = new();

        [Tooltip("Additional spawn multiplier applied when chaos is at maximum.")]
        public float chaosSpawnMultiplier = 2f;

        public ChaosMeter chaosMeter;
        public Transform container;

        private float _elapsed;
        private int _nextIndex;

        private void Update()
        {
            _elapsed += Time.deltaTime * GetChaosMultiplier();

            while (_nextIndex < entries.Count && _elapsed >= entries[_nextIndex].time)
            {
                SpawnEntry entry = entries[_nextIndex];
                Spawn(entry);
                _nextIndex++;
            }
        }

        private float GetChaosMultiplier()
        {
            if (chaosMeter == null)
            {
                return 1f;
            }

            return Mathf.Lerp(1f, chaosSpawnMultiplier, chaosMeter.CurrentValue / chaosMeter.maxValue);
        }

        private void Spawn(SpawnEntry entry)
        {
            if (entry.prefab == null)
            {
                return;
            }

            EnemyBase enemy = Instantiate(entry.prefab, entry.position, Quaternion.Euler(entry.rotation), container);
            if (chaosMeter != null)
            {
                chaosMeter.AddChaos(enemy.chaosContribution);
            }
        }
    }
}
