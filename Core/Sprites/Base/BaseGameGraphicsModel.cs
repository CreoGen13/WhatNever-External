using Core.Base.Classes;
using Core.Infrastructure.Enums;
using UniRx;

namespace Core.Sprites.Base
{
    public abstract class BaseGameGraphicsModel<TModel> : BaseModel<TModel>
        where TModel : BaseGameGraphicsModel<TModel>
    {
        public bool IsSpawned;
        public bool IsActive;
        
        public ArrivalType ArrivalType;
    }
}