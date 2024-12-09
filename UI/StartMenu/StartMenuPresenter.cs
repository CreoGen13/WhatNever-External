using System;
using UI.Base.DefaultMenu;
using UI.Enums;
using Zenject;

namespace UI.StartMenu
{
    public class StartMenuPresenter : MenuPresenter<StartMenuModel, StartMenuView>
    {
        public event Action StartGameEvent;
        
        [Inject]
        public StartMenuPresenter(StartMenuModel model, StartMenuView view)
            : base(model, view)
        {
            InitSubscriptions();
            InitButtons();
        }
        protected sealed override void InitSubscriptions()
        {
            AddSubscriptionWithDistinct(model => model.IsBusy, value =>
            {
                View.SetButtonsInteractable(!value);
            });
        }

        protected sealed override void InitButtons()
        {
            View.OnClickButtonAbout += OnClickClickButtonAbout;
            View.OnClickButtonSettings += OnClickButtonSettings;
            View.OnClickButtonStart += OnClickButtonStart;
        }

        #region BUTTONS

        private void OnClickButtonStart()
        {
            CloseMenu(() =>
            {
                StartGameEvent?.Invoke();
            });
        }

        private void OnClickButtonSettings()
        {
            CloseMenu(() =>
            {
                CallOtherMenu(MenuType.SettingsMenu);
            });
        }

        private void OnClickClickButtonAbout()
        {
            CloseMenu(() =>
            {
                CallOtherMenu(MenuType.AboutMenu);
            });
        }

        #endregion
        
    }
}