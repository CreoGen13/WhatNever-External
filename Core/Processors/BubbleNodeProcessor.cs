using System;
using System.Collections.Generic;
using Core.Base.Classes;
using Core.Game;
using Core.Infrastructure.Enums;
using Core.Infrastructure.Utils;
using Core.Node.Panel;
using Scriptables.Holders;
using UI.MainMenu;

namespace Core.Processors
{
    public class BubbleNodeProcessor : BaseNodeProcessor
    {
        private readonly ScriptableBubblesSpriteHolder _bubblesSpriteHolder;
        private MainMenuPresenter _mainMenuPresenter;

        public BubbleNodeProcessor(LoadedNodeData loadedNodeData, GamePresenter gamePresenter, ScriptableBubblesSpriteHolder bubblesSpriteHolder, MainMenuPresenter mainMenuPresenter) : 
            base(loadedNodeData, gamePresenter)
        {
            _mainMenuPresenter = mainMenuPresenter;
            _bubblesSpriteHolder = bubblesSpriteHolder;
        }

        public override void Activate(Action onComplete)
        {
            var bubble = GamePresenter.GameView.SpawnBubble(_bubblesSpriteHolder.bubblePrefabs[LoadedNodeData.BubblePrefabNumber - 1], Stage.Bubble);
            bubble.Initialize(LoadedNodeData, _bubblesSpriteHolder.bubbleSprites[LoadedNodeData.BubbleSpriteNumber - 1], _bubblesSpriteHolder.bubbleSound, onComplete);
            bubble.SetActive(true);
            
            _mainMenuPresenter.AddSentence(LoadedNodeData.Author, LoadedNodeData.Text);
            
            if (!GamePresenter.GameModel.GameSpritesDictionary.TryAdd(LoadedNodeData.NodeData, bubble))
            {
                this.LogError("You are trying to make sprite with already associated node. Make sure you called DeleteAllTagged before");
            }
        }
    }
}