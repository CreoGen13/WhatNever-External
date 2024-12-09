using System;
using Core.Base.Classes;
using Core.Game;
using Core.Infrastructure.Enums.GameVariables;
using Core.Node.Panel;
using UI.MainMenu;

namespace Core.Processors
{
    public class ChangeCharacterNodeProcessor : BaseNodeProcessor
    {
        private readonly MainMenuPresenter _mainMenuPresenter;

        public ChangeCharacterNodeProcessor(LoadedNodeData loadedNodeData, GamePresenter gamePresenter, MainMenuPresenter mainMenuPresenter)
            : base(loadedNodeData, gamePresenter)
        {
            _mainMenuPresenter = mainMenuPresenter;
        }

        public override void Activate(Action onComplete)
        {
            if (!GamePresenter.GameModel.GameVariables[GameVariable.HasCharactersList])
            {
                GamePresenter.GameModel.GameVariables[GameVariable.HasCharactersList] = true;
            }
            
            _mainMenuPresenter.ChangeCharacter(LoadedNodeData.Author, LoadedNodeData.BubbleSpriteNumber - 1, onComplete);
        }
    }
}