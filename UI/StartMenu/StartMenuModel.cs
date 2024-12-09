using UI.Base.BaseMenu;
using UniRx;

namespace UI.StartMenu
{
    public class StartMenuModel : BaseMenuModel<StartMenuModel>
    {
        public StartMenuModel()
        {
            Subject = new BehaviorSubject<StartMenuModel>(this);
        }
        public override void Update()
        {
            Subject.OnNext(this);
        }
    }
}