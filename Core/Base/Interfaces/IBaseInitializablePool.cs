namespace Core.Base.Interfaces
{
    public interface IBaseInitializablePool<in T> : IBasePool<T>
    {
        public void Initialize(int count);
    }
}