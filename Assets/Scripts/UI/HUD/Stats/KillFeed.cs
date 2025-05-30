using System.Collections;
using TMPro;
using UnityEngine;

namespace UrbanFracture.UI.HUD
{
    public class KillFeed : MonoBehaviour
    {
        public static KillFeed Instance;

        [Header("References")]
        public GameObject killFeedEntryPrefab;
        public Transform feedParent;
        public float entryLifetime = 5f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void ShowKill(string killer, string victim)
        {
            GameObject entry = Instantiate(killFeedEntryPrefab, feedParent);
            TMP_Text text = entry.GetComponentInChildren<TMP_Text>();
            text.text = $"{killer} killed {victim}";
            StartCoroutine(FadeAndDestroy(entry));
        }

        private IEnumerator FadeAndDestroy(GameObject entry)
        {
            yield return new WaitForSeconds(entryLifetime);
            Destroy(entry);
        }
    }
}
