using System;
using Core.Base.Classes;
using Core.Game;
using Core.Infrastructure.Services;
using Core.Mechanics.Poster;
using Core.Node.Panel;

namespace Core.Processors
{
    public class PosterPuzzleNodeProcessor : BaseNodeProcessor
    {
        private PosterMechanics _posterPuzzleMechanics;
        private readonly LoadService _loadService;
        
        public PosterPuzzleNodeProcessor(LoadedNodeData loadedNodeData, GamePresenter gamePresenter, LoadService loadService)
            : base(loadedNodeData, gamePresenter)
        {
            _loadService = loadService;
        }

        public override void Activate(Action onComplete)
        {
            _posterPuzzleMechanics = GamePresenter.GameView.SpawnMechanics<PosterMechanics>(LoadedNodeData.GameObject, LoadedNodeData.Stage);
            _posterPuzzleMechanics.Activate(LoadedNodeData.SpritePosition);
            _posterPuzzleMechanics.SetOnComplete(() =>
            {
                _loadService.UnloadNodeData(LoadedNodeData.NodeData);
                
                GamePresenter.GameModel.InstantNextMove = true;
                GamePresenter.GameModel.Update();
                
                onComplete?.Invoke();
            });
        }
    }
}