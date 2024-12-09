using System;
using Core.Base.Classes;
using Core.Game;
using Core.Infrastructure.Services;
using Core.Mechanics;
using Core.Node.Panel;

namespace Core.Processors
{
    public class AnnoyNodeProcessor : BaseNodeProcessor
    {
        private AnnoyMechanics _annoyMechanics;
        private LoadService _loadService;

        public AnnoyNodeProcessor(LoadedNodeData loadedNodeData, GamePresenter gamePresenter, LoadService loadService) 
            : base(loadedNodeData, gamePresenter)
        {
            _loadService = loadService;
        }

        public override void Activate(Action onComplete)
        {
            _annoyMechanics = GamePresenter.GameView.SpawnMechanics<AnnoyMechanics>(LoadedNodeData.GameObject, LoadedNodeData.Stage);
            _annoyMechanics.Activate(LoadedNodeData.SpritePosition);
            _annoyMechanics.SetOnComplete(() =>
            {
                _loadService.UnloadNodeData(LoadedNodeData.NodeData);
                
                GamePresenter.GameModel.InstantNextMove = true;
                GamePresenter.GameModel.Update();
                
                onComplete?.Invoke();
            });
        }
    }
}