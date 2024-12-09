using Core.Base.Classes;
using Core.Base.Interfaces.Factory;
using UnityEngine;
using Zenject;

namespace Core.Infrastructure.Factories
{
    public class MonoFactory : IBaseGenericDataFactory<MonoBehaviour, MonoFactory.Data>
    {
        public class Data : BaseLoadedData
        {
            public GameObject Prefab;
            public Transform Parent;
        }

        private readonly DiContainer _container;

        [Inject]
        public MonoFactory(DiContainer container)
        {
            _container = container;
        }

        public T Create<T>(Data data) where T : MonoBehaviour
        {
            var resolvedMechanics = _container.InstantiatePrefabForComponent<T>(data.Prefab, data.Parent);
            return resolvedMechanics;
        }
    }
}