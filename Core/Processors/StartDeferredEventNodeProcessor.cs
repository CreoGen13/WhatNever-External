using System;
using Core.Base.Classes;
using Core.Game;
using Core.Node.Panel;

namespace Core.Processors
{
    public class StartDeferredEventNodeProcessor : BaseNodeProcessor
    {
        public StartDeferredEventNodeProcessor(LoadedNodeData loadedNodeData, GamePresenter gamePresenter)
            : base(loadedNodeData, gamePresenter)
        {
        }

        public override void Activate(Action onComplete)
        {
            GamePresenter.GameModel.StartDeferredEvent(LoadedNodeData.DeferredEventType, onComplete);
        }
    }
}