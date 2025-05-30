using TMPro;
using UnityEngine;

namespace UrbanFracture.UI.HUD
{
    public class KillCounter : MonoBehaviour
    {
        public static KillCounter Instance;

        private int killCount = 0;
        public int KillCount => killCount;

        public TextMeshProUGUI KillCountText;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void RegisterKill(string killerName, string victimName)
        {
            killCount++;
            KillFeed.Instance.ShowKill(killerName, victimName);
            ShowKillCount();
        }

        public void ShowKillCount()
        {
            if (KillCountText != null)
            {
                KillCountText.text = $"Kills: {killCount}";
            }
        }

    }
}
