using System;
using Core.Base.Classes;
using Core.Game;
using Core.Infrastructure.Enums;
using Core.Mono;
using Core.Node.Panel;

namespace Core.Processors
{
    public class TransitionNodeProcessor : BaseNodeProcessor
    {
        public TransitionNodeProcessor(LoadedNodeData loadedNodeData, GamePresenter gamePresenter)
            : base(loadedNodeData, gamePresenter)
        {
        }

        public override void Activate(Action onComplete)
        {
            var isBlockedBefore = GamePresenter.GameModel.IsBlocked;
                
            GamePresenter.GameModel.IsBlocked = true;
            GamePresenter.GameModel.Update();

            if (LoadedNodeData.BoolValue)
            {
                var transition = GamePresenter.GameView.SpawnMono<Transition>(LoadedNodeData.GameObject, Stage.Transition);
                GamePresenter.GameView.PlayTransition(transition, () =>
                {
                    GamePresenter.GameModel.IsBlocked = isBlockedBefore;
                    GamePresenter.GameModel.Update();
                
                    onComplete?.Invoke();
                });
            }
            else
            {
                GamePresenter.GameView.StopTransition(() =>
                {
                    GamePresenter.GameModel.IsBlocked = isBlockedBefore;
                    GamePresenter.GameModel.Update();
                
                    onComplete?.Invoke();
                });
            }
        }
    }
}