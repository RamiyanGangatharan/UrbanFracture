using UnityEditor;
using UnityEngine;

namespace UrbanFracture.Player.Components
{
    [CustomEditor(typeof(RagdollPhysics))]
    public class RagdollPhysicsAutoFillEditor : Editor
    {
        private GameObject enemyRoot;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            RagdollPhysics ragdollPhysics = (RagdollPhysics)target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Auto Fill Ragdoll Arrays", EditorStyles.boldLabel);

            enemyRoot = (GameObject)EditorGUILayout.ObjectField("Enemy Root GameObject", enemyRoot, typeof(GameObject), true);

            if (GUILayout.Button("Auto Fill") && enemyRoot != null)
            {
                Rigidbody[] rigidbodies = enemyRoot.GetComponentsInChildren<Rigidbody>(true);
                Collider[] colliders = enemyRoot.GetComponentsInChildren<Collider>(true);

                Undo.RecordObject(ragdollPhysics, "Auto Fill Ragdoll Arrays");

                ragdollPhysics.ragdollRigidbodies = rigidbodies;
                ragdollPhysics.ragdollColliders = colliders;

                // Mark the object dirty so Unity knows to save changes
                EditorUtility.SetDirty(ragdollPhysics);

                Debug.Log($"Auto-filled {rigidbodies.Length} rigidbodies and {colliders.Length} colliders from '{enemyRoot.name}'.");
            }
            else if (enemyRoot == null)
            {
                EditorGUILayout.HelpBox("Assign the enemy root GameObject to auto-fill arrays.", MessageType.Info);
            }
        }
    }

}
