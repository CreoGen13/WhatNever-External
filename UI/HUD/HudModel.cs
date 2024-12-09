using UI.Base.BaseMenu;
using UniRx;

namespace UI.HUD
{
    public class HudModel : BaseMenuModel<HudModel>
    {
        public bool IsPaused => IsBusy || MenuAnimationState != MenuAnimationState.Opened;
        public bool HasCharactersList;
        public bool HasItemsList;

        public HudModel()
        {
            Subject = new BehaviorSubject<HudModel>(this);
        }
        public override void Update()
        {
            Subject.OnNext(this);
        }
    }
}