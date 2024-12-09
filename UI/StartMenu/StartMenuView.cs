using System;
using Core.Infrastructure.Services;
using DG.Tweening;
using Scriptables.Settings;
using UI.Base.DefaultMenu;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.StartMenu
{
    public class StartMenuView : MenuView
    {
        [Header("Main")]
        [SerializeField] private RectTransform header;
        [SerializeField] private RectTransform bottom;
        
        [Header("Buttons")]
        [SerializeField] private Button buttonStart;
        [SerializeField] private Button buttonSettings;
        [SerializeField] private Button buttonAbout;

        #region ButtonEvents

        public event Action OnClickButtonStart;
        public event Action OnClickButtonSettings;
        public event Action OnClickButtonAbout;

        #endregion
        
        private ScriptableUiSettings _uiSettings;
        private SoundService _soundService;

        [Inject]
        private void Construct(StartMenuPresenter presenter, ScriptableUiSettings uiSettings, SoundService soundService)
        {
            _soundService = soundService;
            _uiSettings = uiSettings;
            
            InitButtons();
        }
        protected sealed override void InitButtons()
        {
            buttonStart.onClick.AddListener(() =>
            {
                _soundService.PlayButtonSound();
                
                OnClickButtonStart?.Invoke();
            });
            buttonSettings.onClick.AddListener(() =>
            {
                _soundService.PlayButtonSound();
                
                OnClickButtonSettings?.Invoke();
            });
            buttonAbout.onClick.AddListener(() =>
            {
                _soundService.PlayButtonSound();
                
                OnClickButtonAbout?.Invoke();
            });
        }
        public void SetButtonsInteractable(bool interactable)
        {
            buttonStart.interactable = interactable;
            buttonSettings.interactable = interactable;
            buttonAbout.interactable = interactable;
        }
        
        protected override void OpenMenuInternal(Action callback = null)
        {
            _soundService.PlayMenuSound(MenuSound.BigTreeIntro);
            
            windowTransform.gameObject.SetActive(true);
            bottom.anchoredPosition = new Vector2(-bottom.rect.width, 0);
            header.anchoredPosition = new Vector2(0, header.rect.height);
            
            Sequence = DOTween.Sequence();
            Sequence.Join(bottom.DOAnchorPos(
                Vector2.zero,
                _uiSettings.uiAnimationDuration));
            Sequence.Join(header.DOAnchorPos(
                Vector2.zero,
                _uiSettings.uiAnimationDuration));
            Sequence.OnComplete(() =>
            {
                callback?.Invoke();
            });
            Sequence.Play();
        }
        protected override void CloseMenuInternal(Action callback = null)
        {
            _soundService.PlayMenuSound(MenuSound.BigTreeOutro);
            
            Sequence = DOTween.Sequence();
            Sequence.Join(bottom.DOAnchorPos(
                new Vector3(-bottom.rect.width, 0, 0),
                _uiSettings.uiAnimationDuration));
            Sequence.Join(header.DOAnchorPos(
                new Vector3(0, header.rect.height, 0),
                _uiSettings.uiAnimationDuration));
            Sequence.onComplete = () =>
            {
                windowTransform.gameObject.SetActive(false);
                
                callback?.Invoke();
            };
            Sequence.Play();
        }

        public override void OpenMenuInstant()
        {
            windowTransform.gameObject.SetActive(true);
            header.localPosition = Vector3.zero;
            bottom.localPosition = Vector3.zero;
        }
        public override void CloseMenuInstant()
        {
            windowTransform.gameObject.SetActive(false);
            header.localPosition = new Vector3(0, header.rect.height, 0);
            bottom.localPosition = new Vector3(-bottom.rect.width, 0, 0);
        }
    }
}