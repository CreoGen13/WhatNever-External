using System;
using Core.Infrastructure.Services;
using DG.Tweening;
using Scriptables.Settings;
using UI.Base.TabMenu;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.SettingsMenu
{
    public class SettingsMenuView : TabMenuView<SettingsMenuType>
    {
         [Header("Sides")]
         [SerializeField] private RectTransform left;
         [SerializeField] private RectTransform right;
         [SerializeField] private RectTransform[] trees = new RectTransform[4];
         
         [Header("Left Buttons")]
         [SerializeField] private Button buttonStart;
         [SerializeField] private Button buttonSettings;
         [SerializeField] private Button buttonAbout;

         [Header("Tree Settings")]
         [SerializeField] private Button buttonSettingsOk;
         [SerializeField] private Button buttonLanguageRussian;
         [SerializeField] private Button buttonLanguageEnglish;
         [SerializeField] private Slider sliderVolumeEffects;
         [SerializeField] private Image sliderEffectsImage;
         [SerializeField] private Sprite[] sliderEffectsSprites = new Sprite[3];
         [SerializeField] private Slider sliderVolumeMusic;
         [SerializeField] private Image sliderMusicImage;
         [SerializeField] private Sprite[] sliderMusicSprites = new Sprite[3];
         
         [Header("Tree Choice")]
         [SerializeField] private Button buttonAboutProject;
         [SerializeField] private Button buttonAboutUs;

         [Header("Tree About Project")]
         [SerializeField] private Button buttonAboutProjectOk;
         
         [Header("Tree About Us")]
         [SerializeField] private Button buttonAboutUsOk;

         #region ButtonEvents

         public event Action OnClickButtonLanguageRussian;
         public event Action OnClickButtonLanguageEnglish;
         public event Action OnClickButtonSettingsOk;
         public event Action OnClickButtonStart;
         public event Action<SettingsMenuType> OnClickButtonSettings;
         public event Action<SettingsMenuType> OnClickButtonAbout;
         public event Action<SettingsMenuType> OnClickButtonAboutProject;
         public event Action<SettingsMenuType> OnClickButtonAboutUs;
         public event Action<SettingsMenuType> OnClickButtonAboutProjectOk;
         public event Action<SettingsMenuType> OnClickButtonAboutUsOk;

         #endregion
         
         private ScriptableUiSettings _uiSettings;
         private SoundService _soundService;
         private Sequence _tabSequence;
         private bool _isLeftActive;
         private SettingsMenuType _currentTab;

         [Inject]
         private void Construct(SettingsMenuPresenter presenter, ScriptableUiSettings uiSettings, SoundService soundService)
         {
             _soundService = soundService;
             _uiSettings = uiSettings;
             
             InitButtons();
             InitSliders();
         }
         protected sealed override void InitButtons()
         {
             buttonLanguageRussian.interactable = false;
             buttonLanguageEnglish.interactable = true;
             
             buttonLanguageRussian.onClick.AddListener(() =>
             {
                 _soundService.PlayButtonSound();
                 
                 buttonLanguageRussian.interactable = false;
                 buttonLanguageEnglish.interactable = true;
                 
                 OnClickButtonLanguageRussian?.Invoke();
             });
             buttonLanguageEnglish.onClick.AddListener(() =>
             {
                 _soundService.PlayButtonSound();
                 
                 buttonLanguageRussian.interactable = true;
                 buttonLanguageEnglish.interactable = false;
                 
                 OnClickButtonLanguageEnglish?.Invoke();
             });
             buttonSettingsOk.onClick.AddListener(() =>
             {
                 _soundService.PlayButtonSound();
                 
                 OnClickButtonSettingsOk?.Invoke();
             });
             buttonStart.onClick.AddListener(() =>
             {
                 _soundService.PlayButtonSound();
                 
                 OnClickButtonStart?.Invoke();
             });
             buttonSettings.onClick.AddListener(() =>
             {
                 _soundService.PlayButtonSound();
                 
                 OnClickButtonSettings?.Invoke(SettingsMenuType.Settings);
             });
             buttonAbout.onClick.AddListener(() =>
             {
                 _soundService.PlayButtonSound();
                 
                 OnClickButtonAbout?.Invoke(SettingsMenuType.About);
             });
             buttonAboutProject.onClick.AddListener(() =>
             {
                 _soundService.PlayButtonSound();
                 
                 OnClickButtonAboutProject?.Invoke(SettingsMenuType.AboutProject);
             });
             buttonAboutUs.onClick.AddListener(() =>
             {
                 _soundService.PlayButtonSound();
                 
                 OnClickButtonAboutUs?.Invoke(SettingsMenuType.AboutUs);
             });
             buttonAboutProjectOk.onClick.AddListener(() =>
             {
                 _soundService.PlayButtonSound();
                 
                 
                 OnClickButtonAboutProjectOk?.Invoke(SettingsMenuType.About);
             });
             buttonAboutUsOk.onClick.AddListener(() =>
             {
                 _soundService.PlayButtonSound();
                 
                 OnClickButtonAboutUsOk?.Invoke(SettingsMenuType.About);
             });
         }
         private void InitSliders()
         {
             sliderVolumeMusic.onValueChanged.AddListener((value) =>
             {
                 if (value < 0.5f)
                 {
                     sliderMusicImage.sprite = value == 0 ? sliderMusicSprites[0] : sliderMusicSprites[1];
                 }
                 else
                 {
                     sliderMusicImage.sprite = sliderMusicSprites[2];
                 }
                 
                 _soundService.SetMusicVolume(value);
             });
             sliderVolumeMusic.SetValueWithoutNotify(1);
             sliderVolumeEffects.onValueChanged.AddListener((value) =>
             {
                 if (value < 0.5f)
                 {
                     sliderEffectsImage.sprite = value == 0 ? sliderEffectsSprites[0] : sliderEffectsSprites[1];
                 }
                 else
                 {
                     sliderEffectsImage.sprite = sliderEffectsSprites[2];
                 }
                 _soundService.SetSoundVolume(value);
             });
             sliderVolumeEffects.SetValueWithoutNotify(1);
         }
         
         public void SetButtonsInteractable(bool interactable, bool  isSettings)
         {
             buttonStart.interactable = interactable;
             buttonSettings.interactable = interactable;
             buttonAbout.interactable = interactable;
             buttonSettingsOk.interactable = interactable;
             buttonLanguageRussian.interactable = interactable;
             buttonLanguageEnglish.interactable = interactable;
             sliderVolumeEffects.interactable = interactable;
             sliderVolumeMusic.interactable = interactable;
             buttonAboutProject.interactable = interactable;
             buttonAboutUs.interactable = interactable;
             buttonAboutProjectOk.interactable = interactable;
             buttonAboutUsOk.interactable = interactable;

             if (isSettings)
             {
                 buttonSettings.interactable = false;
             }
             else
             {
                 buttonAbout.interactable = false;
             }
         }
         
         protected override void OpenMenuWithTabInternal(SettingsMenuType settingsMenuType, Action callback = null)
         {
             windowTransform.gameObject.SetActive(true);
             
             _isLeftActive = settingsMenuType != SettingsMenuType.SmallSettings;
             left.gameObject.SetActive(_isLeftActive);

             Sequence = DOTween.Sequence();
             if (_isLeftActive)
             {
                 left.anchoredPosition = new Vector2(-left.rect.width, 0);
                 Sequence.Join(left.DOAnchorPos(
                     Vector2.zero,
                     _uiSettings.uiAnimationDuration));
             }
             Sequence.Join(OpenTab(settingsMenuType));
             Sequence.OnComplete(() =>
             {
                 callback?.Invoke();
             });
             Sequence.Play();
         }
         protected override void CloseMenuInternal(Action onComplete = null)
         {
             Sequence = DOTween.Sequence();
             if (_isLeftActive)
             {
                 left.anchoredPosition = Vector2.zero;
                 Sequence.Join(left.DOAnchorPos(
                     new Vector2(-left.rect.width, 0),
                     _uiSettings.uiAnimationDuration));
             }
             Sequence.Join(CloseTab(_currentTab));
             Sequence.OnComplete(() =>
             {
                 windowTransform.gameObject.SetActive(false);
                 
                 onComplete?.Invoke();
             });
             Sequence.Play();
         }
         public override Sequence OpenTab(SettingsMenuType settingsMenuType, Action callback = null)
         {
             right.anchoredPosition = new Vector2(right.rect.width, 0);
             
             _currentTab = settingsMenuType;
             var currentTree = ChooseTree(_currentTab);
             currentTree.gameObject.SetActive(true);

             _tabSequence = DOTween.Sequence();
             _tabSequence.Join(right.DOAnchorPos(
                 Vector2.zero,
                 _uiSettings.uiAnimationDuration));
             _tabSequence.OnComplete(() =>
             {
                 callback?.Invoke();
             });
             
             return _tabSequence;
         }
         public override Sequence CloseTab(SettingsMenuType settingsMenuType, Action callback = null)
         {
             right.anchoredPosition = Vector2.zero;
             var currentTree = ChooseTree(settingsMenuType);
             
             _tabSequence = DOTween.Sequence();
             _tabSequence.Join(right.DOAnchorPos(
                 new Vector2(right.rect.width, 0),
                 _uiSettings.uiAnimationDuration));
             _tabSequence.OnComplete(() =>
             {
                 currentTree.gameObject.SetActive(false);
                 callback?.Invoke();
             });
             return _tabSequence.Play();
         }
         
         public override void OpenMenuInstant()
         {
             windowTransform.gameObject.SetActive(true);
             
             var currentTree = ChooseTree(_currentTab);
             currentTree.gameObject.SetActive(true);
             
             left.anchoredPosition = Vector2.zero;
             right.anchoredPosition = Vector2.zero;
         }
         public override void CloseMenuInstant()
         {
             windowTransform.gameObject.SetActive(false);

             left.anchoredPosition = new Vector2(-left.rect.width, 0);
             right.anchoredPosition = new Vector2(right.rect.width, 0);
         }

         private RectTransform ChooseTree(SettingsMenuType settingsMenuType)
         {
             RectTransform tree = trees[0];
             switch (settingsMenuType)
             {
                 case SettingsMenuType.Settings:
                 {
                     tree = trees[0];
                     break;
                 }
                 case SettingsMenuType.SmallSettings:
                 {
                     tree = trees[0];
                     break;
                 }
                 case SettingsMenuType.About:
                 {
                     tree = trees[1];
                     break;
                 }
                 case SettingsMenuType.AboutProject:
                 {
                     tree = trees[2];
                     break;
                 }
                 case SettingsMenuType.AboutUs:
                 {
                     tree = trees[3];
                     break;
                 }
             }

             return tree;
         }
    }
}