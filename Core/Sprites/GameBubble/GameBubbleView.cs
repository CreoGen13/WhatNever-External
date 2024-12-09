using System;
using Core.Infrastructure.Enums;
using Core.Sprites.Base;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Core.Sprites.GameBubble
{
    public class GameBubbleView : BaseGameGraphicsView
    {
        [Header("Bubble")]
        [SerializeField] private TextMeshProUGUI bubbleText;
        
        public event Action EnableEvent;
        
        public virtual void SetSprite(Sprite sprite, Color color, string text)
        {
            image.sprite = sprite;
            bubbleText.text = text;
            bubbleText.color = color;
        }
        
        public override void SetTransform(Vector3 position, Vector3 scaleAndRotation, Stage stage = Stage.Third)
        {
            base.SetTransform(position, scaleAndRotation, stage);
            
            rectTransform.sizeDelta = new Vector2(
                rectTransform.sizeDelta.x * scaleAndRotation.x,
                rectTransform.sizeDelta.y * scaleAndRotation.y);
            rectTransform.localScale = Vector3.one;
        }

        public void EnableSprite(ArrivalType arrivalType)
        {
            image.enabled = true;
            
            switch (arrivalType)
            {
                case ArrivalType.Instant:
                {
                    EnableEvent?.Invoke();
                    
                    break;
                }
                case ArrivalType.ShowUp:
                {
                    var tmpScale = transform.localScale;
                    rectTransform.localScale = new Vector3(0, 0, 1);

                    Sequence?.Kill();
                    Sequence = DOTween.Sequence();
                    Sequence.Join(rectTransform.DOScale(
                        new Vector3(tmpScale.x, tmpScale.y, 1),
                        GameSettings.bubbleShowUpDuration));
                    Sequence.OnComplete(() => { EnableEvent?.Invoke(); });
                    Sequence.Play();
                    
                    break;
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            
            bubbleText.text = "";
        }
    }
}