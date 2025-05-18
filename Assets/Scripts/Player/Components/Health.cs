using UnityEngine;
using UnityEngine.Events;

namespace UrbanFracture.Player.Components
{
    /// <summary>
    /// Manages the player's health, damage handling, healing, and death events.
    /// </summary>
    public class Health : MonoBehaviour, IDamageable
    {
        [Header("Health Settings")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float currentHealth;

        [Header("Events")]
        public UnityEvent<float> OnHealthChanged;
        public UnityEvent<float> OnHealed;
        public UnityEvent OnDeath;

        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        public bool IsDead => currentHealth <= 0f;

        /// <summary>
        /// Resets health to maximum values
        /// </summary>
        private void Awake() { currentHealth = maxHealth; }

        /// <summary>
        /// Applies damage to the player and triggers death if health reaches zero.
        /// </summary>
        /// <param name="amount"></param>
        public void TakeDamage(float amount)
        {
            if (IsDead) return;
            currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);
            OnHealthChanged?.Invoke(currentHealth);
            if (currentHealth <= 0f) { Die(); }
        }

        /// <summary>
        /// Heals the player by a specified amount.
        /// </summary>
        /// <param name="amount"></param>
        public void Heal(float amount)
        {
            if (IsDead) return;
            float oldHealth = currentHealth;
            currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
            OnHealthChanged?.Invoke(currentHealth);
            OnHealed?.Invoke(currentHealth - oldHealth);
        }

        /// <summary>
        /// Instantly kills the player.
        /// </summary>
        public void Kill()
        {
            if (IsDead) return;
            currentHealth = 0f;
            OnHealthChanged?.Invoke(currentHealth);
            Die();
        }

        /// <summary>
        /// Handles death logic and triggers any death-related events.
        /// </summary>
        private void Die()
        {
            OnDeath?.Invoke();
            Debug.Log($"{gameObject.name} has died.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="healthAmount"></param>
        public void Revive(float healthAmount)
        {
            if (!IsDead) return;
            currentHealth = Mathf.Clamp(healthAmount, 0f, maxHealth);
            OnHealthChanged?.Invoke(currentHealth);
        }

        public void ResetHealth()
        {
            currentHealth = maxHealth;
            OnHealthChanged?.Invoke(currentHealth);
        }

    }
}
