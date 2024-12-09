using System;
using Core.Infrastructure.Enums;
using Core.Infrastructure.Pools;
using Core.Infrastructure.Services;
using DG.Tweening;
using Scriptables.Holders;
using Scriptables.Settings;
using UI.Base.DefaultMenu;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using Zenject;

namespace UI.HUD.BacklogMenu
{
    public class BacklogMenuView : MenuView
    {
        [Header("Buttons")]
        [SerializeField] private Button buttonExit;

        #region ButtonEvents

        public event Action OnClickButtonExit;

        #endregion

        private ScriptableLocalizationHolder _localizationHolder;
        private ScriptableUiSettings _uiSettings;
        private SoundService _soundService;
        private SentencePool _sentencePool;

        [Inject]
        private void Construct(BacklogMenuPresenter backlogMenuPresenter,
            SentencePool sentencePool,
            ScriptableUiSettings uiSettings,
            ScriptableLocalizationHolder localizationHolder,
            SoundService soundService)
        {
            _localizationHolder = localizationHolder;
            _sentencePool = sentencePool;
            _soundService = soundService;
            _uiSettings = uiSettings;

            InitButtons();
            
            _sentencePool.Initialize(100);
        }

        protected sealed override void InitButtons()
        {
            buttonExit.onClick.AddListener(() =>
            {
                _soundService.PlayButtonSound();
                
                OnClickButtonExit?.Invoke();
            });
        }
        public void SetButtonsInteractable(bool interactable)
        {
            buttonExit.interactable = interactable;
        }

        public void AddSentence(CharacterName character, string text)
        {
            _sentencePool.Spawn().SetText(new LocalizedString(_localizationHolder.uiLocalizationName, character.ToString()).GetLocalizedString(), text);
        }
        public void Clear()
        {
            _sentencePool.Clear();
        }
        
        protected override void OpenMenuInternal(Action onComplete = null)
        {
            _soundService.PlayMenuSound(MenuSound.SmallTreeIntro);
            
            windowTransform.gameObject.SetActive(true);
            windowTransform.anchoredPosition = new Vector2(windowTransform.rect.width, 0);
            background.color = new Color(0, 0, 0, 0);
            
            Sequence = DOTween.Sequence();
            Sequence.Join(windowTransform.DOAnchorPos(
                Vector2.zero,
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

            windowTransform.anchoredPosition = Vector2.zero;
            background.color = _uiSettings.backgroundFadeColor;
            
            Sequence = DOTween.Sequence();
            Sequence.Join(windowTransform.DOAnchorPos(
                new Vector2(windowTransform.rect.width, 0),
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
            windowTransform.anchoredPosition = Vector2.zero;
            windowTransform.gameObject.SetActive(true);
            background.color = _uiSettings.backgroundFadeColor;
        }
        public override void CloseMenuInstant()
        {
            windowTransform.anchoredPosition = new Vector2(windowTransform.rect.width, 0);
            windowTransform.gameObject.SetActive(false);
            background.color = new Color(0, 0, 0, 0);
        }
    }
}