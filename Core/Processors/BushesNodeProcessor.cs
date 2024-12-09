using System;
using Core.Base.Classes;
using Core.Game;
using Core.Infrastructure.Services;
using Core.Mechanics.Bushes;
using Core.Node.Panel;

namespace Core.Processors
{
    public class BushesNodeProcessor : BaseNodeProcessor
    {
        private BushesMechanics _bushesMechanics;
        private LoadService _loadService;

        public BushesNodeProcessor(LoadedNodeData loadedNodeData, GamePresenter gamePresenter, LoadService loadService)
            : base(loadedNodeData, gamePresenter)
        {
            _loadService = loadService;
        }

        public override void Activate(Action onComplete)
        {
            _bushesMechanics = GamePresenter.GameView.SpawnMechanics<BushesMechanics>(LoadedNodeData.GameObject, LoadedNodeData.Stage);
            if (LoadedNodeData.StartWithDeferredEvent)
            {
                _bushesMechanics.Activate(LoadedNodeData.SpritePosition);
                _bushesMechanics.Pause();
                
                GamePresenter.GameModel.AddToDeferredEvent(LoadedNodeData.DeferredEventType, (onDeferredComplete) =>
                {
                    _bushesMechanics.SetOnComplete(() =>
                    {
                        _loadService.UnloadNodeData(LoadedNodeData.NodeData);
                        
                        onDeferredComplete?.Invoke();
                    });
                    _bushesMechanics.Continue();
                });
                
                return;
            }

            _bushesMechanics.Activate(LoadedNodeData.SpritePosition);
            _bushesMechanics.SetOnComplete(() =>
            {
                _loadService.UnloadNodeData(LoadedNodeData.NodeData);
                
                onComplete?.Invoke();
            });
        }
    }
}