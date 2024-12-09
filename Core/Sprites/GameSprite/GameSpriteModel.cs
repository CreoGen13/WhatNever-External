using Core.Infrastructure.Enums;
using Core.Sprites.Base;
using UniRx;
using UnityEngine;

namespace Core.Sprites.GameSprite
{
    public class GameSpriteModel : BaseGameGraphicsModel<GameSpriteModel>
    {
        public SpriteTag SpriteTag;
        public Vector3 EndPosition;
        
        public bool HasButton;
        public bool HasAnimation;
        public bool HasAudio;
        
        public bool WaitForAnimationEnd;
        public bool IsDeferred;
        public ClickEventType ClickEventType;

        public GameSpriteModel()
        {
            Subject = new BehaviorSubject<GameSpriteModel>(this);
        }
        public override void Update()
        {
            Subject.OnNext(this);
        }
    }
}