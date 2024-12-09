using UnityEngine;

namespace Scriptables.Holders
{
    [CreateAssetMenu(fileName = "Sounds Holder", menuName = "Holders/Sounds Holder", order = 2)]
    public class ScriptableSoundsHolder : ScriptableObject
    {
        [Header("Music")]
        public AudioClip mainThemeAudioClip;
        
        [Header("Button and Bubble")]
        public AudioClip buttonAudioClip;
        public AudioClip bubbleAudioClip;
        
        [Header("Menu")]
        public AudioClip menuBigTreeIntro;
        public AudioClip menuBigTreeOutro;
        public AudioClip menuSmallTreeIntro;
        public AudioClip menuSmallTreeOutro;
        public AudioClip menuOutro;
    }
}