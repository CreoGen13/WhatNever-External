using Core.Base.Interfaces.Factory;
using Core.Sprites.GameSprite;
using UnityEngine;
using Zenject;

namespace Core.Infrastructure.Factories
{
    public class SpriteFactory : ITransformFactory<GameSpritePresenter>
    {
        private readonly DiContainer _container;
        private readonly GameObject _prefab;

        [Inject]
        public SpriteFactory(GameObject prefab, DiContainer container)
        {
            _container = container;
            _prefab = prefab;
        }

        public GameSpritePresenter Create(Transform parent)
        {
            var view = _container.InstantiatePrefabForComponent<GameSpriteView>(_prefab, parent);
            var sprite = _container.Instantiate<GameSpritePresenter>(new object[]{new GameSpriteModel(), view});
            
            return sprite;
        }
    }
}