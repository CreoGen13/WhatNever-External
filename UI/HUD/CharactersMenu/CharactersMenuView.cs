using System;
using Core.Infrastructure.Enums;
using Core.Infrastructure.Services;
using DG.Tweening;
using Scriptables.Settings;
using UI.Base.DefaultMenu;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.HUD.CharactersMenu
{
    public class CharactersMenuView : MenuView
    {
        [SerializeField] private Button buttonCharactersOk;
        [SerializeField] private CharacterDescriptionMono[] characters;

        #region ButtonEvents

        public event Action ClickButtonOkEvent;

        #endregion
        
        private ScriptableUiSettings _uiSettings;
        private SoundService _soundService;

        [Inject]
        public void Construct(CharactersMenuPresenter presenter, ScriptableUiSettings uiSettings, SoundService soundService)
        {
            _soundService = soundService;
            _uiSettings = uiSettings;
            
            foreach (var character in characters)
            {
                character.BaseStart();
            }
            
            InitButtons();
        }

        protected sealed override void InitButtons()
        {
            buttonCharactersOk.onClick.AddListener(() =>
            {
                ClickButtonOkEvent?.Invoke();
            });

            foreach (var currentCharacter in characters)
            {
                currentCharacter.Button.onClick.AddListener(() =>
                {
                    foreach (var otherCharacter in characters)
                    {
                        otherCharacter.Close();
                        
                        if(currentCharacter == otherCharacter)
                        {
                            currentCharacter.Open();
                        }
                    }
                });
            }
        }

        public void SetInteractableButtons(bool interactable)
        {
            buttonCharactersOk.interactable = interactable;
            foreach (var character in characters)
            {
                character.Button.interactable = interactable;
            }
        }
        public void ActivateCharacter(CharacterName characterName)
        {
            var currentCharacter = ChooseCharacter(characterName);
            foreach (var otherCharacter in characters)
            {
                otherCharacter.Close();
                
                if (currentCharacter == otherCharacter)
                {
                    currentCharacter.Activate();
                }
            }
            
        }
        public void UpdateCharacterFeatures(CharacterName characterName, bool [] featuresStates)
        {
            ChooseCharacter(characterName).UpdateFeatures(featuresStates);
        }
        public void DeactivateCharacter(CharacterName characterName)
        {
            var currentCharacter = ChooseCharacter(characterName);
            currentCharacter.Deactivate();
        }
        
        protected override void OpenMenuInternal(Action callback = null)
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
                callback?.Invoke();
            });
            Sequence.Play();
        }
        protected override void CloseMenuInternal(Action callback = null)
        {
            _soundService.PlayMenuSound(MenuSound.SmallTreeOutro);
            
            windowTransform.localPosition = Vector3.zero;
            
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
                
                callback?.Invoke();
            });
            Sequence.Play();
        }

        public override void OpenMenuInstant()
        {
            windowTransform.localPosition = Vector3.zero;
            windowTransform.gameObject.SetActive(true);
            background.color = _uiSettings.backgroundFadeColor;
        }

        public override void CloseMenuInstant()
        {
            windowTransform.localPosition = new Vector3(0, Screen.height, 0);
            windowTransform.gameObject.SetActive(false);
            background.color = new Color(0, 0, 0, 0);
        }

        private CharacterDescriptionMono ChooseCharacter(CharacterName characterName)
        {
            switch (characterName)
            {
                case CharacterName.Deer:
                {
                    return characters[0];
                }
                case CharacterName.Snail:
                {
                    return characters[1];
                }
                default:
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}