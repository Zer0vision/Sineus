using System.Collections.Generic;
using BeatMayhem.Combat;
using UnityEngine;

namespace BeatMayhem.Game
{
    /// <summary>
    /// Applies lightweight perk effects to the player.
    /// </summary>
    public class PerkManager : MonoBehaviour
    {
        public BeatClock beatClock;
        public PlayerWeapon playerWeapon;
        public List<PerkDefinition> unlockedPerks = new();

        private bool _piercingActive;
        private bool _timeStop;

        private void Start()
        {
            foreach (PerkDefinition perk in unlockedPerks)
            {
                ApplyPerk(perk);
            }
        }

        public void ApplyPerk(PerkDefinition perk)
        {
            if (perk == null)
            {
                return;
            }

            switch (perk.perkName)
            {
                case "Metronome Aura":
                    ActivateMetronomeAura();
                    break;
                case "Piercing Rounds":
                    _piercingActive = true;
                    break;
                case "Time Snap":
                    _timeStop = true;
                    break;
            }
        }

        private void ActivateMetronomeAura()
        {
            if (beatClock != null)
            {
                beatClock.timingWindowMs += 20f;
            }
        }

        public void OnPerfectHit()
        {
            if (_timeStop)
            {
                Time.timeScale = 0.2f;
                Invoke(nameof(ResumeTime), 0.2f);
            }
        }

        private void ResumeTime()
        {
            Time.timeScale = 1f;
        }
    }
}
