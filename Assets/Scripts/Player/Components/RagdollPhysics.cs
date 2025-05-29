using System.Linq;
using UnityEngine;
using UrbanFracture.Player.AI;

namespace UrbanFracture.Player.Components
{
    /// <summary>
    /// This class handles ragdoll physics for enemies, enabling and disabling ragdoll behavior.
    /// </summary>
    public class RagdollPhysics : MonoBehaviour
    {
        public Rigidbody[] ragdollRigidbodies;
        public Collider[] ragdollColliders;

        private MonoBehaviour[] AIComponents;
        private Animator animator;
        private EnemyManager enemyManager;
        private GameObject enemyRoot;

        private void Awake()
        {
            enemyManager = EnemyManager.Instance;

            if (enemyManager == null) { Debug.LogError("EnemyManager not found in the scene!"); }
            enemyRoot = transform.root.gameObject;

            animator = enemyRoot.GetComponent<Animator>();
            if (animator == null) { Debug.LogWarning("Animator not found on enemy root."); }

            ragdollRigidbodies = GetComponentsInChildren<Rigidbody>(true);
            ragdollColliders = GetComponentsInChildren<Collider>(true);

            // Grab only the AI components from the root enemy GameObject
            AIComponents = enemyRoot.GetComponents<MonoBehaviour>()
                .Where(component =>
                    component is EnemyMovement ||
                    component is EnemyPerception ||
                    component is EnemyAnimation
                )
                .ToArray();
        }

        /// <summary>
        /// This function sets the layer of the ragdoll to "Ragdoll" to ensure proper physics interactions.
        /// </summary>
        private void SetRagdollLayer()
        {
            int ragdollLayer = LayerMask.NameToLayer("Ragdoll");
            SetLayerRecursively(gameObject, ragdollLayer);
        }

        /// <summary>
        /// This function sets the layer of the GameObject and all its children recursively.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="layer"></param>
        private void SetLayerRecursively(GameObject obj, int layer)
        {
            obj.layer = layer;
            foreach (Transform child in obj.transform) { SetLayerRecursively(child.gameObject, layer); }
        }

        /// <summary>
        /// This function enables ragdoll physics for the enemy, 
        /// disabling the animator and enabling rigidbodies and colliders.
        /// </summary>
        public void EnableRagdoll()
        {
            if (animator != null) { animator.enabled = false; }

            foreach (var rigidBody in ragdollRigidbodies)
            {
                rigidBody.isKinematic = false;
                rigidBody.linearVelocity = Vector3.zero;
                rigidBody.angularVelocity = Vector3.zero;
            }

            foreach (var collider in ragdollColliders) { collider.enabled = true; }

            // Unregister the enemy based on the root enemy GameObject
            enemyManager.UnregisterEnemy(
                enemyManager.ActiveEnemies.FirstOrDefault(
                    enemy => enemy.gameObject == enemyRoot
                )
            );

            SetRagdollLayer();

            // Disable AI 
            foreach (var component in AIComponents) { component.enabled = false; }
        }

        /// <summary>
        /// This function disables ragdoll physics for 
        /// the enemy and re-enables the animator and AI components.
        /// </summary>
        public void DisableRagdoll()
        {
            if (animator != null) { animator.enabled = true; }
            foreach (var rigidBody in ragdollRigidbodies) { rigidBody.isKinematic = true; }
            foreach (var collider in ragdollColliders) { collider.enabled = false; }
            foreach (var component in AIComponents) { component.enabled = true; }
        }
    }
}
