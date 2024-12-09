using System;
using Core.Base.Classes;
using Core.Game;
using Core.Infrastructure.Enums;
using Core.Node.Panel;
using UI.MainMenu;

namespace Core.Processors
{
    public class ChoiceNodeProcessor : BaseNodeProcessor
    {
        private MainMenuPresenter _mainMenuPresenter;

        public ChoiceNodeProcessor(LoadedNodeData loadedNodeData, GamePresenter gamePresenter, MainMenuPresenter mainMenuPresenter)
            : base(loadedNodeData, gamePresenter)
        {
            _mainMenuPresenter = mainMenuPresenter;
        }

        public override void Activate(Action onComplete)
        {
            var baseNodeName = GamePresenter.GameModel.CurrentNodeData.name;
            
            _mainMenuPresenter.SetButtonChoice(LoadedNodeData.ChoiceButtonType == ChoiceButtonType.Left, LoadedNodeData.Text, () =>
            {
                _mainMenuPresenter.MoveButtonsChoice(false, () =>
                {
                    if (GamePresenter.GameModel.LoadNextChoice(baseNodeName))
                    {
                        GamePresenter.GameModel.IsBlocked = false;
                        GamePresenter.GameModel.InstantNextMove = true;
                        GamePresenter.GameModel.Update();
                    }
                });
            });
            if (LoadedNodeData.BoolValue)
            {
                _mainMenuPresenter.MoveButtonsChoice(true, onComplete);
            }
            else
            {
                onComplete?.Invoke();
            }
        }
    }
}