using System;
using Core.Infrastructure.Enums;
using UI.Base.DefaultMenu;

namespace UI.HUD.BacklogMenu
{
    public class BacklogMenuPresenter : MenuPresenter<BacklogMenuModel, BacklogMenuView>
    {
        public BacklogMenuPresenter(BacklogMenuModel model, BacklogMenuView view)
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
            View.OnClickButtonExit += OnClickButtonExit;
        }
        
        public void AddSentence(CharacterName character, string text)
        { 
            View.AddSentence(character, text);
        }
        public override void Clear()
        {
            base.Clear();
            
            View.Clear();
        }

        #region Buttons

        public void OnClickButtonExit()
        {
            CloseMenu();
        }

        #endregion
    }
}