using Core.Base.Classes;

namespace UI.Base.BaseMenu
{
    public abstract class BaseMenuModel<T> : BaseModel<T>
        where T : BaseMenuModel<T>
    {
        public virtual bool IsBusy => IsBlocked || MenuAnimationState == MenuAnimationState.Opening || MenuAnimationState == MenuAnimationState.Closing;
        
        public bool IsBlocked;
        public MenuAnimationState MenuAnimationState = MenuAnimationState.Closed;
    }
}