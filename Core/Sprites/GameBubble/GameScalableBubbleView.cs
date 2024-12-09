using Core.Infrastructure.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Sprites.GameBubble
{
    public class GameScalableBubbleView : GameBubbleView
    {
        [SerializeField] private Image fakeImage;
        [SerializeField] private RectTransform maxWidth;
        [SerializeField] private LayoutElement layoutElement;

        public override void SetSprite(Sprite sprite, Color color, string text)
        {
            base.SetSprite(sprite, color, text);
            
            fakeImage.sprite = sprite;
            fakeImage.type = Image.Type.Simple;
        }
        
        public override void SetTransform(Vector3 position, Vector3 scaleAndRotation, Stage stage = Stage.Third)
        {
            base.SetTransform(position, scaleAndRotation, stage);

            var maxTextSize = new Vector2(GameSettings.dialogBubbleMaxWidth * scaleAndRotation.x, maxWidth.rect.size.y);
            var backgroundSize = new Vector2(GameSettings.dialogBubbleBackgroundWidth - maxTextSize.x, image.rectTransform.sizeDelta.y);
            var difference = GameSettings.dialogBubbleSidesWidth * 2 - backgroundSize.x;

            maxWidth.sizeDelta = maxTextSize;
            
            image.rectTransform.localScale = new Vector2(scaleAndRotation.x, scaleAndRotation.y);
            image.rectTransform.sizeDelta = backgroundSize;

            layoutElement.minWidth = Mathf.Clamp(difference, 0, int.MaxValue);
        }
        
        public override void SetDefaultSize()
        {
            fakeImage.SetNativeSize();
        }
    }
}