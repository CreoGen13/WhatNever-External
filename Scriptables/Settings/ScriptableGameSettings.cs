using UnityEngine;
using UnityEngine.Serialization;

namespace Scriptables.Settings
{
    [CreateAssetMenu(fileName = "Game Settings", menuName = "Scriptables/Game Settings", order = 1)]
    public class ScriptableGameSettings : ScriptableObject
    {
        [Header("View Settings")]
        public float cameraDefaultSize;
        public float cameraAnimationDuration;
        public float scrollingSpeed;
        public float scrollAdditionalScale;
        
        [Header("Zoom settings")]
        public float zoomSpritesTransparencyStartDistance;
        public float zoomSpritesTransparencyEndDistance;
        
        [Header("Bubble Settings")]
        public float bubbleShowUpDuration;
        public float bubbleDeleteDuration;
        public Vector2 bubbleDefaultSize;
        
        [Header("Bubble Dialog Settings")]
        public float dialogBubbleOffset;
        public float dialogBubbleMaxWidth;
        public float dialogBubbleBackgroundWidth;
        public float dialogBubbleSidesWidth;
        
        [Header("Sprite Settings")]
        public float dialogScreenArrivalMoveDuration;
        public float arrivalTypeMovingDuration;
        
        [Header("Choice Settings")]
        public float choiceMoveDuration;

        [Header("Bush Settings")]
        public float bushFadingDuration;
        
        [Header("Annoy Mechanic Settings")]
        public float annoySliderDuration;

        [HideInInspector] public int containerNumber;
        [HideInInspector] public string panelName;
    }
}