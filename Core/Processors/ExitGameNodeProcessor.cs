using System;
using Core.Base.Classes;
using Core.Game;
using Core.Node.Panel;
using UI.MainMenu;

namespace Core.Processors
{
    public class ExitGameNodeProcessor : BaseNodeProcessor
    {
        private MainMenuPresenter _mainMenuPresenter;

        public ExitGameNodeProcessor(LoadedNodeData loadedNodeData, GamePresenter gamePresenter, MainMenuPresenter mainMenuPresenter)
            : base(loadedNodeData, gamePresenter)
        {
            _mainMenuPresenter = mainMenuPresenter;
        }

        public override void Activate(Action onComplete)
        {
            _mainMenuPresenter.ReturnToStartMenu();
        }
    }
}