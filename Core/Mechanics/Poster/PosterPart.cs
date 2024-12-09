using Core.Infrastructure.Services;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Mechanics.Poster
{
    public class PosterPart : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform rectTransform;

        public Vector2 CenterPosition => rectTransform.localPosition;
        public bool IsDragging {get; private set;}
        public bool IsActive {get; private set;}

        public void Init()
        {
            IsActive = true;
        }

        public void Drag(InputService.Input input)
        {
            rectTransform.anchoredPosition += input.Delta;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!IsActive)
            {
                return;
            }
            
            IsDragging = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!IsActive)
            {
                return;
            }
            
            IsDragging = false;
        }

        public void Complete(Vector2 position)
        {
            rectTransform.localPosition = position;
            
            IsDragging = false;
            IsActive = false;
        }
    }
}