using System;
using UnityEngine;

namespace BeatMayhem.Game
{
    /// <summary>
    /// Tracks successful beat-aligned hits and triggers an Order Burst when reaching the target combo size.
    /// </summary>
    public class OrderCombo : MonoBehaviour
    {
        [Tooltip("Number of perfect hits required to trigger an Order Burst.")]
        public int comboTarget = 6;

        [Tooltip("Chaos meter affected by the burst.")]
        public ChaosMeter chaosMeter;

        [Tooltip("Duration of global slow motion caused by the burst.")]
        public float slowDuration = 1.5f;

        [Tooltip("Time scale applied during the burst.")]
        public float slowTimeScale = 0.5f;

        public event Action<int> OnComboChanged;
        public event Action OnBurstTriggered;

        public int CurrentCombo { get; private set; }

        private float _slowTimer;
        private bool _isSlowing;

        private void Update()
        {
            if (!_isSlowing)
            {
                return;
            }

            _slowTimer -= Time.unscaledDeltaTime;
            if (_slowTimer <= 0f)
            {
                Time.timeScale = 1f;
                _isSlowing = false;
            }
        }

        public void RegisterHit()
        {
            CurrentCombo++;
            OnComboChanged?.Invoke(CurrentCombo);

            if (CurrentCombo >= comboTarget)
            {
                TriggerBurst();
            }
        }

        public void ResetCombo()
        {
            if (CurrentCombo == 0)
            {
                return;
            }

            CurrentCombo = 0;
            OnComboChanged?.Invoke(CurrentCombo);
        }

        private void TriggerBurst()
        {
            ResetCombo();
            chaosMeter?.TriggerOrderBurst();
            OnBurstTriggered?.Invoke();

            if (slowDuration > 0f && slowTimeScale > 0f)
            {
                Time.timeScale = slowTimeScale;
                _slowTimer = slowDuration;
                _isSlowing = true;
            }
        }
    }
}
