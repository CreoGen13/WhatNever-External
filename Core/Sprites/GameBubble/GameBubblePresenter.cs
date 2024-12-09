using System;
using Core.Infrastructure.Enums;
using Core.Node.Panel;
using Core.Sprites.Base;
using UnityEngine;

namespace Core.Sprites.GameBubble
{
    public class GameBubblePresenter : BaseGameGraphicsPresenter<GameBubbleModel, GameBubbleView>
    {
        public event Action<GameBubblePresenter> ReturnEvent;
        
        public override SpriteTag SpriteTag => SpriteTag.Bubble;
        public string PrefabName { get; set; }
        
        private Action _enableAction;

        public GameBubblePresenter(GameBubbleModel model, GameBubbleView view) : base(model, view)
        {
            InitActions();
            InitSubscriptions();
        }

        protected sealed override void InitActions()
        {
            View.EnableEvent += () => _enableAction?.Invoke();
        }

        protected sealed override void InitSubscriptions()
        {
            AddSubscriptionWithDistinct(model => model.IsActive, value =>
            {
                if (!value)
                {
                    Reset();
                    
                    return;
                }

                View.SetAudioState(true);

                EnableSprite();
            });
        }
        
        public void Initialize(LoadedNodeData nodeData, Sprite sprite, AudioClip audioClip, Action onComplete)
        {
            View.SetSprite(sprite, nodeData.Color, nodeData.Text);
            View.SetDefaultSize();
            View.SetTransform(nodeData.SpritePosition, nodeData.ScaleAndRotation, nodeData.Stage);
            View.SetAudio(audioClip);

            _enableAction = onComplete;
        }
        
        public override void EnableSprite()
        {
            View.EnableSprite(Model.ArrivalType);
        }

        protected override void OnReturnInvoke()
        {
            ReturnEvent?.Invoke(this);
        }

        protected override void Reset()
        {
            base.Reset();
            
            _enableAction = null;
        }
    }
}