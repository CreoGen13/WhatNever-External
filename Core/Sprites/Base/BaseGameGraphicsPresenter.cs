using System;
using Core.Base.Classes;
using Core.Infrastructure.Enums;
using UnityEngine;

namespace Core.Sprites.Base
{
    public abstract class BaseGameGraphicsPresenter<TModel, TView> : BasePresenter<TModel, TView>, IGraphicsPresenter
        where TModel : BaseGameGraphicsModel<TModel>
        where TView : BaseGameGraphicsView
    {
        public abstract SpriteTag SpriteTag { get; }
        public Transform Transform => View.transform;

        protected BaseGameGraphicsPresenter(TModel model, TView view): base(model, view) { }
        
        public void SetParent(Transform parent)
        {
            View.SetParent(parent);
        }
        public void SetSpawned(bool spawned)
        {
            View.gameObject.SetActive(spawned);

            Model.IsSpawned = spawned;
            Model.Update();
        }
        public void SetActive(bool active)
        {
            Model.IsActive = active;
            Model.Update();
        }

        public abstract void EnableSprite();
        public virtual void DisableAndReturn(bool withAnimation, bool waitForAnimationEnd = false, Action onComplete = null)
        {
            if (withAnimation)
            {
                if (waitForAnimationEnd)
                {
                    View.DisableSprite(() =>
                    {
                        onComplete?.Invoke();
                        OnReturnInvoke();
                    });
                    return;
                }
            
                View.DisableSprite();
            }
            
            onComplete?.Invoke();
            OnReturnInvoke();
        }

        protected abstract void OnReturnInvoke();
        
        protected virtual void Reset()
        {
            View.Reset();
        }
        
        public void RepositionGameObject(Vector3 moveVector)
        {
            View.RepositionGameObject(moveVector);
        }
        
        public void SetTransparency(float alpha)
        {
            View.SetTransparency(alpha);
        }
    }
}