using System;
using Core.Infrastructure.Enums;
using UI.Base.DefaultMenu;
using Zenject;

namespace UI.HUD.CharactersMenu
{
    // TODO: refactor to implement game variables
    public class CharactersMenuPresenter : MenuPresenter<CharactersMenuModel, CharactersMenuView>
    {
        [Inject]
        public CharactersMenuPresenter(CharactersMenuModel model, CharactersMenuView view)
            : base(model, view)
        {
            InitSubscriptions();
            InitActions();
            InitButtons();
        }
        protected sealed override void InitSubscriptions()
        {
            AddSubscriptionWithDistinct(model => model.IsBusy, (value) =>
            {
                View.SetInteractableButtons(!value);
            });
            AddDictionarySubscriptionWithDistinct(model => model.Characters,
                (key, oldValue, newValue) =>
                {
                    if (newValue.IsActive != oldValue.IsActive)
                    {
                        if(newValue.IsActive)
                        {
                            View.ActivateCharacter(key);
                        }
                        else
                        {
                            View.DeactivateCharacter(key);
                        }
                    }
                    else
                    {
                        View.UpdateCharacterFeatures(key, newValue.Features);
                    }
                });
        }
        protected sealed override void InitActions()
        {
            View.MenuClosedEvent += ClearTemporaryActions;
        }
        protected sealed override void InitButtons()
        {
            View.ClickButtonOkEvent += OnClickButtonOk;
        }

        public void AddTemporaryCallback(Action onComplete)
        {
            Model.TemporaryActions.Add(onComplete);
            MenuClosedEvent += onComplete;
        }
        private void ClearTemporaryActions()
        {
            foreach (var temporaryAction in Model.TemporaryActions)
            {
                MenuClosedEvent -= temporaryAction;
            }
            Model.TemporaryActions.Clear();
        }
        
        public void ActivateCharacter(CharacterName characterName)
        {
            Model.Characters[characterName] = new CharacterState(true, new bool[6]);
            Model.Update();
        }
        public void ActivateCharacterFeature(CharacterName characterName, int featureNumber)
        {
            var characterState = new CharacterState(true, Model.Characters[characterName].Features)
            {
                Features =
                {
                    [featureNumber] = true
                }
            };

            Model.Characters[characterName] = characterState;
        }
        public override void Clear()
        {
            base.Clear();
            
            Model.ClearCharacters();
        }

        #region Buttons

        private void OnClickButtonOk()
        {
            if(Model.IsBusy)
            {
                return;
            }

            CloseMenu();
        }

        #endregion
    }
}