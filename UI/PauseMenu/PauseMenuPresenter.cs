using System;
using UI.Base.DefaultMenu;
using UI.Enums;
using Zenject;

namespace UI.PauseMenu
{
    public class PauseMenuPresenter : MenuPresenter<PauseMenuModel, PauseMenuView>
    {
        public event Action EndGameEvent;
        
        [Inject]
        public PauseMenuPresenter(PauseMenuModel model, PauseMenuView view)
            : base(model, view)
        {
            InitSubscriptions();
            InitButtons();
        }

        protected sealed override void InitSubscriptions()
        {
            AddSubscriptionWithDistinct(model => model.IsBusy, (value) =>
            {
                View.SetButtonsInteractable(!value);
            });
        }
        protected sealed override void InitButtons()
        {
            View.OnClickButtonSettings += OnClickButtonSettings;
            View.OnClickButtonContinue += OnClickButtonContinue;
            View.OnClickButtonMainMenu += OnClickButtonMainMenu;
        }

        #region BUTTONS

        private void OnClickButtonSettings()
        {
            CloseMenu(() =>
            {
                CallOtherMenu(MenuType.SettingsMenu);
            });
        }
        private void OnClickButtonMainMenu()
        {
            CloseMenu(() =>
            {
                EndGameEvent?.Invoke();
            });
        }
        private void OnClickButtonContinue()
        {
            CloseMenu(() =>
            {
                CallOtherMenu(MenuType.Hud);
            });
        }

        #endregion
    }
}