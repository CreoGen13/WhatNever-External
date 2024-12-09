using Core.Infrastructure.Services;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.Mechanics.Bushes
{
    public class Bush : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private const string ANIMATION_NAME = "Blinking";
        
        [SerializeField] private RectTransform center;
        [SerializeField] private Image bush;
        [SerializeField] private Image outline;
        [SerializeField] private Image outline2;
        [SerializeField] private Animator animator;

        public Vector2 CenterPosition => center.localPosition;
        public bool IsDragging { get; private set; }
        public bool IsActive {get; private set;}
        
        private float _fadingDuration;
        
        public void Init(float fadingDuration)
        {
            _fadingDuration = fadingDuration;
            IsActive = true;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            animator.enabled = false;
            outline2.color = new Color(1, 1,1, 0);
            
            IsDragging = true;
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            IsDragging = false;

            if (!IsActive)
            {
                return;
            }
            
            animator.enabled = true;
            animator.Play(ANIMATION_NAME, 0, 0f);
        }

        public void Drag(InputService.Input input)
        {
            center.anchoredPosition += input.Delta;
        }

        public void Complete()
        {
            IsActive = false;
            
            animator.enabled = false;
            
            DOTween.To(() => bush.color, value =>
            {
                bush.color = value;
                outline.color = value;

                if (outline2.color.a > value.a)
                {
                    outline2.color = value;
                }
            }, new Color(255, 255, 255, 0), _fadingDuration).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }
    }
}