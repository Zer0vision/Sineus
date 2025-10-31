using System;
using UnityEngine;

namespace BeatMayhem.Game
{
    /// <summary>
    /// Central rhythm clock. Emits an <see cref="OnBeat"/> event on every beat using a fixed BPM value.
    /// Also exposes helpers for checking whether a timestamp falls inside the allowed firing window.
    /// </summary>
    public class BeatClock : MonoBehaviour
    {
        [Tooltip("Beats per minute of the gameplay track.")]
        [Range(60f, 200f)]
        public float bpm = 100f;

        [Tooltip("Window around the beat where actions are considered on-time, in milliseconds.")]
        [Range(20f, 500f)]
        public float timingWindowMs = 120f;

        [Tooltip("Optional audio source that owns the music loop. Its time is used to align beats when available.")]
        public AudioSource musicSource;

        public event Action<int> OnBeat;
        public event Action<float> OnBeatProgress;

        public float BeatInterval => 60f / bpm;

        public float TimingWindowSeconds => timingWindowMs / 1000f;

        private int _beatIndex;
        private float _nextBeatTime;

        private void OnEnable()
        {
            _beatIndex = 0;
            _nextBeatTime = GetCurrentTime() + BeatInterval;
        }

        private void Update()
        {
            float currentTime = GetCurrentTime();
            float interval = BeatInterval;

            if (currentTime >= _nextBeatTime)
            {
                _beatIndex++;
                OnBeat?.Invoke(_beatIndex);
                _nextBeatTime += interval;
            }

            float beatElapsed = Mathf.Repeat(currentTime, interval);
            float progress = beatElapsed / interval;
            OnBeatProgress?.Invoke(progress);
        }

        private float GetCurrentTime()
        {
            if (musicSource != null && musicSource.isPlaying)
            {
                return musicSource.time;
            }

            return Time.time;
        }

        /// <summary>
        /// Returns true if the provided timestamp falls within the current timing window.
        /// </summary>
        public bool IsWithinWindow(float timestamp)
        {
            float interval = BeatInterval;
            float position = Mathf.Repeat(timestamp, interval);
            float window = TimingWindowSeconds;

            return position <= window || position >= interval - window;
        }

        /// <summary>
        /// Tests whether the current time is inside the beat window.
        /// </summary>
        public bool IsInBeatWindow() => IsWithinWindow(GetCurrentTime());
    }
}
