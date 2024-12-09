using System;
using Core.Base.Classes;
using Core.Game;
using Core.Node.Panel;
using UI.MainMenu;

namespace Core.Processors
{
    public class ChangePencilColorNodeProcessor : BaseNodeProcessor
    {
        private MainMenuPresenter _mainMenuPresenter;

        public ChangePencilColorNodeProcessor(LoadedNodeData loadedNodeData, GamePresenter gamePresenter, MainMenuPresenter mainMenuPresenter) 
            : base(loadedNodeData, gamePresenter)
        {
            _mainMenuPresenter = mainMenuPresenter;
        }

        public override void Activate(Action onComplete)
        {
            _mainMenuPresenter.ChangePencilColor(LoadedNodeData.Color);
            
            onComplete?.Invoke();
        }
    }
}