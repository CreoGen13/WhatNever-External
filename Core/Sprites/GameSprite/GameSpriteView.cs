using System;
using Core.Infrastructure.Enums;
using Core.Sprites.Base;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Sprites.GameSprite
{
    public class GameSpriteView : BaseGameGraphicsView
    {
        [Header("Sprite")]
        [SerializeField] private Button button;
        [SerializeField] private Animator animator;

        public event Action AnimationEndEvent;
        public event Action ButtonClickEvent;
        public event Action EnableEvent;
        
        public Button Button => button;
        public AudioSource AudioSource => audioSource;
        public Animator Animator => animator;

        public void Init()
        {
            button.onClick.AddListener(() =>
            {
                ButtonClickEvent?.Invoke();
            });
        }

        public void SetSprite(Sprite sprite, Color color)
        {
            image.sprite = sprite;
            image.color = color;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }
        
        public override void SetTransform(Vector3 position, Vector3 scaleAndRotation, Stage stage = Stage.Third)
        {
            base.SetTransform(position, scaleAndRotation, stage);
            
            rectTransform.localScale = new Vector3(scaleAndRotation.x, scaleAndRotation.y, 1);
        }
        
        public void SetAnimation(RuntimeAnimatorController animatorController)
        {
            animator.runtimeAnimatorController = animatorController;
            animator.Play(EntryName);
            animator.Update(0);
            image.SetNativeSize();
            
            animator.speed = 0;
        }
        
        public void SetAnimationState(bool state)
        {
            if (state)
            {
                animator.speed = 1;

                if (AnimationEndEvent == null)
                {
                    return;
                }
            
                var duration = Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
                
                Sequence?.Kill();
                Sequence = DOTween.Sequence();
                Sequence.AppendInterval(duration);
                Sequence.AppendCallback(() => { AnimationEndEvent?.Invoke(); });
            }
            else
            {
                animator.speed = 0;
                
                Sequence?.Kill();
                AnimationEndEvent?.Invoke();
            }
        }

        public void EnableSprite(ArrivalType arrivalType, Vector3 endPosition)
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
                case ArrivalType.Moving:
                {
                    Sequence?.Kill();
                    Sequence = DOTween.Sequence();
                    Sequence.Join(rectTransform.DOLocalMove(
                        endPosition,
                        GameSettings.arrivalTypeMovingDuration));
                    Sequence.OnComplete(() => { EnableEvent?.Invoke(); });
                    Sequence.Play();
                    
                    break;
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            
            animator.runtimeAnimatorController = null;
        }
    }
}