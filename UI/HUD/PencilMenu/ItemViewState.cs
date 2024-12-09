using UnityEngine;

namespace UI.HUD.PencilMenu
{
    public struct ItemViewState
    {
        public readonly Sprite Sprite;
        public readonly Vector3 Position;

        public ItemViewState(Sprite sprite, Vector3 position)
        {
            Sprite = sprite;
            Position = position;
        }
    }
}