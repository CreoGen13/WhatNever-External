using System;
using Core.Infrastructure.Enums;
using Core.Infrastructure.Services;
using DG.Tweening;
using Scriptables.Settings;
using TMPro;
using UI.Base.DefaultMenu;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.HUD
{
    public class HudView : MenuView
    {
        [Header("Variables")]
        [SerializeField] private float dialogScreenOffset;
        [SerializeField] private float openCloseDelta;
        
        [Header("Fitters")]
        [SerializeField] private RectTransform hudCanvasRectTransform;
        [SerializeField] private RectTransform left;
        [SerializeField] private RectTransform right;

        [Header("Buttons")]
        [SerializeField] private RectTransform buttonsParent;
        [SerializeField] private Button buttonCharacters;
        [SerializeField] private Button buttonPencil;
        [SerializeField] private Button buttonPauseMenu;
        [SerializeField] private Button buttonBacklog;

        [Header("Choice")]
        [SerializeField] private RectTransform bottomTab;
        [SerializeField] private Button buttonLeftChoice;
        [SerializeField] private Button buttonRightChoice;
        [SerializeField] private TextMeshProUGUI buttonLeftChoiceText;
        [SerializeField] private TextMeshProUGUI buttonRightChoiceText;
        
        [Header("DialogScreen")]
        [SerializeField] private Image dialogScreenLeft;
        [SerializeField] private Image dialogScreenRight;
        [SerializeField] private RectTransform dialogScreenLeftRectTransform;
        [SerializeField] private RectTransform dialogScreenRightRectTransform;

        #region ButtonEvents

        public event Action OnClickButtonPauseMenu;
        public event Action OnClickButtonCharactersMenu;
        public event Action OnClickButtonPencil;
        public event Action OnClickButtonBacklog;

        #endregion
        
        private Sequence _charactersShakeSequence;
        private ScriptableUiSettings _uiSettings;
        private SoundService _soundService;
        private ScriptableGameSettings _gameSettings;

        [Inject]
        private void Construct(HudPresenter presenter,
            ScriptableGameSettings gameSettings,
            ScriptableUiSettings uiSettings,
            SoundService soundService)
        {
            _gameSettings = gameSettings;
            _soundService = soundService;
            _uiSettings = uiSettings;

            _charactersShakeSequence = DOTween.Sequence();
            _charactersShakeSequence.SetLoops(-1);
            _charactersShakeSequence.Join(buttonCharacters.transform.DOShakeRotation(
                _uiSettings.shakeDuration,
                _uiSettings.shakeStrength,
                _uiSettings.shakeVibrato,
                _uiSettings.shakeRandomness,
                false));
            _charactersShakeSequence.Join(buttonCharacters.transform.DOShakePosition(
                _uiSettings.shakeDuration,
                _uiSettings.shakeStrength,
                _uiSettings.shakeVibrato,
                _uiSettings.shakeRandomness,
                false,
                false));
            _charactersShakeSequence.Pause();
            
            InitButtons();
        }
        protected sealed override void InitButtons()
        {
            buttonPauseMenu.onClick.AddListener(() =>
            {
                _soundService.PlayButtonSound();
                
                OnClickButtonPauseMenu?.Invoke();
            });
            buttonCharacters.onClick.AddListener(() =>
            {
                _soundService.PlayButtonSound();
                
                OnClickButtonCharactersMenu?.Invoke();
            });
            buttonPencil.onClick.AddListener(() =>
            {
                _soundService.PlayButtonSound();
                
                OnClickButtonPencil?.Invoke();
            });
            buttonBacklog.onClick.AddListener(() =>
            {
                _soundService.PlayButtonSound();
                
                OnClickButtonBacklog?.Invoke();
            });
        }

        public void SetButtonsInteractable(bool interactable)
        {
            buttonCharacters.interactable = interactable;
            buttonPauseMenu.interactable = interactable;
            buttonBacklog.interactable = interactable;
            buttonPencil.interactable = interactable;
            buttonPencil.targetGraphic.raycastTarget = interactable;
            buttonRightChoice.interactable = interactable;
            buttonLeftChoice.interactable = interactable;
        }
        public void ChangeCharactersList(bool state)
        {
            buttonCharacters.gameObject.SetActive(state);
        }
        public void ChangeItemsList(bool state)
        {
            buttonPencil.gameObject.SetActive(state);
        }
        
        public void ChangeCharactersShake(bool active)
        {
            if(active)
            {
                _charactersShakeSequence.Play();
            }
            else
            {
                _charactersShakeSequence.Pause();
            }
        }
        public void SetDialogScreen(bool isLeft, DialogScreenActionType screenActionType, ArrivalType arrivalType, Sprite sprite, Vector3 position, Vector3 endPosition, Vector3 scale, bool waitForAnimation, Action onComplete)
        {
            var currentScreenImage = isLeft ? dialogScreenLeft : dialogScreenRight;
            var currentScreenTransform = isLeft ? dialogScreenLeftRectTransform : dialogScreenRightRectTransform;
            
            Sequence = DOTween.Sequence();
            
            switch (screenActionType)
            {
                case DialogScreenActionType.Change:
                {
                    if (arrivalType == ArrivalType.Moving)
                    {
                        currentScreenImage.sprite = sprite;
                        currentScreenImage.rectTransform.localPosition = position;
                        
                        Sequence.Join(currentScreenImage.rectTransform.DOLocalMove(
                            endPosition,
                            _gameSettings.dialogScreenArrivalMoveDuration));
                        Sequence.Join(currentScreenImage.rectTransform.DOScale(
                            new Vector3(scale.x, scale.y, 1),
                            _gameSettings.dialogScreenArrivalMoveDuration));
                        Sequence.Play();
                    }
                    else
                    {
                        currentScreenImage.sprite = sprite;
                        currentScreenImage.rectTransform.localPosition = position;
                    }
                    
                    currentScreenImage.SetNativeSize();
                    
                    break;
                }
                case DialogScreenActionType.Appear:
                {
                    currentScreenImage.sprite = sprite;
                    currentScreenImage.rectTransform.localPosition = position;
                    currentScreenImage.SetNativeSize();
                    
                    currentScreenTransform.gameObject.SetActive(true);
                    endPosition = Vector3.zero;
                    Sequence.Join(currentScreenTransform.DOLocalMove(
                        endPosition,
                        _uiSettings.uiAnimationDuration));
                    Sequence.Play();
                    
                    break;
                }
                case DialogScreenActionType.Disappear:
                {
                    endPosition = new Vector3(hudCanvasRectTransform.rect.width + dialogScreenOffset, 0, 0);
                    Sequence.Join(currentScreenTransform.DOLocalMove(
                        isLeft ? -endPosition : endPosition,
                        _uiSettings.uiAnimationDuration));
                    Sequence.onComplete += () =>
                    {
                        currentScreenTransform.gameObject.SetActive(false);
                    };
                    Sequence.Play();
                    
                    break;
                }
            }

            if (waitForAnimation)
            {
                Sequence.onComplete += () =>
                {
                    onComplete?.Invoke();
                };
            }
            else
            {
                onComplete?.Invoke();
            }
            
            Sequence.Play();
        }
        protected internal void SetButtonChoice(bool isLeftButton, string text, Action onComplete)
        {
            var buttonText = isLeftButton ? buttonLeftChoiceText : buttonRightChoiceText;
            var button = isLeftButton ? buttonLeftChoice : buttonRightChoice;
            
            buttonText.text = text;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => { onComplete(); });
        }
        public void MoveButtonsChoice(bool toOpen, Action callback)
        {
            var endPosition = toOpen ? new Vector2(0, bottomTab.rect.height) : Vector2.zero;
            bottomTab.anchoredPosition = Vector2.zero;
            bottomTab.DOAnchorPos(
                endPosition,
                _gameSettings.choiceMoveDuration).OnComplete(() =>
            {
                callback?.Invoke();
            });
        }
        
        private void ClearDialogScreen()
        {
            var screenWidth = hudCanvasRectTransform.rect.width;
            dialogScreenLeftRectTransform.localPosition = new Vector2(
                -screenWidth / 2 - dialogScreenOffset,
                dialogScreenLeft.transform.localPosition.y);
            dialogScreenRightRectTransform.localPosition = new Vector2(
                screenWidth / 2 + dialogScreenOffset,
                dialogScreenRight.transform.localPosition.y);

            dialogScreenLeftRectTransform.gameObject.SetActive(false);
            dialogScreenRightRectTransform.gameObject.SetActive(false);
        }
        private void ClearButtonsChoice()
        {
            bottomTab.anchoredPosition = Vector2.zero;
            buttonLeftChoice.onClick.RemoveAllListeners();
            buttonRightChoice.onClick.RemoveAllListeners();
        }

        public void Clear()
        {
            ClearDialogScreen();
            ClearButtonsChoice();
            
            ChangeCharactersShake(false);
        }
        
        protected override void OpenMenuInternal(Action onComplete = null)
        {
            Sequence = DOTween.Sequence();
            Sequence.Join(buttonsParent.DOSizeDelta(
                Vector2.zero,
                _uiSettings.uiAnimationDuration));
            Sequence.AppendCallback(() =>
            {
                onComplete?.Invoke();
            });
            Sequence.Play();
        }
        protected override void CloseMenuInternal(Action onComplete = null)
        {
            Sequence = DOTween.Sequence();
            Sequence.Join(buttonsParent.DOSizeDelta(
                new Vector2(openCloseDelta, 0),
                _uiSettings.uiAnimationDuration));
            Sequence.AppendCallback(() =>
            {
                onComplete?.Invoke();
            });
            Sequence.Play();
        }
        public override void OpenMenuInstant()
        {
            buttonsParent.sizeDelta = Vector2.zero;
        }
        public override void CloseMenuInstant()
        {
            buttonsParent.sizeDelta = new Vector2(openCloseDelta, 0);
        }
    }
}