using Core.Base.Interfaces.Factory;
using UI.HUD.BacklogMenu;
using UnityEngine;
using Zenject;

namespace Core.Infrastructure.Factories
{
    public class SentenceFactory : ITransformFactory<SentenceMono>
    {
        private readonly DiContainer _container;
        private readonly GameObject _prefab;

        [Inject]
        public SentenceFactory(GameObject prefab, DiContainer container)
        {
            _container = container;
            _prefab = prefab;
        }

        public SentenceMono Create(Transform parent)
        {
            var sentence = _container.InstantiatePrefabForComponent<SentenceMono>(_prefab, parent);
            return sentence;
        }
    }
}