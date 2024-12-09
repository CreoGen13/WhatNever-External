using System;
using UI.Base.BaseMenu;

namespace UI.Base.DefaultMenu
{
    public abstract class MenuView : BaseMenuView
    {
        public void OpenMenu(Action onComplete = null)
        {
            OpenMenuInternal(() =>
            {
                OnMenuOpened();

                onComplete?.Invoke();
            });
        }

        protected abstract void OpenMenuInternal(Action onComplete = null);
    }
}