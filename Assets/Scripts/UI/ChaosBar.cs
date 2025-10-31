using BeatMayhem.Game;
using UnityEngine;
using UnityEngine.UI;

namespace BeatMayhem.UI
{
    /// <summary>
    /// Displays the chaos meter value in the UI.
    /// </summary>
    public class ChaosBar : MonoBehaviour
    {
        public ChaosMeter chaosMeter;
        public Slider slider;
        public Gradient colorGradient;
        public Image fillImage;

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
            if (slider != null)
            {
                slider.value = normalized;
            }

            if (fillImage != null)
            {
                fillImage.color = colorGradient.Evaluate(normalized);
            }
        }
    }
}
