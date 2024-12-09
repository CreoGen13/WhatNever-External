using System;
using Core.Base.Classes;
using Core.Infrastructure.Enums;
using Core.Infrastructure.Enums.GameVariables;
using UnityEngine;

namespace Core.Node.Panel
{
    public class LoadedNodeData : BaseLoadedData
    {
        public NodeData NodeData { get; private set; }
        
        public SpriteTag SpriteTag => NodeData.spriteTag;
        public int BubbleSpriteNumber => NodeData.bubbleSpriteNumber;
        public int BubblePrefabNumber => NodeData.bubblePrefabNumber;
        public ChoiceButtonType ChoiceButtonType => NodeData.choiceButtonType;
        public Type GameVariableType { get; private set; }
        public int GameVariableValue => NodeData.gameVariableValue;
        public ItemActionType ItemActionType => NodeData.itemActionType;
        public DialogScreenActionType DialogScreenActionType => NodeData.dialogScreenActionType;
        public ArrivalType ArrivalType => NodeData.arrivalType;
        public Vector3 MovePosition => NodeData.movePosition;
        public NextNodeConnection NextNodeConnection => NodeData.nextNodeConnection;
        public bool BoolValue => NodeData.boolValue;
        public bool BoolValue2 => NodeData.boolValue2;
        public ClickEventType ClickEvent => NodeData.onClickEvent;
        public bool HasAnimation => NodeData.hasAnimation;
        public bool WaitForAnimationEnd => NodeData.animatorEvent;
        public bool StartWithDeferredEvent => NodeData.startWithDeferredEvent;
        public DeferredEventType DeferredEventType => NodeData.deferredEventType;
        public bool StopWithDeferredEvent => NodeData.stopWithDeferredEvent;
        public DeferredEventType StopDeferredEventType => NodeData.stopDeferredEventType;
        public bool HasAudio => NodeData.hasAudio;
        public Color Color => NodeData.color;
        public Stage Stage => NodeData.stage;
        public Vector3 SpritePosition => NodeData.spritePosition;
        public Vector3 ScaleAndRotation => NodeData.scaleAndRotation;
        public CharacterName Author => NodeData.author;
        
        public Type Type;
        public AudioClip AudioClip;
        public GameObject GameObject;
        public Sprite Sprite;
        public RuntimeAnimatorController AnimatorController;
        public string Text;
        
        public LoadedNodeData(NodeData nodeData)
        {
            NodeData = nodeData;

            if (!string.IsNullOrEmpty(nodeData.gameVariableType))
            {
                GameVariableType = typeof(GameVariable).Assembly.GetType(nodeData.gameVariableType);
            }
        }
    }
}