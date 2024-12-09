using Core.Base.Classes;

namespace Core.Base.Interfaces.Factory
{
    public interface IBaseGenericDataFactory<in T1, in T2> : IBaseGenericFactory<T1, T2>
        where T2 : BaseLoadedData
    {
        new T Create<T>(T2 data) where T : T1;
    }
}