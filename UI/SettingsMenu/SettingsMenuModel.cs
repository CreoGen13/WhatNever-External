using UI.Base.TabMenu;
using UniRx;
using UnityEngine.Localization.Settings;

namespace UI.SettingsMenu
{
    public class SettingsMenuModel : TabMenuModel<SettingsMenuModel, SettingsMenuType>
    {
        public SettingsMenuModel()
        {
            Subject = new BehaviorSubject<SettingsMenuModel>(this);
        }
        public override void Update()
        {
            Subject.OnNext(this);
        }
    }
}