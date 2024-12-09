using System;
using Core.Base.Classes;
using Core.Game;
using Core.Node.Panel;

namespace Core.Processors
{
    public class ZoomNodeProcessor : BaseNodeProcessor
    {
        public ZoomNodeProcessor(LoadedNodeData loadedNodeData, GamePresenter gamePresenter)
            : base(loadedNodeData, gamePresenter)
        {
        }

        public override void Activate(Action onComplete)
        {
            GamePresenter.GameView.PlayZoom(LoadedNodeData.SpritePosition, LoadedNodeData.MovePosition.x, onComplete);
        }
    }
}