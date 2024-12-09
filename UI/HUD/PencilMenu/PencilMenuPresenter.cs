using System;
using Core.Infrastructure.Enums.GameVariables;
using Core.Infrastructure.Utils;
using UI.Base.BaseMenu;
using UI.Base.DefaultMenu;
using UnityEngine;
using Zenject;

namespace UI.HUD.PencilMenu
{
    public class PencilMenuPresenter : MenuPresenter<PencilMenuModel, PencilMenuView>
    {
        [Inject]
        public PencilMenuPresenter(PencilMenuModel model, PencilMenuView view)
            : base(model, view)
        {
            InitSubscriptions();
            InitButtons();
        }
        
        protected sealed override void InitSubscriptions()
        {
            AddSubscriptionWithDistinct(model => model.MenuAnimationState, (value) =>
            {
                View.ChangePencilButtons(value == MenuAnimationState.Opened);
            });
            AddSubscriptionWithDistinct(model => model.IsBusy, (value) =>
            {
                View.SetButtonsInteractable(!value);
            });
            AddSubscriptionWithDistinct(model => model.IsShaking, (value) =>
            {
                View.ChangeShakeAnimation(value);
            });
            AddDictionarySubscriptionWithDistinct(model => model.Pencils, (key, oldValue, newValue) =>
            {
                View.ChangeItem(key, newValue.IsActive);
            });
        }
        protected sealed override void InitButtons()
        {
            View.OnClickButtonClose += OnClickButtonClose;
            View.OnClickButtonPencilItem += OnClickButtonPencilItem;
        }

        public void SetActivated(bool state)
        {
            View.SetActivated(state);
        }
        
        public void ChangePencilColor(Color color)
        {
            View.ChangePencilColor(color);
        }
        public void ChangeItem(ItemVariable item, bool state)
        {
            Model.Pencils[item] = new ItemState(state, null);
            Model.Update();
        }
        public void FocusAndAnimateItem(ItemVariable item, bool claim, Action onComplete)
        {
            if (!Model.Pencils[item].IsActive)
            {
                this.LogError("Item you want to focus is not active");
                
                onComplete?.Invoke();
                
                return;
            }
            
            View.FocusItem(item);
            
            Model.Pencils[item] = new ItemState(true,
                () => View.FocusItemClick(item, claim,
                () => CloseMenu(
                    () => onComplete?.Invoke())));
            Model.ShouldShake = true;
            Model.Update();
        }
        public void FocusItem(ItemVariable item, Action onComplete)
        {
            if (!Model.Pencils[item].IsActive)
            {
                this.LogError("Item you want to focus is not active");
                
                onComplete?.Invoke();
                
                return;
            }
            
            View.FocusItem(item);
            
            Model.Pencils[item] = new ItemState(true,
                    () => CloseMenu(
                        () => onComplete?.Invoke()));
            Model.ShouldShake = true;
            Model.Update();
        }
        
        public override void Clear()
        {
            base.Clear();
            
            View.Clear();
        }

        #region Buttons

        private void OnClickButtonPencilItem(ItemVariable itemVariable)
        {
            Model.Pencils[itemVariable].Callback?.Invoke();
            Model.Pencils[itemVariable] = new ItemState(Model.Pencils[itemVariable].IsActive, null);
            Model.ShouldShake = false;
            Model.Update();
        }
        private void OnClickButtonClose()
        {
            CloseMenu();
        }

        #endregion
    }
}