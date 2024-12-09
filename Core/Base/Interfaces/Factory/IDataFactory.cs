using Core.Base.Classes;

namespace Core.Base.Interfaces.Factory
{
    public interface IDataFactory<out T1, in T2> : IBaseFactory<T1, T2>
        where T2 : BaseLoadedData
    {
        new T1 Create(T2 data);
    }
}