using System;
using Core.Base.Classes;
using DG.Tweening;
using Scriptables.Settings;
using UnityEngine;
using Zenject;

namespace UI.MainMenu
{
    public class MainMenuView : BaseView
    {
        [Header("Main")]
        [SerializeField] private RectTransform windowTransform;
        [SerializeField] private GameObject mainMenu;
        
        private ScriptableUiSettings _uiSettings;
        
        [Inject]
        private void Construct(ScriptableUiSettings uiSettings)
        {
            _uiSettings = uiSettings;
        }
        
        public void OpenMenu(Action callback = null)
        {
            windowTransform.gameObject.SetActive(true);
            windowTransform.anchoredPosition = new Vector2(0, windowTransform.rect.height);
            
            Sequence = DOTween.Sequence();
            Sequence.Join(windowTransform.DOAnchorPos(
                Vector2.zero,
                _uiSettings.uiAnimationDuration));
            Sequence.OnComplete(() =>
            {
                callback?.Invoke();
            });
        }
        public void CloseMenu(Action callback = null)
        {
            Sequence = DOTween.Sequence();
            Sequence.Join(windowTransform.DOAnchorPos(
                new Vector2(0, windowTransform.rect.height),
                _uiSettings.uiAnimationDuration));
            Sequence.OnComplete(() =>
            {
                windowTransform.gameObject.SetActive(false);
                
                callback?.Invoke();
            });
        }
    }
}