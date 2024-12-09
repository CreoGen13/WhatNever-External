using System;
using Core.Base.Classes;
using Core.Game;
using Core.Node.Panel;

namespace Core.Processors
{
    public class EndNodeProcessor : BaseNodeProcessor
    {
        public EndNodeProcessor(LoadedNodeData loadedNodeData, GamePresenter gamePresenter)
            : base(loadedNodeData, gamePresenter)
        {
        }

        public override void Activate(Action onComplete)
        {
            GamePresenter.EndPanel();
        }
    }
}