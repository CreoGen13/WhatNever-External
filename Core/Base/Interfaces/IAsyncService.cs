using Cysharp.Threading.Tasks;

namespace Core.Base.Interfaces
{
    public interface IAsyncService
    {
        public UniTask Initialize();
    }
}