using Core.Base.Classes;
using DG.Tweening;
using Scriptables.Settings;
using UnityEngine;
using Zenject;

namespace Core.Mechanics.Bushes
{
    public class BushesMechanics : BaseMechanics
    {
        [SerializeField] private RectTransform moveRectangle;
        [SerializeField] private Bush[] bushes;
        
        private ScriptableGameSettings _gameSettings;
        private Sequence _sequence;

        private Vector2 _xBounds;
        private Vector2 _yBounds;
        private int _count;
        private ScriptableUiSettings _uiSettings;

        [Inject]
        private void Construct(ScriptableGameSettings gameSettings, ScriptableUiSettings uiSettings)
        {
            _gameSettings = gameSettings;
            _uiSettings = uiSettings;

            _xBounds = new Vector2(
                moveRectangle.localPosition.x - moveRectangle.sizeDelta.x / 2,
                moveRectangle.localPosition.x + moveRectangle.sizeDelta.x / 2);
            _yBounds = new Vector2(
                moveRectangle.localPosition.y - moveRectangle.sizeDelta.y / 2,
                moveRectangle.localPosition.y + moveRectangle.sizeDelta.y / 2);
            
            foreach (var bush in bushes)
            {
                bush.Init(_gameSettings.bushFadingDuration);
            }
        }
        public override void Activate(Vector3 position)
        {
            base.Activate(position);

            _count = bushes.Length;
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
            
            foreach (var bush in bushes)
            {
                if(!bush.IsDragging || !bush.IsActive)
                {
                    continue;
                }

                if (IsBushInsideRectangle(bush))
                {
                    bush.Drag(input);
                }
                else
                {
                    bush.Complete();
                    OnBushComplete();
                }
                
                break;
            }
        }

        private void OnBushComplete()
        {
            _count--;

            if (_count != 0)
            {
                return;
            }

            Deactivate();
        }
        
        private bool IsBushInsideRectangle(Bush bush)
        {
            var centerPosition = bush.CenterPosition;
            
            if (centerPosition.x < _xBounds.x || centerPosition.x > _xBounds.y)
            {
                return false;
            }
            
            if (centerPosition.y < _yBounds.x || centerPosition.y > _yBounds.y)
            {
                return false;
            }

            return true;
        }
    }
}