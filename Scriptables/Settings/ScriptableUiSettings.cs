using UnityEngine;

namespace Scriptables.Settings
{
    [CreateAssetMenu(fileName = "UI Settings", menuName = "Scriptables/UI Settings", order = 2)]
    public class ScriptableUiSettings : ScriptableObject
    {
        [Header("Default UI settings")]
        public float uiAnimationDuration;
        public Color backgroundFadeColor;
        
        [Header("Shake Settings")]
        public float shakeDuration;
        public Vector3 shakeStrength;
        public int shakeVibrato;
        public float shakeRandomness;
    }
}