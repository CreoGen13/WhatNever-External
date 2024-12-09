using System;
using Core.Infrastructure.AssetReferences;
using Core.Infrastructure.Enums;
using Core.Infrastructure.Enums.GameVariables;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Serialization;
using AssetReferenceSprite = Core.Infrastructure.AssetReferences.AssetReferenceSprite;

namespace Core.Node.Panel
{
    [Serializable]
    public class NodeData
    {
        public string name;
        public Vector2 position;
        
        public string nodeType;
        public SpriteTag spriteTag;
        public int bubbleSpriteNumber;
        public int bubblePrefabNumber;
        [FormerlySerializedAs("choiceButton")] public ChoiceButtonType choiceButtonType;
        [FormerlySerializedAs("dialogScreenAction")] public DialogScreenActionType dialogScreenActionType;
        public string gameVariableType;
        public int gameVariableValue;
        public ItemActionType itemActionType;
        
        public ArrivalType arrivalType;
        public Vector3 movePosition;
        public NextNodeConnection nextNodeConnection;
        public bool boolValue;
        public bool boolValue2;
        public ClickEventType onClickEvent;
        public bool hasAnimation;
        public AssetReferenceAnimatorController animatorController;
        public bool animatorEvent;
        public bool startWithDeferredEvent;
        [FormerlySerializedAs("deferredEvent")] public DeferredEventType deferredEventType;
        public bool stopWithDeferredEvent;
        [FormerlySerializedAs("stopDeferredEvent")] public DeferredEventType stopDeferredEventType;
        public bool hasAudio;
        public AssetReferenceAudioClip audioClip;
        
        public AssetReferenceGameObject gameObject;
        public AssetReferenceSprite sprite;
        public Color color;
        public Stage stage;
        public Vector3 spritePosition;
        public Vector3 scaleAndRotation;

        public CharacterName author;
        public LocalizedString localizedText;   
    }
}
