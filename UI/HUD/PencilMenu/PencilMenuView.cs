using System;
using Core.Infrastructure.Enums.GameVariables;
using Core.Infrastructure.Services;
using Core.Infrastructure.Utils;
using Core.Mono;
using DG.Tweening;
using Scriptables.Settings;
using UI.Base.DefaultMenu;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.HUD.PencilMenu
{
    public class PencilMenuView : MenuView
    {
        [Header("Variables")]
        [SerializeField] private float closedOffset;
        
        [Header("Pencil")]
        [SerializeField] private Button buttonClosePencil;
        [SerializeField] private Image pencilImage;

        [Header("Groups")]
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private RectTransform pencilGroup;
        
        [Header("Items")]
        [SerializeField] private PencilItemMono stick;
        [SerializeField] private PencilItemMono bluePencil;
        [SerializeField] private PencilItemMono greenPencil;
        [SerializeField] private PencilItemMono redPencil;
        [SerializeField] private PencilItemMono yellowPencil;
        [SerializeField] private ItemFocus itemFocus;

        #region ButtonEvents

        public event Action OnClickButtonClose;
        public event Action<ItemVariable> OnClickButtonPencilItem;

        #endregion
        
        private ScriptableUiSettings _uiSettings;
        private SoundService _soundService;
        
        private Sequence _shakingSequence;

        [Inject]
        public void Construct(PencilMenuPresenter pencilMenuPresenter, ScriptableUiSettings uiSettings, SoundService soundService)
        {
            _soundService = soundService;
            _uiSettings = uiSettings;

            _shakingSequence = DOTween.Sequence();
            _shakingSequence.SetLoops(-1);
            _shakingSequence.Join(windowTransform.DOShakeRotation(
                _uiSettings.shakeDuration,
                new Vector3(0, 0, 2f), _uiSettings.shakeVibrato,
                _uiSettings.shakeRandomness, false));
            _shakingSequence.Join(windowTransform.DOShakePosition(
                _uiSettings.shakeDuration,
                new Vector3(4f, 0, 0), _uiSettings.shakeVibrato,
                _uiSettings.shakeRandomness, false, false));
            _shakingSequence.Pause();

            InitButtons();
        }
        protected sealed override void InitButtons()
        {
            buttonClosePencil.onClick.AddListener(() =>
            {
                _soundService.PlayButtonSound();
                
                OnClickButtonClose?.Invoke();
            });
            
            stick.Button.onClick.AddListener(() =>
            {
                _soundService.PlayButtonSound();
                
                OnClickButtonPencilItem?.Invoke(ItemVariable.Stick);
            });
            bluePencil.Button.onClick.AddListener(() =>
            {
                _soundService.PlayButtonSound();

                OnClickButtonPencilItem?.Invoke(ItemVariable.PencilBlue);
            });
            greenPencil.Button.onClick.AddListener(() =>
            {
                _soundService.PlayButtonSound();
                
                OnClickButtonPencilItem?.Invoke(ItemVariable.PencilGreen);
            });
            redPencil.Button.onClick.AddListener(() =>
            {
                _soundService.PlayButtonSound();
                
                OnClickButtonPencilItem?.Invoke(ItemVariable.PencilRed);
            });
            yellowPencil.Button.onClick.AddListener(() =>
            {
                _soundService.PlayButtonSound();
                
                OnClickButtonPencilItem?.Invoke(ItemVariable.PencilYellow);
            });
        }

        protected override void OpenMenuInternal(Action onComplete = null)
        {
            Sequence = DOTween.Sequence();
            Sequence.Join(windowTransform.DOAnchorPos(
                new Vector2(windowTransform.rect.width, 0),
                _uiSettings.uiAnimationDuration));
            Sequence.Join(windowTransform.DOLocalRotate(
                Vector3.zero,
                _uiSettings.uiAnimationDuration));
            Sequence.OnComplete(() =>
            {
                onComplete?.Invoke();
            });
            Sequence.Play();
        }
        protected override void CloseMenuInternal(Action onComplete = null)
        {
            Sequence = DOTween.Sequence();
            Sequence.Join(windowTransform.DOAnchorPos(
                new Vector2(closedOffset, 0),
                _uiSettings.uiAnimationDuration));
            Sequence.Join(windowTransform.transform.DOLocalRotate(
                Vector3.zero,
                _uiSettings.uiAnimationDuration));
            Sequence.OnComplete(() =>
            {
                onComplete?.Invoke();
            });
            Sequence.Play();
        }

        public override void OpenMenuInstant()
        {
            windowTransform.anchoredPosition = new Vector2(windowTransform.rect.width, 0);
            windowTransform.localRotation = Quaternion.identity;
        }
        public override void CloseMenuInstant()
        {
            windowTransform.anchoredPosition = new Vector2(closedOffset, 0);
            windowTransform.localRotation =  Quaternion.identity;
        }

        public void SetActivated(bool state)
        {
            windowTransform.gameObject.SetActive(state);
        }

        public void SetButtonsInteractable(bool interactable)
        {
            stick.Button.interactable = interactable;
            bluePencil.Button.interactable = interactable;
            greenPencil.Button.interactable = interactable;
            redPencil.Button.interactable = interactable;
            yellowPencil.Button.interactable = interactable;
            
            buttonClosePencil.interactable = interactable;

            scrollRect.enabled = interactable;
        }
        public void ChangePencilButtons(bool isOpen)
        {
            buttonClosePencil.image.raycastTarget = isOpen;
        }
        public void Clear()
        {
            itemFocus.gameObject.SetActive(false);
        }

        public void ChangePencilColor(Color color)
        {
            pencilImage.color = color;
        }
        public void ChangeItem(ItemVariable item, bool active)
        {
            var currentItem = GetPencilItem(item);
            currentItem.gameObject.SetActive(active);
        }
        public void FocusItem(ItemVariable item)
        {
            var currentItem = GetPencilItem(item);
            
            itemFocus.gameObject.SetActive(true);
            itemFocus.Focus(currentItem.transform);
            
            currentItem.SetImageAlpha(0);
        }
        public void FocusItemClick(ItemVariable item, bool claim, Action onComplete)
        {
            var currentItem = GetPencilItem(item);

            DOTween.To(
                currentItem.SetImageAlpha, 
                claim ? 0 : 1,
                claim ? 1 : 0,
                _uiSettings.uiAnimationDuration).onComplete = () =>
            {
                onComplete?.Invoke();
                
                itemFocus.gameObject.SetActive(false);
            };
        }
        
        public void ChangeShakeAnimation(bool active)
        {
            if(active)
            {
                _shakingSequence.Play();
            }
            else
            {
                _shakingSequence.Pause();
            }
        }
        private PencilItemMono GetPencilItem(ItemVariable pencil)
        {
            PencilItemMono currentPencil = null;
            switch (pencil)
            {
                case ItemVariable.Stick:
                {
                    currentPencil = stick;
                    break;
                }
                case ItemVariable.PencilBlue:
                {
                    currentPencil = bluePencil;
                    break;
                }
                case ItemVariable.PencilGreen:
                {
                    currentPencil = greenPencil;
                    break;
                }
                case ItemVariable.PencilRed:
                {
                    currentPencil = redPencil;
                    break;
                }
                case ItemVariable.PencilYellow:
                {
                    currentPencil = yellowPencil;
                    break;
                }
                default:
                {
                    this.LogError("No such item as " + pencil);
                    break;
                }
            }

            return currentPencil;
        }
    }
}
