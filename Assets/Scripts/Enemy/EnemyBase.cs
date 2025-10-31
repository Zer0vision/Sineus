using BeatMayhem.Combat;
using BeatMayhem.Game;
using UnityEngine;

namespace BeatMayhem.Enemy
{
    /// <summary>
    /// Base enemy behaviour that hooks into the beat clock for synchronized actions.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public abstract class EnemyBase : MonoBehaviour, IDamageable
    {
        public int maxHealth = 3;
        public int scoreValue = 10;
        public BeatClock beatClock;
        public float chaosContribution = 5f;

        protected int CurrentHealth { get; private set; }

        protected virtual void Awake()
        {
            CurrentHealth = maxHealth;
            if (beatClock != null)
            {
                beatClock.OnBeat += HandleBeat;
            }
        }

        protected virtual void OnDestroy()
        {
            if (beatClock != null)
            {
                beatClock.OnBeat -= HandleBeat;
            }
        }

        protected abstract void HandleBeat(int index);

        public virtual void TakeDamage(int amount)
        {
            CurrentHealth -= amount;
            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            Destroy(gameObject);
        }
    }
}
