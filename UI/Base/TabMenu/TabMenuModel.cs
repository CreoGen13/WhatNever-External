using System;
using UI.Base.BaseMenu;

namespace UI.Base.TabMenu
{
    public abstract class TabMenuModel<T, TE> : BaseMenuModel<T> 
        where T : TabMenuModel<T, TE>
        where TE : Enum
    {
        public TE CurrentTab;
    }
}