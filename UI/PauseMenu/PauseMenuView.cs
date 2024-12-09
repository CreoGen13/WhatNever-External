using System;
using Core.Infrastructure.Services;
using DG.Tweening;
using Scriptables.Settings;
using UI.Base.DefaultMenu;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.PauseMenu
{
    public class PauseMenuView : MenuView
    {
        [Header("Buttons")]
        [SerializeField] private Button buttonContinue;
        [SerializeField] private Button buttonSettings;
        [SerializeField] private Button buttonMainMenu;

        #region ButtonEvents

        public event Action OnClickButtonContinue;
        public event Action OnClickButtonSettings;
        public event Action OnClickButtonMainMenu;

        #endregion
        
        private ScriptableUiSettings _uiSettings;
        private SoundService _soundService;

        [Inject]
        private void Construct(PauseMenuPresenter pauseMenuPresenter, ScriptableUiSettings uiSettings, SoundService soundService)
        {
            _soundService = soundService;
            _uiSettings = uiSettings;

            InitButtons();
        }

        protected sealed override void InitButtons()
        {
            buttonContinue.onClick.AddListener(() =>
            {
                _soundService.PlayButtonSound();
                
                OnClickButtonContinue?.Invoke();
            });
            buttonSettings.onClick.AddListener(() =>
            {
                _soundService.PlayButtonSound();
                
                OnClickButtonSettings?.Invoke();

            });
            buttonMainMenu.onClick.AddListener(() =>
            {
                _soundService.PlayButtonSound();
                
                OnClickButtonMainMenu?.Invoke();
            });
        }
        public void SetButtonsInteractable(bool interactable)
        {
            buttonContinue.interactable = interactable;
            buttonSettings.interactable = interactable;
            buttonMainMenu.interactable = interactable;
        }
        
        protected override void OpenMenuInternal(Action onComplete = null)
        {
            _soundService.PlayMenuSound(MenuSound.SmallTreeIntro);
            
            windowTransform.gameObject.SetActive(true);
            windowTransform.localPosition = new Vector3(0, Screen.height, 0);
            background.color = new Color(0, 0, 0, 0);
            
            Sequence = DOTween.Sequence();
            Sequence.Join(windowTransform.DOLocalMove(
                Vector3.zero,
                _uiSettings.uiAnimationDuration));
            Sequence.Join(background.DOColor(
                _uiSettings.backgroundFadeColor, 
                _uiSettings.uiAnimationDuration));
            Sequence.OnComplete(() =>
            {
                onComplete?.Invoke();
            });
            Sequence.Play();
        }
        protected override void CloseMenuInternal(Action onComplete = null)
        {
            _soundService.PlayMenuSound(MenuSound.SmallTreeOutro);
            
            Sequence = DOTween.Sequence();
            Sequence.Join(windowTransform.DOLocalMove(
                new Vector3(0, Screen.height, 0),
                _uiSettings.uiAnimationDuration));
            Sequence.Join(background.DOColor(
                new Color(0, 0, 0, 0),
                _uiSettings.uiAnimationDuration));
            Sequence.OnComplete(() =>
            {
                windowTransform.gameObject.SetActive(false);
                onComplete?.Invoke();
            });
            Sequence.Play();
        }
        
        public override void OpenMenuInstant()
        {
            windowTransform.gameObject.SetActive(true);
            windowTransform.localPosition = Vector3.zero;
            background.color = _uiSettings.backgroundFadeColor;
        }
        public override void CloseMenuInstant()
        {
            windowTransform.gameObject.SetActive(false);
            windowTransform.localPosition = new Vector3(0, Screen.height, 0);
            background.color = new Color(0, 0, 0, 0);
        }
    }
}