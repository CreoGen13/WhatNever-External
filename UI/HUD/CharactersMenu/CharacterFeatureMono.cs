using TMPro;
using UnityEngine;

namespace UI.HUD.CharactersMenu
{
    public class CharacterFeatureMono : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textHidden;
        [SerializeField] private TextMeshProUGUI textFeature;

        public void Activate()
        {
            textHidden.gameObject.SetActive(false);
            textFeature.gameObject.SetActive(true);
        }
        public void Deactivate()
        {
            textHidden.gameObject.SetActive(true);
            textFeature.gameObject.SetActive(false);
        }
    }
}