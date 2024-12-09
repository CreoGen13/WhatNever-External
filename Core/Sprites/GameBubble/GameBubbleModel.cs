using Core.Base.Classes;
using Core.Infrastructure.Enums;
using Core.Sprites.Base;
using UniRx;

namespace Core.Sprites.GameBubble
{
    public class GameBubbleModel : BaseGameGraphicsModel<GameBubbleModel>
    {
        public GameBubbleModel()
        {
            Subject = new BehaviorSubject<GameBubbleModel>(this);
        }
        public override void Update()
        {
            Subject.OnNext(this);
        }
    }
}