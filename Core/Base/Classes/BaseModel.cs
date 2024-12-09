using System;
using UniRx;

namespace Core.Base.Classes
{
    public abstract class BaseModel<TModel>
        where TModel : BaseModel<TModel>
    {
        protected BehaviorSubject<TModel> Subject;

        public IObservable<TModel> Observe()
        {
            return Subject.AsObservable();
        }

        public abstract void Update();
    }
}