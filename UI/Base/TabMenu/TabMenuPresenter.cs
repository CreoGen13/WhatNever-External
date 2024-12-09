using System;
using UI.Base.BaseMenu;

namespace UI.Base.TabMenu
{
    public abstract class TabMenuPresenter<TModel, TView, TE> : BaseMenuPresenter<TModel, TView>
        where TModel : TabMenuModel<TModel, TE>
        where TView : TabMenuView<TE>
        where TE : Enum
    {
        protected TabMenuPresenter(TModel model, TView view)
            : base(model, view) {}

        public virtual void OpenMenuWithTab(TE tabType, Action onComplete = null)
        {
            Model.CurrentTab = tabType;
            Model.MenuAnimationState = MenuAnimationState.Opening;
            Model.Update();
            
            View.OpenMenuWithTab(tabType, () =>
            {
                Model.MenuAnimationState = MenuAnimationState.Opened;
                Model.Update();
                
                onComplete?.Invoke();
            });
        }
        public virtual void OpenTab(TE tabType, Action onComplete = null)
        {
            Model.MenuAnimationState = MenuAnimationState.Opening;
            Model.Update();
            
            View.CloseTab(Model.CurrentTab, () =>
            {
                Model.CurrentTab = tabType;
                Model.Update();

                View.OpenTab(Model.CurrentTab, () =>
                {
                    Model.MenuAnimationState = MenuAnimationState.Opened;
                    Model.Update();
                    
                    onComplete?.Invoke();
                });
            });
        }
        public virtual void CloseMenu(Action onComplete = null)
        {
            Model.MenuAnimationState = MenuAnimationState.Closing;
            Model.Update();
            
            View.CloseMenu(() =>
            {
                Model.MenuAnimationState = MenuAnimationState.Closed;
                Model.Update();
                
                onComplete?.Invoke();
            });
        }
    }
}