using UnityEngine;

namespace UrbanFracture.Core.Player
{
    /// <summary>
    /// A passive enemy used for testing health and combat mechanics.
    /// Does not move or attack.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class EnemyController : BaseCharacterController
    {
        protected override void Awake() { base.Awake(); }

        private void Update()
        {
           
        }

        // Optional: Expose damage for testing
        public void DebugTakeDamage(float amount)
        {
            TakeDamage(amount);
        }
    }
}
