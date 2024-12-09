using UI.Base.BaseMenu;
using UniRx;

namespace UI.MainMenu
{
    public class MainMenuModel : BaseMenuModel<MainMenuModel>
    {
        public MainMenuType CurrentMenu = MainMenuType.StartMenu;
        public bool IsOpened;
        
        public MainMenuModel()
        {
            Subject = new BehaviorSubject<MainMenuModel>(this);
            
            IsOpened = true;
            MenuAnimationState = MenuAnimationState.Opened;
        }
        public override void Update()
        {
            Subject.OnNext(this);
        }
    }
}