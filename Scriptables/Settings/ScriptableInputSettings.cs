using UnityEngine;

namespace Scriptables.Settings
{
    [CreateAssetMenu(fileName = "Input Settings", menuName = "Scriptables/Input Settings", order = 4)]
    public class ScriptableInputSettings : ScriptableObject
    {
        [Header("Scroll Settings")]
        public float scrollingSpeed;
    }
}