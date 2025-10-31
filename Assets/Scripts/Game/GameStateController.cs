using BeatMayhem.Combat;
using BeatMayhem.Game;
using UnityEngine;

namespace BeatMayhem
{
    /// <summary>
    /// High level orchestrator for the arena prototype. Handles player death and assist toggles.
    /// </summary>
    public class GameStateController : MonoBehaviour
    {
        public BeatClock beatClock;
        public ChaosMeter chaosMeter;
        public PlayerWeapon playerWeapon;
        public RhythmDifficultyScaler difficultyScaler;
        public bool assistMode;

        [Header("Assist Settings")]
        public float assistWindowMs = 200f;

        private float _defaultWindowMs;

        private void Awake()
        {
            if (beatClock != null)
            {
                _defaultWindowMs = beatClock.timingWindowMs;
            }
        }

        private void Start()
        {
            if (difficultyScaler != null)
            {
                difficultyScaler.enabled = !assistMode;
            }
            ApplyAssistMode();
        }

        public void SetAssistMode(bool enabled)
        {
            assistMode = enabled;
            ApplyAssistMode();
        }

        private void ApplyAssistMode()
        {
            if (beatClock == null)
            {
                return;
            }

            if (assistMode)
            {
                beatClock.timingWindowMs = assistWindowMs;
                if (difficultyScaler != null)
                {
                    difficultyScaler.enabled = false;
                }
            }
            else
            {
                beatClock.timingWindowMs = _defaultWindowMs;
                if (difficultyScaler != null)
                {
                    difficultyScaler.enabled = true;
                }
            }
        }
    }
}
