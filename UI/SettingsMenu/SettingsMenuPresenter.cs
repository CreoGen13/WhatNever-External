using System;
using Scriptables.Holders;
using Scriptables.Settings;
using UI.Base.TabMenu;
using UI.Enums;
using UnityEngine.Localization.Settings;
using Zenject;

namespace UI.SettingsMenu
{
    public class SettingsMenuPresenter : TabMenuPresenter<SettingsMenuModel, SettingsMenuView, SettingsMenuType>
    {
        private readonly LocalizationSettings _localizationSettings;
        private ScriptableLocalizationHolder _localizationHolder;
        public event Action StartGameEvent;
        
        [Inject]
        public SettingsMenuPresenter(SettingsMenuModel model,
            SettingsMenuView view,
            LocalizationSettings localizationSettings,
            ScriptableLocalizationHolder localizationHolder)
            : base(model, view)
        {
            _localizationHolder = localizationHolder;
            _localizationSettings = localizationSettings;
            
            InitSubscriptions();
            InitButtons();
        }

        protected sealed override void InitSubscriptions()
        {
            AddSubscriptionWithDistinct(model => model.IsBusy, value =>
            {
                View.SetButtonsInteractable(!value, Model.CurrentTab == SettingsMenuType.Settings);
            });
        }

        protected sealed override void InitButtons()
        {
            View.OnClickButtonLanguageRussian += () => { OnClickButtonLanguage(_localizationHolder.ruLocaleCode); };
            View.OnClickButtonLanguageEnglish += () => { OnClickButtonLanguage(_localizationHolder.enLocaleCode); };
            View.OnClickButtonSettingsOk += OnClickButtonOk;
            View.OnClickButtonStart += OnClickButtonStart;
            View.OnClickButtonSettings += OnClickButtonChangeTab;
            View.OnClickButtonAbout += OnClickButtonChangeTab;
            View.OnClickButtonAboutProject += OnClickButtonChangeTab;
            View.OnClickButtonAboutUs += OnClickButtonChangeTab;
            View.OnClickButtonAboutProjectOk += OnClickButtonChangeTab;
            View.OnClickButtonAboutUsOk += OnClickButtonChangeTab;
            View.OnClickButtonAbout += OnClickButtonChangeTab;
        }

        #region Buttons

        private void OnClickButtonChangeTab(SettingsMenuType aboutMenuType)
        {
            OpenTab(aboutMenuType);
        }

        private void OnClickButtonStart()
        {
            CloseMenu(() =>
            {
                StartGameEvent?.Invoke();
            });
        }

        private void OnClickButtonOk()
        {
            CloseMenu(() =>
            {
                CallOtherMenu(Model.CurrentTab == SettingsMenuType.SmallSettings
                    ? MenuType.PauseMenu
                    : MenuType.MainMenu);
            });
        }

        private void OnClickButtonLanguage(string localeCode)
        {
            _localizationSettings.SetSelectedLocale(_localizationSettings.GetAvailableLocales().GetLocale(localeCode));
        }

        #endregion
    }
}