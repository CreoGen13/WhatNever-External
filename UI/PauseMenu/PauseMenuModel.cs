using UI.Base.BaseMenu;
using UniRx;

namespace UI.PauseMenu
{
    public class PauseMenuModel : BaseMenuModel<PauseMenuModel>
    {
        public PauseMenuModel()
        {
            Subject = new BehaviorSubject<PauseMenuModel>(this);
        }
        public override void Update()
        {
            Subject.OnNext(this);
        }
    }
}