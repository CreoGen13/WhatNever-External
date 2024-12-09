using System;
using DG.Tweening;
using UI.Base.BaseMenu;

namespace UI.Base.TabMenu
{
    public abstract class TabMenuView<TEnum> : BaseMenuView
        where TEnum : Enum
    {
        public void OpenMenuWithTab(TEnum tabType, Action onComplete = null)
        {
            OpenMenuWithTabInternal(tabType, () =>
            {
                OnMenuOpened();
                onComplete?.Invoke();
            });
        }
        protected abstract void OpenMenuWithTabInternal(TEnum tabType, Action onComplete = null);
        
        public abstract Sequence OpenTab(TEnum tabType, Action callback = null);
        public abstract Sequence CloseTab(TEnum tabType, Action callback = null);
    }
}