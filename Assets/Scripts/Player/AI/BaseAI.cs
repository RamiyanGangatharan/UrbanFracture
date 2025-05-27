using UnityEngine;

namespace UrbanFracture.Player.AI
{
    public abstract class BaseAI : MonoBehaviour
    {
        protected Transform playerTransform;

        protected virtual void Awake()
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (playerTransform == null) { Debug.LogError("Player not found."); }
        }

        protected virtual void Start() { Initialize(); }

        // Called once at start for setup, override for your own initialization
        protected abstract void Initialize();

        // Called every frame to update behavior
        protected abstract void Tick();

        private void Update()
        {
            if (playerTransform == null) { return; }
            Tick();
        }
    }
}
