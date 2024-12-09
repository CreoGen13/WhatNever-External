namespace Core.Base.Interfaces.Factory
{
    public interface IBaseGenericFactory<in T1, in T2>
    {
        T Create<T>(T2 arg) where T : T1;
    }
}