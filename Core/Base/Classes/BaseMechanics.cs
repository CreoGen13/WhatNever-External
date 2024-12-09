using System;
using Core.Infrastructure.Services;
using UnityEngine;
using Zenject;

namespace Core.Base.Classes
{
    public abstract class BaseMechanics : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;

        public event Action<BaseMechanics> DestroyEvent;
        protected Action CompleteAction;
        
        protected virtual bool IsBusy => !IsEnabled || IsValidating;
        
        protected bool IsEnabled;
        protected bool IsValidating;
        
        protected InputService InputService;

        [Inject]
        private void Construct(InputService inputService)
        {
            InputService = inputService;
        }

        public virtual void Activate(Vector3 position)
        {
            rectTransform.localPosition = position;
            IsEnabled = true;
        }
        
        public virtual void ManualUpdate(){}

        public void SetOnComplete(Action onComplete)
        {
            CompleteAction = onComplete;
        }

        public void Pause()
        {
            IsEnabled = false;
        }

        public void Continue()
        {
            IsEnabled = true;
        }

        protected virtual void Deactivate()
        {
            IsEnabled = false;
            
            DestroyEvent?.Invoke(this);
            
            Destroy(gameObject);
            
            CompleteAction?.Invoke();
        }

        public void Destroy()
        {
            DestroyEvent?.Invoke(this);
            
            Destroy(gameObject);
        }
    }
}