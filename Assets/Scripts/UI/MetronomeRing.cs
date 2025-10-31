using BeatMayhem.Game;
using UnityEngine;
using UnityEngine.UI;

namespace BeatMayhem.UI
{
    /// <summary>
    /// Animates a radial fill image to visualize the beat window.
    /// </summary>
    public class MetronomeRing : MonoBehaviour
    {
        public BeatClock beatClock;
        public Image ringImage;
        public Image perfectFlash;

        public float perfectThreshold = 0.08f;
        public float flashDuration = 0.15f;

        private float _flashTimer;

        private void OnEnable()
        {
            if (beatClock != null)
            {
                beatClock.OnBeat += HandleBeat;
                beatClock.OnBeatProgress += HandleProgress;
            }
        }

        private void OnDisable()
        {
            if (beatClock != null)
            {
                beatClock.OnBeat -= HandleBeat;
                beatClock.OnBeatProgress -= HandleProgress;
            }
        }

        private void Update()
        {
            if (perfectFlash != null && _flashTimer > 0f)
            {
                _flashTimer -= Time.deltaTime;
                float alpha = Mathf.Clamp01(_flashTimer / flashDuration);
                perfectFlash.color = new Color(perfectFlash.color.r, perfectFlash.color.g, perfectFlash.color.b, alpha);
            }
        }

        private void HandleProgress(float progress)
        {
            if (ringImage != null)
            {
                ringImage.fillAmount = 1f - progress;
            }
        }

        private void HandleBeat(int beat)
        {
            if (perfectFlash != null)
            {
                _flashTimer = flashDuration;
                perfectFlash.color = new Color(perfectFlash.color.r, perfectFlash.color.g, perfectFlash.color.b, 1f);
            }
        }

        public void ShowPerfectWindow()
        {
            if (perfectFlash != null)
            {
                _flashTimer = flashDuration;
                perfectFlash.color = new Color(perfectFlash.color.r, perfectFlash.color.g, perfectFlash.color.b, 1f);
            }
        }
    }
}
