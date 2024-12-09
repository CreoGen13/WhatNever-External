using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD.PencilMenu
{
    public class PencilItemMono : MonoBehaviour
    {
        [SerializeField] private Button pencilButton;
        [SerializeField] private Image pencilImage;
        [SerializeField] private Image backgroundImage;
        
        public Button Button => pencilButton;

        public void SetImageAlpha(float alpha)
        {
            pencilImage.color = new Color(pencilImage.color.r, pencilImage.color.g, pencilImage.color.b, alpha);
            backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, alpha);
        }

        public Sprite GetSprite()
        {
            return pencilImage.sprite;
        }
    }
}
