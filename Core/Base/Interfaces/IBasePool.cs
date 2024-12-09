namespace Core.Base.Interfaces
{
    public interface IBasePool<in T>
    {
        public void Return(T poolMember);
        public void Clear();
    }
}