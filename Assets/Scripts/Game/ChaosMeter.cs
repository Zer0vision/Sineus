using System;
using UnityEngine;

namespace BeatMayhem.Game
{
    /// <summary>
    /// Tracks chaos intensity. Increasing chaos tightens the timing window and speeds up enemy spawns.
    /// Auto-triggers an Order Burst fail-safe when the meter stays above the danger threshold long enough.
    /// </summary>
    public class ChaosMeter : MonoBehaviour
    {
        [Tooltip("Maximum chaos value.")]
        public float maxValue = 100f;

        [Tooltip("Chaos value above which the fail-safe kicks in.")]
        public float dangerThreshold = 80f;

        [Tooltip("Seconds the chaos value needs to stay above the danger threshold before the auto burst is triggered.")]
        public float dangerDuration = 5f;

        public event Action<float> OnChaosChanged;
        public event Action OnOrderBurst;

        public float CurrentValue { get; private set; }

        private float _dangerTimer;

        public void AddChaos(float amount)
        {
            if (amount <= 0f)
            {
                return;
            }

            SetChaos(CurrentValue + amount);
        }

        public void ReduceChaos(float amount)
        {
            if (amount <= 0f)
            {
                return;
            }

            SetChaos(CurrentValue - amount);
        }

        public void ResetChaos()
        {
            SetChaos(0f);
        }

        public void TriggerOrderBurst()
        {
            ResetChaos();
            OnOrderBurst?.Invoke();
            _dangerTimer = 0f;
        }

        private void Update()
        {
            if (CurrentValue >= dangerThreshold)
            {
                _dangerTimer += Time.deltaTime;
                if (_dangerTimer >= dangerDuration)
                {
                    TriggerOrderBurst();
                }
            }
            else
            {
                _dangerTimer = 0f;
            }
        }

        private void SetChaos(float value)
        {
            float clamped = Mathf.Clamp(value, 0f, maxValue);
            if (!Mathf.Approximately(clamped, CurrentValue))
            {
                CurrentValue = clamped;
                OnChaosChanged?.Invoke(CurrentValue / maxValue);
            }
        }
    }
}
