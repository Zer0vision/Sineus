using System;
using System.Collections.Generic;
using BeatMayhem.Game;
using UnityEngine;

namespace BeatMayhem.Enemy
{
    /// <summary>
    /// Parses a lightweight JSON wave description and schedules spawns on the <see cref="ChaosSpawner"/>.
    /// </summary>
    public class WavePatternLoader : MonoBehaviour
    {
        [Serializable]
        public class PrefabBinding
        {
            public string key;
            public EnemyBase prefab;
        }

        [TextArea]
        public string waveJson;
        public ChaosSpawner spawner;
        public BeatClock beatClock;
        public List<PrefabBinding> prefabs = new();

        [Serializable]
        private class WaveDocument
        {
            public float bpm = 100f;
            public List<WaveEntry> waves;
        }

        [Serializable]
        private class WaveEntry
        {
            public int bar;
            public SpawnEntry[] spawns;
            public BossEntry boss;
        }

        [Serializable]
        private class SpawnEntry
        {
            public string t;
            public int n = 1;
            public string angle;
        }

        [Serializable]
        private class BossEntry
        {
            public string t;
        }

        private Dictionary<string, EnemyBase> _prefabLookup;

        private void Awake()
        {
            _prefabLookup = new Dictionary<string, EnemyBase>(StringComparer.OrdinalIgnoreCase);
            foreach (PrefabBinding binding in prefabs)
            {
                if (binding.prefab != null && !string.IsNullOrEmpty(binding.key))
                {
                    _prefabLookup[binding.key] = binding.prefab;
                }
            }
        }

        private void Start()
        {
            if (string.IsNullOrWhiteSpace(waveJson) || spawner == null || beatClock == null)
            {
                return;
            }

            WaveDocument document = JsonUtility.FromJson<WaveDocument>(waveJson);
            beatClock.bpm = document.bpm;

            foreach (WaveEntry wave in document.waves)
            {
                if (wave.spawns != null)
                {
                    foreach (SpawnEntry spawn in wave.spawns)
                    {
                        ScheduleSpawn(wave.bar, spawn);
                    }
                }

                if (wave.boss != null)
                {
                    ScheduleBoss(wave.bar, wave.boss);
                }
            }
        }

        private void ScheduleSpawn(int bar, SpawnEntry spawn)
        {
            if (!_prefabLookup.TryGetValue(spawn.t, out EnemyBase prefab))
            {
                Debug.LogWarning($"Unknown spawn type '{spawn.t}'.");
                return;
            }

            float seconds = (bar - 1) * 4 * beatClock.BeatInterval;
            for (int i = 0; i < Mathf.Max(1, spawn.n); i++)
            {
                spawner.entries.Add(new ChaosSpawner.SpawnEntry
                {
                    time = seconds,
                    prefab = prefab,
                    position = transform.position,
                    rotation = Vector3.zero
                });
            }
        }

        private void ScheduleBoss(int bar, BossEntry boss)
        {
            if (!_prefabLookup.TryGetValue(boss.t, out EnemyBase prefab))
            {
                Debug.LogWarning($"Unknown boss type '{boss.t}'.");
                return;
            }

            float seconds = (bar - 1) * 4 * beatClock.BeatInterval;
            spawner.entries.Add(new ChaosSpawner.SpawnEntry
            {
                time = seconds,
                prefab = prefab,
                position = transform.position,
                rotation = Vector3.zero
            });
        }
    }
}
