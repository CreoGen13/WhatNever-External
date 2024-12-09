using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.Infrastructure.AssetReferences
{
    [System.Serializable]
    public class AssetReferenceAnimatorController : AssetReferenceT<RuntimeAnimatorController> 
    {
        public AssetReferenceAnimatorController(string guid) : base(guid) { }
    }
}