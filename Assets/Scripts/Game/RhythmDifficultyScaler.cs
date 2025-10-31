using UnityEngine;

namespace BeatMayhem.Game
{
    /// <summary>
    /// Adjusts the beat timing window and spawn multiplier based on the chaos meter.
    /// </summary>
    public class RhythmDifficultyScaler : MonoBehaviour
    {
        public BeatClock beatClock;
        public ChaosMeter chaosMeter;
        public float minWindowMs = 60f;
        public float baseWindowMs = 120f;

        private void Awake()
        {
            if (beatClock != null)
            {
                baseWindowMs = beatClock.timingWindowMs;
            }
        }

        private void OnEnable()
        {
            if (chaosMeter != null)
            {
                chaosMeter.OnChaosChanged += HandleChaosChanged;
                HandleChaosChanged(chaosMeter.CurrentValue / chaosMeter.maxValue);
            }
        }

        private void OnDisable()
        {
            if (chaosMeter != null)
            {
                chaosMeter.OnChaosChanged -= HandleChaosChanged;
            }
        }

        private void HandleChaosChanged(float normalized)
        {
            if (beatClock == null)
            {
                return;
            }

            float tightened = Mathf.Lerp(baseWindowMs, minWindowMs, normalized);
            beatClock.timingWindowMs = tightened;
        }
    }
}
