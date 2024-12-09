using UI.HUD;
using UI.HUD.BacklogMenu;
using UI.HUD.CharactersMenu;
using UI.HUD.PencilMenu;
using UI.MainMenu;
using UI.PauseMenu;
using UI.SettingsMenu;
using UI.StartMenu;
using UnityEngine;
using Zenject;

namespace ZenjectInstallers
{
    public class UIContextInstaller : MonoInstaller
    {
        [Header("Main Menu")]
        [SerializeField] private MainMenuView mainMenuView;
        [SerializeField] private StartMenuView startMenuView;
        [SerializeField] private SettingsMenuView settingsMenuView;
        
        [Header("HUD")]
        [SerializeField] private HudView hudView;
        [SerializeField] private PencilMenuView pencilMenuView;
        [SerializeField] private BacklogMenuView backlogMenuView;
        [SerializeField] private CharactersMenuView charactersMenuView;
        [SerializeField] private PauseMenuView pauseMenuView;
        
        public override void InstallBindings()
        {
            BindBacklogMenu();
            BindCharactersMenu();
            BindPencilMenu();
            BindHud();
            
            BindStartMenu();
            BindPauseMenu();
            BindSettingsMenu();
            BindMainMenu();
        }
        
        private void BindMainMenu()
        {
            Container
                .Bind<MainMenuPresenter>()
                .AsSingle()
                .WithArguments(new MainMenuModel(), mainMenuView);
        }
        private void BindStartMenu()
        {
            Container
                .Bind<StartMenuPresenter>()
                .AsSingle()
                .WithArguments(new StartMenuModel(), startMenuView);
        }
        private void BindSettingsMenu()
        {
            Container
                .Bind<SettingsMenuPresenter>()
                .AsSingle()
                .WithArguments(new SettingsMenuModel(), settingsMenuView);
        }
        private void BindHud()
        {
            Container
                .Bind<HudPresenter>()
                .AsSingle()
                .WithArguments(new HudModel(), hudView);
        }
        private void BindBacklogMenu()
        {
            Container
                .Bind<BacklogMenuPresenter>()
                .AsSingle()
                .WithArguments(new BacklogMenuModel(), backlogMenuView);
        }
        private void BindCharactersMenu()
        {
            Container
                .Bind<CharactersMenuPresenter>()
                .AsSingle()
                .WithArguments(new CharactersMenuModel(), charactersMenuView);
        }
        private void BindPauseMenu()
        {
            Container
                .Bind<PauseMenuPresenter>()
                .AsSingle()
                .WithArguments(new PauseMenuModel(), pauseMenuView);
        }
        private void BindPencilMenu()
        {
            Container
                .Bind<PencilMenuPresenter>()
                .AsSingle()
                .WithArguments(new PencilMenuModel(), pencilMenuView);
        }
    }
}