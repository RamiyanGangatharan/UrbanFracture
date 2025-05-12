using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UrbanFracture.UI.Components
{
    /// <summary>
    /// Adds hover and click audio feedback to a UI Button.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class UIButtonAudio : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] private AudioSource hoverSound;
        [SerializeField] private AudioSource clickSound;

        private void Awake()
        {
            var button = GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(PlayClickSound);

                EventTrigger trigger = gameObject.GetComponent<EventTrigger>() ?? gameObject.AddComponent<EventTrigger>();

                var entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
                entry.callback.AddListener(_ => PlayHoverSound());
                trigger.triggers.Add(entry);
            }
        }

        private void PlayHoverSound() { if (hoverSound != null) hoverSound.Play(); }
        private void PlayClickSound() { if (clickSound != null) clickSound.Play(); }
    }
}
