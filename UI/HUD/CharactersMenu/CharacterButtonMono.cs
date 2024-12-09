using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD.CharactersMenu
{
    public class CharacterButtonMono : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textHidden;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Button button;

        public Button Button => button;
        public void Activate()
        {
            textHidden.gameObject.SetActive(false);
            text.gameObject.SetActive(true);
            
            button.interactable = true;
        }
        public void Deactivate()
        {
            textHidden.gameObject.SetActive(true);
            text.gameObject.SetActive(false);
            
            button.interactable = false;
        }
    }
}