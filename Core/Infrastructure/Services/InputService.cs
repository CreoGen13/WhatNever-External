using Core.Base.Classes;
using Scriptables.Settings;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Core.Infrastructure.Services
{
    public class InputService : BaseService
    {
        private readonly PlayerInput _playerInput;

        private InputAction _clickedAction;
        private InputAction _scrollAction;
        private InputAction _mousePositionAction;

        private bool _firstClick = true;
        private readonly float _screenFactor = Mathf.Sqrt(Screen.width + Screen.height);

        private ScriptableInputSettings _inputSettings;

        private Input _input;
        
        [Inject]
        public InputService(ScriptableInputSettings inputSettings, PlayerInput playerInput)
        {
            _inputSettings = inputSettings;
            _playerInput = playerInput;
        }

        public override void Initialize()
        {
            _clickedAction = _playerInput.actions["Click"];
            _scrollAction = _playerInput.actions["MouseScroll"];
            _mousePositionAction = _playerInput.actions["MousePosition"];
        }

        public override void Update()
        {
            _input = GetInputInternal();
        }

        public Input GetInput()
        {
            return _input;
        }

        private Input GetInputInternal()
        {
            var position = _mousePositionAction.ReadValue<Vector2>();
            var delta = position - _input.Position;

            switch (_clickedAction.phase)
            {
                case InputActionPhase.Performed when _firstClick: // Click
                {
                    _firstClick = false;
                    return new Input
                    {
                        Position = position,
                        Delta = delta,
                        Phase = InputActionPhase.Performed,
                        InputType = InputType.Click
                    };
                }
                case InputActionPhase.Performed when !_firstClick: // Scroll
                {
                    return new Input
                    {
                        Position = position,
                        Delta = delta,
                        Phase = InputActionPhase.Performed,
                        InputType = InputType.Scroll
                    };
                }
                case InputActionPhase.Waiting when _clickedAction.WasReleasedThisFrame(): // End scroll/click
                {
                    _firstClick = true;
                    
                    return new Input
                    {
                        Position = position,
                        Delta = delta,
                        Phase = InputActionPhase.Canceled,
                        InputType = _input.InputType
                    };
                }
            }
            
            var scrollDelta = Vector2.down * (_scrollAction.ReadValue<float>() / _screenFactor * _inputSettings.scrollingSpeed);

            switch (_scrollAction.phase)
            {
                case InputActionPhase.Started: // Start scroll
                {
                    return new Input
                    {
                        Position = position,
                        Delta = scrollDelta,
                        Phase = InputActionPhase.Started,
                        InputType = InputType.Scroll
                    };
                }
                case InputActionPhase.Performed: // Scroll
                {
                    return new Input
                    {
                        Position = position,
                        Delta = scrollDelta,
                        Phase = InputActionPhase.Performed,
                        InputType = InputType.Scroll
                    };
                }
                case InputActionPhase.Waiting when _scrollAction.WasReleasedThisFrame(): // End scroll
                {
                    return new Input
                    {
                        Position = position,
                        Delta = scrollDelta,
                        Phase = InputActionPhase.Canceled,
                        InputType = InputType.Scroll
                    };
                }
            }

            return new Input
            {
                Position = position,
                Delta = delta,
                Phase = InputActionPhase.Waiting,
                InputType = InputType.None
            };
        }

        public struct Input
        {
            public Vector2 Delta { get; set; }
            public Vector2 Position { get; set; }
            public InputActionPhase Phase { get; set; }
            public InputType InputType { get; set; }
        }

        public enum InputType
        {
            None,
            Click,
            Scroll
        }
    }
}
