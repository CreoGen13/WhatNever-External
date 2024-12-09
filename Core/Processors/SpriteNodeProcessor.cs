using System;
using System.Collections.Generic;
using Core.Base.Classes;
using Core.Game;
using Core.Infrastructure.Enums;
using Core.Infrastructure.Enums.GameVariables;
using Core.Infrastructure.Utils;
using Core.Node.Panel;
using Core.Sprites.GameSprite;

namespace Core.Processors
{
    public class SpriteNodeProcessor : BaseNodeProcessor
    {
        private GameSpritePresenter _gameSpritePresenter;
        
        public SpriteNodeProcessor(LoadedNodeData loadedNodeData, GamePresenter gamePresenter)
            : base(loadedNodeData, gamePresenter)
        {
        }

        public override void Activate(Action onComplete)
        {
            var sprite = GamePresenter.GameView.SpawnSprite(LoadedNodeData.SpriteTag, LoadedNodeData.Stage);

            Action onProcessorComplete = null;
            Action onSpriteComplete = onComplete;

            if (LoadedNodeData.BoolValue && LoadedNodeData.ClickEvent == ClickEventType.ChangeVariable)
            {
                onProcessorComplete = onComplete;
                
                onSpriteComplete = () =>
                {
                    GamePresenter.GameModel.ChangeGlobalVariable(LoadedNodeData.GameVariableType, LoadedNodeData.GameVariableValue, true);
                    GamePresenter.GameModel.Update();
                };
            }
            
            sprite.Initialize(LoadedNodeData, onSpriteComplete);
            sprite.SetActive(true);

            if (LoadedNodeData.StartWithDeferredEvent)
            {
                GamePresenter.GameModel.AddToDeferredEvent(LoadedNodeData.DeferredEventType, (onDeferredComplete) =>
                {
                    sprite.StartDeferred(onDeferredComplete);
                });
            }
            
            if (LoadedNodeData.StopWithDeferredEvent)
            {
                GamePresenter.GameModel.AddToDeferredEvent(LoadedNodeData.StopDeferredEventType, (onDeferredComplete) =>
                {
                    sprite.StopDeferred(onDeferredComplete);
                });
            }
            
            if (!GamePresenter.GameModel.GameSpritesDictionary.TryAdd(LoadedNodeData.NodeData, sprite))
            {
                this.LogError("You are trying to make sprite with already associated node. Make sure you called DeleteAllTagged before");
            }
            
            onProcessorComplete?.Invoke();
        }
    }
}