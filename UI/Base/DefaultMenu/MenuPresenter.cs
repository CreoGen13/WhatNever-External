using System;
using UI.Base.BaseMenu;

namespace UI.Base.DefaultMenu
{
    public abstract class MenuPresenter<TModel, TView> : BaseMenuPresenter<TModel, TView>
        where TModel : BaseMenuModel<TModel>
        where TView : MenuView
    {
        protected MenuPresenter(TModel model, TView view)
            : base(model, view)
        {
            View.CloseMenuInstant();
        }

        public virtual void OpenMenu(Action onComplete = null)
        {
            if (Model.MenuAnimationState == MenuAnimationState.Opened)
            {
                return;
            }
            
            Model.MenuAnimationState = MenuAnimationState.Opening;
            Model.Update();
            
            View.OpenMenu(() =>
            {
                Model.MenuAnimationState = MenuAnimationState.Opened;
                Model.Update();
                
                onComplete?.Invoke();
            });
        }
        public virtual void CloseMenu(Action onComplete = null)
        {
            if (Model.MenuAnimationState == MenuAnimationState.Closed)
            {
                return;
            }
            
            Model.MenuAnimationState = MenuAnimationState.Closing;
            Model.Update();
            
            View.CloseMenu(() =>
            {
                Model.MenuAnimationState = MenuAnimationState.Closed;
                Model.Update();
                
                onComplete?.Invoke();
            });
        }

        public virtual void Clear()
        {
            CloseMenuInstant();
        }
    }
}