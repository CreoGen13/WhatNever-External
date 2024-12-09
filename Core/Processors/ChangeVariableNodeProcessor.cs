using System;
using Core.Base.Classes;
using Core.Game;
using Core.Node.Panel;

namespace Core.Processors
{
    public class ChangeVariableNodeProcessor : BaseNodeProcessor
    {
        public ChangeVariableNodeProcessor(LoadedNodeData loadedNodeData, GamePresenter gamePresenter)
            : base(loadedNodeData, gamePresenter)
        { }

        public override void Activate(Action onComplete)
        {
            GamePresenter.GameModel.ChangeGlobalVariable(LoadedNodeData.GameVariableType, LoadedNodeData.GameVariableValue, LoadedNodeData.BoolValue);
            GamePresenter.GameModel.Update();
            
            onComplete?.Invoke();
        }
    }
}