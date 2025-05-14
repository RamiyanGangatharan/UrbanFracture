using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UrbanFracture.Combat;
using UrbanFracture.Core.Player;

namespace UrbanFracture.UI.HUD
{
    public class GameHUD : MonoBehaviour
    {
        [Header("References")]
        public TextMeshProUGUI AmmoText;
        public TextMeshProUGUI HealthText;
        public TextMeshProUGUI WeaponName;
        public RawImage WeaponImage;
        public TextMeshProUGUI ReloadText; 

        private FirstPersonController player;
        private Gun currentGun;

        private void Start()
        {
            player = GetComponent<FirstPersonController>();

            if (player != null)
            {
                currentGun = player.EquippedGun;

                if (player.PlayerHealth != null)
                    player.PlayerHealth.OnHealthChanged.AddListener(UpdateHealth);
            }

            UpdateHUD();
        }

        private void Update()
        {
            if (player != null && currentGun != null)
            {
                UpdateHUD();
            }
        }

        public void UpdateHUD()
        {
            if (currentGun != null)
            {
                AmmoText.text = $"{currentGun.currentAmmo} / {currentGun.gunData.MagazineSize}";
                WeaponName.text = currentGun.gunData.WeaponName;

                if (WeaponImage != null && currentGun.gunData.WeaponIcon != null)
                    WeaponImage.texture = currentGun.gunData.WeaponIcon;
            }

            if (player.PlayerHealth != null)
            {
                HealthText.text = $"HP: {Mathf.RoundToInt(player.PlayerHealth.CurrentHealth)}";
            }
        }

        private void UpdateHealth(float currentHealth)
        {
            HealthText.text = $"HP: {Mathf.RoundToInt(currentHealth)}";
        }

    }
}
