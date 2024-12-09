using System;

namespace UI.HUD.PencilMenu
{
    public struct ItemState
    {
        public readonly bool IsActive;
        public readonly Action Callback;

        public ItemState(bool isActive, Action callback)
        {
            IsActive = isActive;
            Callback = callback;
        }
    }
}