using System;
using Core.Base.Classes;
using Core.Game;
using Core.Infrastructure.Enums.GameVariables;
using Core.Node.Panel;

namespace Core.Processors
{
    public class CheckVariableNodeProcessor : BaseNodeProcessor
    {
        public CheckVariableNodeProcessor(LoadedNodeData loadedNodeData, GamePresenter gamePresenter)
            : base(loadedNodeData, gamePresenter)
        {
        }

        public override void Activate(Action onComplete)
        {
            var value = false;
            var baseNodeName = GamePresenter.GameModel.CurrentNodeData.name;
            
            switch (LoadedNodeData.GameVariableType) {
                case { } gameVariableType when gameVariableType == typeof(GameVariable):
                {
                    var variable = (GameVariable)LoadedNodeData.GameVariableValue;
            
                    value = GamePresenter.GameModel.GameVariables[variable];
                    
                    break;
                }
                case { } itemVariableType when itemVariableType == typeof(ItemVariable):
                {
                    var variable = (ItemVariable)LoadedNodeData.GameVariableValue;
            
                    value = GamePresenter.GameModel.ItemVariables[variable];
                    
                    break;
                }
                case { } itemVariableType when itemVariableType == typeof(PosterPartVariable):
                {
                    var variable = (PosterPartVariable)LoadedNodeData.GameVariableValue;
            
                    value = GamePresenter.GameModel.PosterPartVariables[variable];
                    
                    break;
                }
            }
            
            if (GamePresenter.GameModel.LoadNextCheck(baseNodeName, value))
            {
                GamePresenter.GameModel.IsBlocked = false;
                GamePresenter.GameModel.InstantNextMove = true;
                GamePresenter.GameModel.Update();
            }
        }
    }
}