using System;
using Core.Base.Classes;
using Core.Infrastructure.Serializable;
using Core.Mono;
using DG.Tweening;
using Scriptables.Settings;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Core.Mechanics.Poster
{
    public class PosterMechanics : BaseMechanics
    {
        [Header("Variables")]
        [SerializeField] private float pointDistanceAccuracy;

        [Header("References")]
        [SerializeField] private SerializableDictionary<PointHolder, PosterPart> parts;

        private ScriptableUiSettings _uiSettings;
        private Sequence _sequence;

        private int _count;

        [Inject]
        public void Construct(Camera mainCamera, ScriptableUiSettings uiSettings)
        {
            _uiSettings = uiSettings;

            foreach (var pair in parts)
            {
                pair.Value.Init();
            }
        }
        
        public override void Activate(Vector3 position)
        {
            base.Activate(position);
            
            _count = parts.Count;
        }

        protected override void Deactivate()
        {
            IsValidating = true;
            
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Join(DOTween.To(() => 0, value => { }, 1, _uiSettings.uiAnimationDuration));
            _sequence.OnComplete(() =>
            {
                IsValidating = false;

                base.Deactivate();
            });
            _sequence.Play();
        }

        public override void ManualUpdate()
        {
            if (IsBusy)
            {
                return;
            }

            var input = InputService.GetInput();
            
            foreach (var pair in parts)
            {
                var posterPart = pair.Value;

                if (!posterPart.IsActive)
                {
                    continue;
                }
                
                var point = pair.Key;
                
                if (IsPartNearPoint(posterPart, point) && input.Phase == InputActionPhase.Canceled)
                {
                    posterPart.Complete(point.RectTransform.localPosition);
                    OnPosterPartComplete();
                }
                
                if(!posterPart.IsDragging)
                {
                    continue;
                }
                
                posterPart.Drag(input);
            }
        }

        private bool IsPartNearPoint(PosterPart posterPart, PointHolder point)
        {
            return Vector2.Distance(point.RectTransform.localPosition, posterPart.CenterPosition) < pointDistanceAccuracy;
        }

        private void OnPosterPartComplete()
        {
            _count--;

            if (_count != 0)
            {
                return;
            }

            Deactivate();
        }
    }
}