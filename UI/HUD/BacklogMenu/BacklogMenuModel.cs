using UI.Base.BaseMenu;
using UniRx;

namespace UI.HUD.BacklogMenu
{
    public class BacklogMenuModel : BaseMenuModel<BacklogMenuModel>
    {
        public BacklogMenuModel()
        {
            Subject = new BehaviorSubject<BacklogMenuModel>(this);
        }
        public override void Update()
        {
            Subject.OnNext(this);
        }
    }
}