using System;
using Core.Base.Classes;
using Core.Game;
using Core.Infrastructure.Enums;
using Core.Infrastructure.Enums.GameVariables;
using Core.Node.Panel;
using UI.MainMenu;

namespace Core.Processors
{
    public class ChangeItemNodeProcessor : BaseNodeProcessor
    {
        private readonly MainMenuPresenter _mainMenuPresenter;

        public ChangeItemNodeProcessor(LoadedNodeData loadedNodeData, GamePresenter gamePresenter, MainMenuPresenter mainMenuPresenter)
            : base(loadedNodeData, gamePresenter)
        {
            _mainMenuPresenter = mainMenuPresenter;
        }

        public override void Activate(Action onComplete)
        {
            if (!GamePresenter.GameModel.GameVariables[GameVariable.HasItemsList])
            {
                GamePresenter.GameModel.GameVariables[GameVariable.HasItemsList] = true;
            }
            
            var variable = (ItemVariable)LoadedNodeData.GameVariableValue;

            switch (LoadedNodeData.ItemActionType)
            {
                case ItemActionType.Add:
                {
                    GamePresenter.GameModel.ChangeGlobalVariable(LoadedNodeData.GameVariableType, LoadedNodeData.GameVariableValue, true);
                    GamePresenter.GameModel.Update();
                    
                    _mainMenuPresenter.FocusAndAnimateItem(variable, true, onComplete);
                    
                    break;
                }
                case ItemActionType.Remove:
                {
                    _mainMenuPresenter.FocusAndAnimateItem(variable, false, () =>
                    {
                        GamePresenter.GameModel.ChangeGlobalVariable(LoadedNodeData.GameVariableType, LoadedNodeData.GameVariableValue, false);
                        GamePresenter.GameModel.Update();
                                
                        onComplete?.Invoke();
                    });
                    
                    break;
                }
                case ItemActionType.Click:
                {
                    _mainMenuPresenter.FocusItem(variable, onComplete);
                    
                    break;
                }
            }
            
            GamePresenter.GameModel.InstantNextMove = true;
        }
    }
}