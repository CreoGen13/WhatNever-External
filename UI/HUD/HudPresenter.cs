using System;
using Core.Infrastructure.Enums;
using Core.Infrastructure.Enums.GameVariables;
using Core.Node.Panel;
using UI.Base.DefaultMenu;
using UI.Enums;
using UI.HUD.BacklogMenu;
using UI.HUD.CharactersMenu;
using UI.HUD.PencilMenu;
using UI.SettingsMenu;
using UnityEngine;
using CharacterName = Core.Infrastructure.Enums.CharacterName;

namespace UI.HUD
{
    public class HudPresenter : MenuPresenter<HudModel, HudView>
    {
        public event Action<bool> PauseGameEvent;
        
        private readonly CharactersMenuPresenter _charactersMenu;
        private readonly PencilMenuPresenter _pencilMenu;
        private readonly BacklogMenuPresenter _backlogMenu;
        private readonly SettingsMenuPresenter _settingsMenu;

        public HudPresenter(HudModel model, HudView view,
            CharactersMenuPresenter charactersMenu,
            PencilMenuPresenter pencilMenu,
            BacklogMenuPresenter backlogMenu,
            SettingsMenuPresenter settingsMenu)
            : base(model, view)
        {
            _settingsMenu = settingsMenu;
            _backlogMenu = backlogMenu;
            _pencilMenu = pencilMenu;
            _charactersMenu = charactersMenu;
            
            InitActions();
            InitSubscriptions();
            InitButtons();
        }

        protected sealed override void InitActions()
        {
            _settingsMenu.MenuClosedEvent += OnMenuClosed;
            _backlogMenu.MenuClosedEvent += OnMenuClosed;
            _pencilMenu.MenuClosedEvent += OnMenuClosed;
            _charactersMenu.MenuClosedEvent += OnMenuClosed;
        }
        private void OnMenuClosed()
        {
            Model.IsBlocked = false;
            Model.Update();
        }
        
        protected sealed override void InitSubscriptions()
        {
            AddSubscriptionWithDistinct(model => model.IsBusy, (value) =>
            {
                View.SetButtonsInteractable(!value);
            });
            AddSubscriptionWithDistinct(model => model.IsPaused, (value) =>
            {
                PauseGameEvent?.Invoke(value);
            });
            AddSubscriptionWithDistinct(model => model.HasCharactersList, (value) =>
            {
                if (!value)
                {
                    _charactersMenu.CloseMenuInstant();
                }
                
                View.ChangeCharactersList(value);
            });
            AddSubscriptionWithDistinct(model => model.HasItemsList, (value) =>
            {
                if (!value)
                {
                    _pencilMenu.CloseMenuInstant();
                }
                
                _pencilMenu.SetActivated(value);
                View.ChangeItemsList(value);
            });
        }
        protected sealed override void InitButtons()
        {
            View.OnClickButtonBacklog += OnClickButtonBacklog;
            View.OnClickButtonPencil += OnClickButtonPencil;
            View.OnClickButtonCharactersMenu += OnClickButtonCharactersMenu;
            View.OnClickButtonPauseMenu += OnClickButtonPauseMenu;
        }
        public override void Clear()
        {
            base.Clear();
            
            View.Clear();
            
            _charactersMenu.Clear();
            _pencilMenu.Clear();
            _backlogMenu.Clear();
        }

        #region INTERACTIONS
        
        public void SetButtonChoice(bool isLeftButton, string text, Action onComplete)
        {
            View.SetButtonChoice(isLeftButton, text, onComplete);
        }
        public void MoveButtonsChoice(bool toOpen, Action onComplete)
        {
            Model.IsBlocked = true;
            Model.Update();
            
            View.MoveButtonsChoice(toOpen, () =>
            {
                Model.IsBlocked = false;
                Model.Update();
                
                onComplete?.Invoke();
            });
        }
        public void SetDialogScreen(LoadedNodeData loadedNodeData, bool waitForAnimation, Action onComplete)
        {
            var position = loadedNodeData.ArrivalType == ArrivalType.Moving
                ? loadedNodeData.MovePosition
                : loadedNodeData.SpritePosition;

            var endPosition = loadedNodeData.SpritePosition;
            
            View.SetDialogScreen(loadedNodeData.ChoiceButtonType == ChoiceButtonType.Left,
                loadedNodeData.DialogScreenActionType,
                loadedNodeData.ArrivalType,
                loadedNodeData.Sprite,
                position,
                endPosition,
                new Vector3(loadedNodeData.ScaleAndRotation.x, loadedNodeData.ScaleAndRotation.y, 1),
                waitForAnimation,
                onComplete);
        }

        public void ChangeItemsList(bool state)
        {
            Model.HasItemsList = state;
            Model.Update();
        }
        public void ChangePencilColor(Color color)
        {
            _pencilMenu.ChangePencilColor(color);
        }
        public void ChangeItem(ItemVariable item, bool state)
        {
            _pencilMenu.ChangeItem(item, state);
        }
        public void FocusAndAnimateItem(ItemVariable item, bool claim, Action onComplete)
        {
            _pencilMenu.FocusAndAnimateItem(item, claim, onComplete);
        }
        public void FocusItem(ItemVariable item, Action onComplete)
        {
            _pencilMenu.FocusItem(item, onComplete);
        }

        public void ChangeCharactersList(bool state)
        {
            Model.HasCharactersList = state;
            Model.Update();
        }
        public void ChangeCharacter(CharacterName character, int feature, Action onComplete)
        {
            _charactersMenu.ActivateCharacter(character);
            View.ChangeCharactersShake(true);
            
            if (feature > -1)
            {
                _charactersMenu.ActivateCharacterFeature(character, feature);
            }

            _charactersMenu.AddTemporaryCallback(() =>
            {
                View.ChangeCharactersShake(false);
                
                onComplete?.Invoke();
            });
        }
        
        public void AddSentence(CharacterName author, string text)
        {
            _backlogMenu.AddSentence(author, text);
        }
        public void ClearSentences()
        {
            _backlogMenu.Clear();
        }
        
        #endregion
        #region Buttons

        private void OnClickButtonPauseMenu()
        {
            CloseMenu(() =>
            {
                CallOtherMenu(MenuType.PauseMenu);
            });
        }

        private void OnClickButtonCharactersMenu()
        {
            Model.IsBlocked = true;
            Model.Update();
            
            _charactersMenu.OpenMenu();
        }

        private void OnClickButtonPencil()
        {
            Model.IsBlocked = true;
            Model.Update();
            
            _pencilMenu.OpenMenu();
        }

        private void OnClickButtonBacklog()
        {
            Model.IsBlocked = true;
            Model.Update();
            
            _backlogMenu.OpenMenu();
        }

        #endregion
    }
}
