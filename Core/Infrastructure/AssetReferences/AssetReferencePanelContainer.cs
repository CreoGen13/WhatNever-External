using Core.Node.Panel;
using UnityEngine.AddressableAssets;

namespace Core.Infrastructure.AssetReferences
{
    [System.Serializable]
    public class AssetReferencePanelContainer : AssetReferenceT<PanelContainer> 
    {
        public AssetReferencePanelContainer(string guid) : base(guid) { }
    }
}