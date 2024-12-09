using System;
using DG.Tweening;
using UnityEngine;

namespace Core.Base.Classes
{
    public abstract class BaseView: MonoBehaviour
    {
        public event Action OnDispose;
        
        protected Sequence Sequence;

        protected virtual void OnDestroy()
        {
            Dispose();
        }
        private void Dispose()
        {
            OnDispose?.Invoke();
        }
    }
}