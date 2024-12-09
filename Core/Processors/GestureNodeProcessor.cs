using System;
using Core.Base.Classes;
using Core.Game;
using Core.Infrastructure.Services;
using Core.Mechanics.Gesture;
using Core.Node.Panel;

namespace Core.Processors
{
    public class GestureNodeProcessor : BaseNodeProcessor
    {
        private GestureMechanics _gestureMechanics;
        private readonly LoadService _loadService;

        public GestureNodeProcessor(LoadedNodeData loadedNodeData, GamePresenter gamePresenter, LoadService loadService)
            : base(loadedNodeData, gamePresenter)
        {
            _loadService = loadService;
        }

        public override void Activate(Action onComplete)
        {
            _gestureMechanics = GamePresenter.GameView.SpawnMechanics<GestureMechanics>(LoadedNodeData.GameObject, LoadedNodeData.Stage);
            _gestureMechanics.Activate(LoadedNodeData.SpritePosition);
            _gestureMechanics.SetOnComplete(() =>
            {
                _loadService.UnloadNodeData(LoadedNodeData.NodeData);
                
                GamePresenter.GameModel.InstantNextMove = true;
                GamePresenter.GameModel.Update();
                
                onComplete?.Invoke();
            });
        }
    }
}