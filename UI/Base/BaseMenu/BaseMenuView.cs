using System;
using Core.Base.Classes;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Base.BaseMenu
{
    [RequireComponent(typeof(Canvas), typeof(GraphicRaycaster))]
    public abstract class BaseMenuView : BaseView
    {
        public event Action MenuOpenedEvent;
        public event Action MenuClosedEvent;
        
        [Header("Main")]
        [SerializeField] protected RectTransform windowTransform;
        [SerializeField] protected Image background;

        protected virtual void InitButtons(){}
        
        public abstract void OpenMenuInstant();
        public abstract void CloseMenuInstant();

        public void CloseMenu(Action onComplete = null)
        {
            CloseMenuInternal(() =>
            {
                MenuClosedEvent?.Invoke();

                onComplete?.Invoke();
            });
        }
        protected abstract void CloseMenuInternal(Action onComplete = null);
        
        protected void OnMenuOpened()
        {
            MenuOpenedEvent?.Invoke();
        }
    }
}