using TMPro;
using UnityEngine;

namespace ForgeCore.Utility.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class FpsDisplay : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField, Range(0.5f, 2f), Tooltip("Defines how often the display is updated (s). Don't put this number too low or in the end it will impact performance.")]
        private float updateInterval = 0.5f;
        
        // Components
        private TextMeshProUGUI _fpsText;

        // Fps Display
        private float _time;
        private int _frameCount;
        
        private void Awake()
        {
            _fpsText = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            _time += Time.unscaledDeltaTime;
            _frameCount++;
            
            if (_time < updateInterval)
                return;
            
            var frameRate = Mathf.RoundToInt(_frameCount / _time);
            _fpsText.text = $"{frameRate} fps";
            
            _time -= updateInterval;
            _frameCount = 0;
        }
    }
}