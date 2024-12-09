using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD.CharactersMenu
{
    public class CharacterDescriptionMono : MonoBehaviour
    {
        [SerializeField] private Image hiddenAvatar;
        [SerializeField] private Image avatar;
        [SerializeField] private CharacterButtonMono buttonCharacter;
        [SerializeField] private CharacterFeatureMono[] features;

        public Button Button => buttonCharacter.Button;

        public void BaseStart()
        {
            Deactivate();
        }
        public void Activate()
        {
            hiddenAvatar.gameObject.SetActive(false);
            avatar.gameObject.SetActive(true);
            buttonCharacter.Activate();
            
            Open();
        }
        public void Deactivate()
        {
            hiddenAvatar.gameObject.SetActive(true);
            avatar.gameObject.SetActive(false);
            buttonCharacter.Deactivate();
            
            foreach (var characterFeature in features)
            {
                characterFeature.Deactivate();
            }
        }

        public void UpdateFeatures(bool [] featuresStates)
        {
            for (int i = 0; i < featuresStates.Length; i++)
            {
                if (featuresStates[i])
                {
                    features[i].Activate();
                }
                else
                {
                    features[i].Deactivate();
                }
            }
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }
        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}