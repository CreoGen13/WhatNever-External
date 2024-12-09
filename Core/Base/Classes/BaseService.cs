using System;

namespace Core.Base.Classes
{
    public abstract class BaseService
    {
        public abstract void Initialize();
        public virtual void Update()
        {
            // throw new NotImplementedException();
        }
    }
}