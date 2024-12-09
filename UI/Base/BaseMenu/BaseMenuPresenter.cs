using System;
using Core.Base.Classes;
using Core.Infrastructure.Utils;
using UI.Enums;

namespace UI.Base.BaseMenu
{
    public abstract class BaseMenuPresenter<TModel, TView> : BasePresenter<TModel, TView>
        where TModel : BaseMenuModel<TModel>
        where TView : BaseMenuView
    {
        public event Action<MenuType> OtherMenuCallEvent;
        public event Action MenuOpenedEvent
        {
            add => View.MenuOpenedEvent += value;
            remove => View.MenuOpenedEvent -= value;
        }
        public event Action MenuClosedEvent
        {
            add => View.MenuClosedEvent += value;
            remove => View.MenuClosedEvent -= value;
        }
        protected BaseMenuPresenter(TModel model, TView view)
            : base(model, view) {}
        
        protected virtual void InitButtons(){}

        protected void CallOtherMenu(MenuType menuType)
        {
            if (OtherMenuCallEvent == null)
            {
                this.LogError("Action " + nameof(CallOtherMenu) + " is empty");
                
                return;
            }
            
            OtherMenuCallEvent.Invoke(menuType);
        }
        
        public void OpenMenuInstant()
        {
            Model.MenuAnimationState = MenuAnimationState.Opened;
            Model.IsBlocked = false;
            Model.Update();
            
            View.OpenMenuInstant();
        }
        public void CloseMenuInstant()
        {
            Model.MenuAnimationState = MenuAnimationState.Closed;
            Model.IsBlocked = false;
            Model.Update();
            
            View.CloseMenuInstant();
        }
    }
}