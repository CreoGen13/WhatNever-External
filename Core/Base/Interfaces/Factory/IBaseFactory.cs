namespace Core.Base.Interfaces.Factory
{
    public interface IBaseFactory<out T1, in T2>
    {
        T1 Create(T2 arg);
    }
}