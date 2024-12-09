using System;
using Core.Infrastructure.Enums;
using Core.Node.Panel;
using Core.Sprites.Base;
using UnityEngine;

namespace Core.Sprites.GameSprite
{
    public class GameSpritePresenter : BaseGameGraphicsPresenter<GameSpriteModel, GameSpriteView>
    {
        public event Action<GameSpritePresenter> ReturnEvent;
        
        public override SpriteTag SpriteTag => Model.SpriteTag;

        private Action _enableAction;
        private Action _buttonAction;
        private Action _buttonEndAction;
        private Action _animationEndAction;
        
        public GameSpritePresenter(GameSpriteModel model, GameSpriteView view)
            : base(model, view)
        {
            View.Init();

            InitActions();
            InitSubscriptions();
        }

        protected sealed override void InitActions()
        {
            View.AnimationEndEvent += () => { _animationEndAction?.Invoke(); };
            View.ButtonClickEvent += () => { _buttonAction?.Invoke(); };
            View.EnableEvent += () => { _enableAction?.Invoke(); };
        }

        protected sealed override void InitSubscriptions()
        {
            AddSubscriptionWithDistinct(model => model.HasButton, value =>
            {
                View.Button.enabled = value;
            });
            AddSubscriptionWithDistinct(model => model.HasAnimation, value =>
            {
                View.Animator.enabled = value;
            });
            AddSubscriptionWithDistinct(model => model.HasAudio, value =>
            {
                View.AudioSource.enabled = value;
            });
            AddSubscriptionWithDistinct(model => model.IsActive, value =>
            {
                if (!value)
                {
                    Reset();
                    
                    return;
                }

                if (Model.IsDeferred)
                {
                    EnableSprite();
                    
                    return;
                }
                
                if (Model.HasAudio)
                {
                    View.SetAudioState(true);
                }

                if (Model.HasAnimation)
                {
                    View.SetAnimationState(true);
                }

                EnableSprite();
            });
        }
        
        public void Initialize(LoadedNodeData nodeData, Action onComplete)
        {
            var position = nodeData.ArrivalType == ArrivalType.Moving ? nodeData.MovePosition : nodeData.SpritePosition; 
            
            SetDefault(nodeData, nodeData.Sprite, position);
            SetButton(nodeData.BoolValue, nodeData.ClickEvent);
            SetAnimator(nodeData.HasAnimation, nodeData.AnimatorController);
            SetAudio(nodeData.HasAudio, nodeData.AudioClip);
            SetOnComplete(onComplete);
        }
        
        private void SetDefault(LoadedNodeData nodeData, Sprite sprite, Vector3 spritePosition)
        {
            Model.ArrivalType = nodeData.ArrivalType;
            Model.SpriteTag = nodeData.SpriteTag;
            Model.EndPosition = nodeData.SpritePosition;
            Model.WaitForAnimationEnd = nodeData.WaitForAnimationEnd;
            Model.IsDeferred = nodeData.StartWithDeferredEvent;
            Model.Update();
            
            View.SetSprite(sprite, nodeData.Color);
            View.SetDefaultSize();
            View.SetTransform(spritePosition, nodeData.ScaleAndRotation, nodeData.Stage);
        }
        private void SetOnComplete(Action onComplete)
        {
            if (Model.IsDeferred)
            {
                _enableAction = onComplete;
            }
            else if(Model.HasAnimation && Model.WaitForAnimationEnd)
            {
                _animationEndAction = onComplete;
            }
            else if(Model.HasButton)
            {
                _buttonEndAction = onComplete;
            }
            else
            {
                _enableAction = onComplete;
            }
        }

        private void SetButton(bool hasButton, ClickEventType clickEventType)
        {
            Model.HasButton = hasButton;
            Model.ClickEventType = clickEventType;
            Model.Update();

            if (!hasButton)
            {
                return;
            }
            
            switch (clickEventType)
            {
                case ClickEventType.ChangeVariable:
                case ClickEventType.JustDelete:
                {
                    _buttonAction = () => DisableAndReturn(true, true, _buttonEndAction);
                        
                    break;
                }
                case ClickEventType.JustDisable:
                {
                    _buttonAction = () =>
                    {
                        Model.HasButton = false;
                        Model.Update();
                
                        _buttonEndAction?.Invoke();
                    };
                        
                    break;
                }
            }
        }
        private void SetAnimator(bool hasAnimation, RuntimeAnimatorController animatorController)
        {
            Model.HasAnimation = hasAnimation;
            Model.Update();

            if (hasAnimation)
            {
                View.SetAnimation(animatorController);
            }
        }
        private void SetAudio(bool hasAudio, AudioClip audioClip)
        {
            Model.HasAudio = hasAudio;
            Model.Update();

            if (hasAudio)
            {
                View.SetAudio(audioClip);
            }
        }
        
        public override void EnableSprite()
        {
            View.EnableSprite(Model.ArrivalType, Model.EndPosition);
        }

        protected override void OnReturnInvoke()
        {
            ReturnEvent?.Invoke(this);
        }

        protected override void Reset()
        {
            base.Reset();
            
            Model.HasAudio = false;
            Model.HasAnimation = false;
            Model.WaitForAnimationEnd = false;
            Model.HasButton = false;
            Model.IsDeferred = false;
            Model.EndPosition = Vector3.zero;
            Model.Update();

            _enableAction = null;
            _buttonAction = null;
            _buttonEndAction = null;
            _animationEndAction = null;
        }
        
        public void StartDeferred(Action onComplete)
        {
            if (Model.HasAudio)
            {
                View.SetAudioState(true);
            }
            
            if (Model.HasAnimation)
            {
                View.SetAnimationState(true);
            }
            
            if(Model.HasAnimation && Model.WaitForAnimationEnd)
            {
                _animationEndAction = onComplete;
            }
            else if(Model.HasButton)
            {
                _buttonEndAction = onComplete;
            }
            else
            {
                onComplete?.Invoke();
            }
        }
        public void StopDeferred(Action onComplete)
        {
            if (Model.HasAudio)
            {
                View.SetAudioState(false);
            }
            
            if (Model.HasAnimation)
            {
                View.SetAnimationState(false);
            }
            
            onComplete?.Invoke();
        }
    }
}