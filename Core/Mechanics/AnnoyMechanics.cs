using System;
using Core.Base.Classes;
using Core.Infrastructure.Services;
using DG.Tweening;
using Scriptables.Settings;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.Mechanics
{
    public class AnnoyMechanics : BaseMechanics
    {
        [SerializeField] private float scoreMultiplier;
        [SerializeField] private RectTransform sliderParent;
        [SerializeField] private Slider slider;
        
        private ScriptableGameSettings _gameSettings;
        private Sequence _sequence;

        private float _value;
        
        [Inject]
        private void Construct(ScriptableGameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }
        
        public override void ManualUpdate()
        {
            if (IsBusy)
            {
                return;
            }
            
            _value -= Time.deltaTime * scoreMultiplier;
            _value = Mathf.Clamp(_value, 0, 1);
            if (InputService.GetInput().InputType == InputService.InputType.Click)
            {
                _value += 0.2f;
                _value = Mathf.Clamp(_value, 0, 1);
            }

            slider.value = _value;
            if (Math.Abs(_value - 1f) <= 0)
            {
                Deactivate();
            }
        }

        public override void Activate(Vector3 position)
        {
            base.Activate(position);
            
            sliderParent.gameObject.SetActive(false);
            slider.value = 0;
            _value = 0;

            CompleteAction = null;
            
            IsValidating = true;
            sliderParent.gameObject.SetActive(true);
            sliderParent.anchoredPosition =  Vector3.zero;
            slider.value = 0;
            _value = 0;
            
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Join(sliderParent.DOAnchorPos(
                new Vector3(0, sliderParent.rect.height, 0),
                _gameSettings.annoySliderDuration));
            _sequence.OnComplete(() =>
            {
                IsValidating = false;
            });
            _sequence.Play();
        }

        protected override void Deactivate()
        {
            IsValidating = true;
            
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Join(sliderParent.DOAnchorPos(
                Vector3.zero,
                _gameSettings.annoySliderDuration));
            _sequence.OnComplete(() =>
            {
                IsValidating = false;

                base.Deactivate();
            });
            _sequence.Play();
        }
    }
}