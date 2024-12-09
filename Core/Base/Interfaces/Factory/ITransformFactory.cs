using UnityEngine;

namespace Core.Base.Interfaces.Factory
{
    public interface ITransformFactory<out T1> : IBaseFactory<T1, Transform>
    {
        new T1 Create(Transform parent);
    }
}