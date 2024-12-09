using System;
using Core.Base.Classes;
using Core.Infrastructure.Enums;
using Core.Infrastructure.Enums.GameVariables;
using Core.Infrastructure.Utils;
using Core.Node.Panel;
using UI.Enums;
using UI.HUD;
using UI.PauseMenu;
using UI.SettingsMenu;
using UI.StartMenu;
using UnityEngine;
using Zenject;

namespace UI.MainMenu
{
     public class MainMenuPresenter : BasePresenter<MainMenuModel, MainMenuView>
     {
         public event Action<Action> StartGameEvent;
         public event Action<bool> PauseGameEvent;
         public event Action EndGameEvent;
         
         private readonly SettingsMenuPresenter _settingsMenuPresenter;
         private readonly StartMenuPresenter _startMenuPresenter;
         private readonly HudPresenter _hudPresenter;
         private readonly PauseMenuPresenter _pauseMenuPresenter;

         #region INIT
         
         [Inject]
         public MainMenuPresenter(
             MainMenuModel model,
             MainMenuView view,
             SettingsMenuPresenter settingsMenuPresenter,
             StartMenuPresenter startMenuPresenter,
             HudPresenter hudPresenter,
             PauseMenuPresenter pauseMenuPresenter)
             : base(model, view)
         {
             _settingsMenuPresenter = settingsMenuPresenter;
             _startMenuPresenter = startMenuPresenter;
             _hudPresenter = hudPresenter;
             _pauseMenuPresenter = pauseMenuPresenter;

             InitSubscriptions();
             InitActions();
             
             this.Log("UI initialized");
         }
         protected sealed override void InitSubscriptions() { }
         protected sealed override void InitActions()
         {
             _startMenuPresenter.OtherMenuCallEvent += (menuType) =>
             {
                 switch (menuType)
                 {
                     case MenuType.SettingsMenu:
                     {
                        OpenSettingsMenu(SettingsMenuType.Settings);
                        
                         break;
                     }
                     case MenuType.AboutMenu:
                     {
                         OpenSettingsMenu(SettingsMenuType.About);
                        
                         break;
                     }
                 }
             };
             _settingsMenuPresenter.OtherMenuCallEvent += (menuType) =>
             {
                 switch (menuType)
                 {
                     case MenuType.MainMenu:
                     {
                         OpenStartMenu();
                         
                         break;
                     }
                     case MenuType.PauseMenu:
                     {
                         OpenPauseMenu();
                         
                         break;
                     }
                 }
             };
             _hudPresenter.OtherMenuCallEvent += (menuType) =>
             {
                 switch (menuType)
                 {
                     case MenuType.PauseMenu:
                     {
                         OpenPauseMenu();
                         
                         break;
                     }
                 }
             };
             _pauseMenuPresenter.OtherMenuCallEvent += (menuType) =>
             {
                 switch (menuType)
                 {
                     case MenuType.MainMenu:
                     {
                         OpenStartMenu();
                        
                         break;
                     }
                     case MenuType.SettingsMenu:
                     {
                         OpenSettingsMenu(SettingsMenuType.SmallSettings);
                        
                         break;
                     }
                     case MenuType.Hud:
                     {
                         OpenHudMenu();
                        
                         break;
                     }
                 }
             };
             
             _startMenuPresenter.StartGameEvent += () =>
             {
                 StartGameEvent?.Invoke(() => OpenHudMenu());
             };
             _settingsMenuPresenter.StartGameEvent += () =>
             {
                 StartGameEvent?.Invoke(() => OpenHudMenu());
             };
             _hudPresenter.PauseGameEvent += value =>
             {
                 PauseGameEvent?.Invoke(value);
             };
             _pauseMenuPresenter.EndGameEvent += () =>
             {
                 OpenStartMenu(EndGameEvent);
             };
         }
         
         public void Start()
         {
             OpenStartMenu();
             
             this.Log("UI started");
         }
         
         public void Clear()
         {
             _hudPresenter.Clear();
             _pauseMenuPresenter.Clear();
         }
         
         #endregion
         #region OPEN CLOSE LOGIC
         
