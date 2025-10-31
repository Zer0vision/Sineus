using BeatMayhem.Game;
using TMPro;
using UnityEngine;

namespace BeatMayhem.UI
{
    /// <summary>
    /// Shows the current combo count and plays a short animation when the value increases.
    /// </summary>
    public class ComboDisplay : MonoBehaviour
    {
        public OrderCombo combo;
        public TMP_Text comboText;
        public Animator animator;
        public string pulseTrigger = "Pulse";

        private void OnEnable()
        {
            if (combo != null)
            {
                combo.OnComboChanged += HandleComboChanged;
                combo.OnBurstTriggered += HandleBurst;
                HandleComboChanged(combo.CurrentCombo);
            }
        }

        private void OnDisable()
        {
            if (combo != null)
            {
                combo.OnComboChanged -= HandleComboChanged;
                combo.OnBurstTriggered -= HandleBurst;
            }
        }

        private void HandleComboChanged(int value)
        {
            if (comboText != null)
            {
                comboText.text = value.ToString();
            }

            if (animator != null)
            {
                animator.SetTrigger(pulseTrigger);
            }
        }

        private void HandleBurst()
        {
            if (animator != null)
            {
                animator.SetTrigger("Burst");
            }
        }
    }
}
