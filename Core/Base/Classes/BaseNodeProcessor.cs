using System;
using Core.Game;
using Core.Node.Panel;
using Zenject;

namespace Core.Base.Classes
{
    public abstract class BaseNodeProcessor
    {
        protected readonly LoadedNodeData LoadedNodeData;
        protected readonly GamePresenter GamePresenter;
        
        [Inject]
        protected BaseNodeProcessor(LoadedNodeData loadedNodeData, GamePresenter gamePresenter)
        {
            LoadedNodeData = loadedNodeData;
            GamePresenter = gamePresenter;
        }

        public abstract void Activate(Action onComplete);
    }
}