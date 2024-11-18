using ForgeCore.Utility.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ForgeCore.Notifications
{
    public class Notification : MonoBehaviour
    {
        [Header("Fade")] 
        [SerializeField] private float fadeInTime = 0.5f;
        [SerializeField] private float lifeTime = 3f;
        [SerializeField] private float fadeOutTime = 0.5f;

        [Header("Components")] 
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Image backgroundImage;

        public void SetNotification(string message)
        {
            messageText.text = message;

            StartCoroutine(UIUtility.FadeInOutImage(fadeInTime, lifeTime, fadeOutTime, backgroundImage));
            StartCoroutine(UIUtility.FadeInOutText(fadeInTime, lifeTime, fadeOutTime, messageText));
        }
    }
}