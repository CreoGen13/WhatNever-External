namespace Core.Base.Interfaces
{
    public interface IBaseGenericPool <T> : IBaseInitializablePool<T>
    {
        public T Spawn();
    }
}