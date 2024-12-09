using Core.Sprites.GameBubble;
using UnityEngine;
using Zenject;

namespace Core.Infrastructure.Factories
{
    public class BubbleFactory
    {
        private readonly DiContainer _container;

        [Inject]
        public BubbleFactory(DiContainer container)
        {
            _container = container;
        }

        public GameBubblePresenter Create(GameBubbleView prefab, Transform parent)
        {
            var view = _container.InstantiatePrefabForComponent<GameBubbleView>(prefab, parent);
            var bubble = _container.Instantiate<GameBubblePresenter>(new object[]{new GameBubbleModel(), view});
            bubble.PrefabName = prefab.gameObject.name;
            
            return bubble;
        }
    }
}