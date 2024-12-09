using System;
using Core.Infrastructure.Enums;
using UnityEngine;

namespace Core.Sprites.Base
{
    public interface IGraphicsPresenter
    {
        public SpriteTag SpriteTag { get; }
        public Transform Transform { get; }

        public void EnableSprite();
        public void DisableAndReturn(bool withAnimation, bool waitForAnimationEnd = false, Action onComplete = null);
        public void SetTransparency(float alpha);
    }
}