         private void OpenMenu(Action onComplete = null)
         {
             View.OpenMenu(() =>
             {
                 Model.IsOpened = true;
                 Model.Update();
                     
                 onComplete?.Invoke();
             });
         }
         private void CloseMenu(Action onComplete = null)
         {
             View.CloseMenu(() =>
             {
                 Model.IsOpened = false;
                 Model.Update();
                     
                 onComplete?.Invoke();
             });
         }

         private void OpenHudMenu(Action onComplete = null)
         {
             Model.CurrentMenu = MainMenuType.HUD;
             Model.Update();
             
             if (Model.IsOpened)
             {
                 CloseMenu(() =>
                 {
                     _hudPresenter.OpenMenu(onComplete);
                 });
             }
             else
             {
                 _hudPresenter.OpenMenu(onComplete);
             }
         }
         private void OpenPauseMenu(Action onComplete = null)
         {
             Model.CurrentMenu = MainMenuType.PauseMenu;
             Model.Update();
             
             if (Model.IsOpened)
             {
                 CloseMenu(() =>
                 {
                     _pauseMenuPresenter.OpenMenu(onComplete);
                 });
             }
             else
             {
                 _pauseMenuPresenter.OpenMenu(onComplete);
             }
         }
         private void OpenStartMenu(Action onComplete = null)
         {
             Model.CurrentMenu = MainMenuType.StartMenu;
             Model.Update();
             
             if (Model.IsOpened)
             {
                 _startMenuPresenter.OpenMenu(onComplete);
             }
             else
             {
                 OpenMenu(() =>
                 {
                     _startMenuPresenter.OpenMenu(onComplete);
                 });
             }
         }
         private void OpenSettingsMenu(SettingsMenuType settingsMenuType, Action onComplete = null)
         {
             Model.CurrentMenu = MainMenuType.SettingsMenu;
             Model.Update();

             if (Model.IsOpened)
             {
                 _settingsMenuPresenter.OpenMenuWithTab(settingsMenuType, onComplete);
             }
             else
             {
                 OpenMenu(() =>
                 {
                     _settingsMenuPresenter.OpenMenuWithTab(settingsMenuType, onComplete);
                 });
             }
         }
         
         #endregion
         #region INTERACTIONS

         public void ReturnToStartMenu()
         {
             _hudPresenter.CloseMenu();

             OpenStartMenu(EndGameEvent);
         }
         
         public void ChangeCharacter(CharacterName character, int feature, Action onComplete)
         {
             _hudPresenter.ChangeCharacter(character, feature, onComplete);
         }

         public void ChangePencilColor(Color color)
         {
             _hudPresenter.ChangePencilColor(color);
         }
         public void FocusAndAnimateItem(ItemVariable item, bool claim, Action onComplete)
         {
             _hudPresenter.FocusAndAnimateItem(item, claim, onComplete);
         }
         public void FocusItem(ItemVariable item, Action onComplete)
         {
             _hudPresenter.FocusItem(item, onComplete);
         }
         
         public void SetButtonChoice(bool isLeft, string text, Action onComplete)
         {
             _hudPresenter.SetButtonChoice(isLeft, text, onComplete);
         }
         public void MoveButtonsChoice(bool toOpen, Action onComplete)
         {
             _hudPresenter.MoveButtonsChoice(toOpen, onComplete);
         }

         public void SetDialogScreen(LoadedNodeData nodeData, bool waitForAnimation, Action onComplete)
         {
             _hudPresenter.SetDialogScreen(nodeData, waitForAnimation, onComplete);
         }
         
         public void AddSentence(CharacterName author, string text)
         {
             _hudPresenter.AddSentence(author, text);
         }
         public void ClearSentences()
         {
             _hudPresenter.ClearSentences();
         }

         #endregion
         #region SUBSCRIPTIONS

         public void OnItemVariableChanged(ItemVariable item, bool state)
         {
             _hudPresenter.ChangeItem(item, state);
         }
         public void OnGameVariableChanged(GameVariable variable, bool state)
         {
             switch (variable)
             {
                 case GameVariable.HasCharactersList:
                 {
                     _hudPresenter.ChangeCharactersList(state);
                     
                     break;
                 }
                 case GameVariable.HasItemsList:
                 {
                     _hudPresenter.ChangeItemsList(state);
                     
                     break;
                 }
             }
         }

         #endregion
     }
}