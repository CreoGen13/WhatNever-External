using System;
using System.Linq;
using Core.Base.Classes;
using Core.Game;
using Core.Infrastructure.Enums;
using Core.Infrastructure.Services;
using Core.Node.Panel;

namespace Core.Processors
{
    public class DeleteAllTaggedNodeProcessor : BaseNodeProcessor
    {
        private readonly LoadService _loadService;

        public DeleteAllTaggedNodeProcessor(LoadedNodeData loadedNodeData, GamePresenter gamePresenter, LoadService loadService)
            : base(loadedNodeData, gamePresenter)
        {
            _loadService = loadService;
        }

        public override void Activate(Action onComplete)
        {
            var spritesPairs = GamePresenter.GameModel.GameSpritesDictionary.Where(spritePair => spritePair.Value.SpriteTag == LoadedNodeData.SpriteTag).ToList();

            if (spritesPairs.Count == 0)
            {
                onComplete?.Invoke();
                
                return;
            }
            
            for (int i = 0; i < spritesPairs.Count; i++)
            {
                GamePresenter.GameModel.GameSpritesDictionary.Remove(spritesPairs[i].Key);
                
                if (i == spritesPairs.Count - 1)
                {
                    spritesPairs[i].Value.DisableAndReturn(LoadedNodeData.ArrivalType == ArrivalType.ShowUp,
                        LoadedNodeData.NextNodeConnection == NextNodeConnection.Solo,
                        onComplete);
                }
                else
                {
                    spritesPairs[i].Value.DisableAndReturn(LoadedNodeData.ArrivalType == ArrivalType.ShowUp);
                }
                
                _loadService.UnloadNodeData(spritesPairs[i].Key);
            }
        }
    }
}