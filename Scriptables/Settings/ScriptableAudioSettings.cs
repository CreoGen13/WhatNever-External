using UnityEngine;

namespace Scriptables.Settings
{
    [CreateAssetMenu(fileName = "Audio Settings", menuName = "Scriptables/Audio Settings", order = 3)]
    public class ScriptableAudioSettings : ScriptableObject
    {
        public float audioChangeDuration;
    }
}