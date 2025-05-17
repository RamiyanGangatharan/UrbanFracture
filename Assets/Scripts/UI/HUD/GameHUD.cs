using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UrbanFracture.Combat;
using UrbanFracture.Core.Player;

namespace UrbanFracture.UI.HUD
{
    /// <summary>
    /// Manages and updates the in-game HUD (Head-Up Display) for displaying the player's health, ammo count,
    /// equipped weapon name, and weapon icon. The HUD is updated every frame to reflect the current state of
    /// the player's health and equipped weapon.
    /// </summary>
    public class GameHUD : MonoBehaviour
    {
        [Header("References")]
        public TextMeshProUGUI AmmoText;
        public TextMeshProUGUI HealthText;
        public TextMeshProUGUI WeaponName;
        public RawImage WeaponImage;
        public RawImage WeaponIcon;
        public RawImage AmmoIcon;

        private FirstPersonController player;
        private Gun currentGun;

        /// <summary>
        /// Initializes the HUD by retrieving the player controller and setting up the necessary listeners.
        /// </summary>
        private void Start()
        {
            player = GetComponent<FirstPersonController>();

            if (player != null)
            {
                currentGun = player.EquippedGun;
                if (player.PlayerHealth != null) { player.PlayerHealth.OnHealthChanged.AddListener(UpdateHealth); }
            }

            UpdateHUD();
        }

        /// <summary>
        /// Updates the HUD every frame if the player and equipped gun are available.
        /// </summary>
        private void Update() { if (player != null && currentGun != null) { UpdateHUD(); } }

        /// <summary>
        /// Updates the displayed HUD elements with the current values for ammo, 
        /// weapon name, weapon icon, and player's health.
        /// </summary>
        public void UpdateHUD()
        {
            if (currentGun != null)
            {
                bool isHolstered = currentGun.IsHolstered();

                // Fade or show weapon UI elements
                SetWeaponUIAlpha(isHolstered ? 0f : 1f);

                if (!isHolstered)
                {
                    AmmoText.text = $"{currentGun.currentAmmo} / {currentGun.gunData.MagazineSize}";
                    WeaponName.text = currentGun.gunData.WeaponName;

                    if (WeaponImage != null && currentGun.gunData.WeaponIcon != null)
                    {
                        WeaponImage.texture = currentGun.gunData.WeaponIcon;
                    }
                }
            }

            if (player.PlayerHealth != null)
            {
                HealthText.text = $"HP: {Mathf.RoundToInt(player.PlayerHealth.CurrentHealth)}";
            }
        }

        /// <summary>
        /// This makes the HUD for the weapons fade out or in based on the alpha value provided.
        /// </summary>
        /// <param name="alpha"></param>
        private void SetWeaponUIAlpha(float alpha)
        {
            if (AmmoText != null)
            {
                Color c = AmmoText.color;
                c.a = alpha;
                AmmoText.color = c;
            }

            if (WeaponName != null)
            {
                Color c = WeaponName.color;
                c.a = alpha;
                WeaponName.color = c;
            }

            if (WeaponImage != null)
            {
                Color c = WeaponImage.color;
                c.a = alpha;
                WeaponImage.color = c;
            }

            if (WeaponIcon != null)
            {
                Color c = WeaponIcon.color;
                c.a = alpha;
                WeaponIcon.color = c;
            }

            if (AmmoIcon != null)
            {
                Color c = AmmoIcon.color;
                c.a = alpha;
                AmmoIcon.color = c;
            }
        }

        /// <summary>
        /// Updates the player's health text based on the current health value.
        /// </summary>
        /// <param name="currentHealth">The player's current health value.</param>
        private void UpdateHealth(float currentHealth) { HealthText.text = $"HP: {Mathf.RoundToInt(currentHealth)}"; }
    }
}
