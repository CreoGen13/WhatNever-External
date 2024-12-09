using System.Collections.Generic;
using Core.Sprites.GameBubble;
using UnityEngine;

namespace Scriptables.Holders
{
    [CreateAssetMenu(fileName = "Bubbles Sprite Holder", menuName = "Holders/Bubble Sprite Holder", order = 0)]
    public class ScriptableBubblesSpriteHolder: ScriptableObject
    {
        public List<GameBubbleView> bubblePrefabs;
        public List<Sprite> bubbleSprites;
        public AudioClip bubbleSound;
    }
}