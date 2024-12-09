using Core.Base.Classes;
using Core.Base.Interfaces.Factory;
using Core.Node.Panel;
using Zenject;

namespace Core.Infrastructure.Factories
{
    public class NodeFactory : IDataFactory<BaseNodeProcessor, LoadedNodeData>
    {
        private readonly DiContainer _container;

        [Inject]
        public NodeFactory(DiContainer container)
        {
            _container = container;
        }

        public BaseNodeProcessor Create(LoadedNodeData nodeData)
        {
            var resolvedNode = _container.Instantiate(nodeData.Type, new object[]{nodeData});
            return (BaseNodeProcessor) resolvedNode;
        }
    }
}