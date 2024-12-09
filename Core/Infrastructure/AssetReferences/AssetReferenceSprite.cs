using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.Infrastructure.AssetReferences
{
    [System.Serializable]
    public class AssetReferenceSprite : AssetReferenceT<Sprite> 
    {
        public AssetReferenceSprite(string guid) : base(guid) { }
    }
}