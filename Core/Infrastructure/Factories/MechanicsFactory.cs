using Core.Base.Classes;
using Core.Base.Interfaces.Factory;
using UnityEngine;
using Zenject;

namespace Core.Infrastructure.Factories
{
    public class MechanicsFactory : IBaseGenericDataFactory<BaseMechanics, MechanicsFactory.Data>
    {
        public class Data : BaseLoadedData
        {
            public GameObject Prefab;
            public Transform Parent;
        }
        
        private readonly DiContainer _container;

        [Inject]
        public MechanicsFactory(DiContainer container)
        {
            _container = container;
        } 

        public T Create<T>(Data data) where T : BaseMechanics
        {
            var resolvedMechanics = _container.InstantiatePrefabForComponent<T>(data.Prefab, data.Parent);
            return resolvedMechanics;
        }
    }
}