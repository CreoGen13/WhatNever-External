using UnityEngine;

namespace Scriptables.Holders
{
    [CreateAssetMenu(fileName = "Localization Holder", menuName = "Holders/Localization Holder", order = 1)]
    public class ScriptableLocalizationHolder : ScriptableObject
    {
        public string localizationName;
        public string uiLocalizationName;
        public string choiceLocalizationName;

        public string ruLocaleCode;
        public string enLocaleCode;
    }
}