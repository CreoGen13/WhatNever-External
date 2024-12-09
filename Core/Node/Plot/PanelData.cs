using System;
using Core.Infrastructure.AssetReferences;
using Core.Infrastructure.Enums;
using Core.Infrastructure.Enums.GameVariables;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.Node.Plot
{
    [Serializable]
    public class PanelData
    {
        public string id;
        public string name;
        public Vector2 position;
        
        public AssetReferencePanelContainer panelContainer;
        public PanelType panelType;
        public bool deletePreviousPanel;
        public bool hasCondition;
        public GameVariable plotCondition;
        public bool hasPrefab;
        public AssetReferenceGameObject prefab;
        public bool hasAudio;
        public AssetReferenceAudioClip audio;
    }
}