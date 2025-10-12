using Foundations.Patterns.Singleton;

using GameSystems.Models;
using GameSystems.PlainServices;
using GameSystems.UnityServices;

namespace Foundations.LazyReferenceRepository
{
    public class Global_LazyReferenceRepositoryDBManager<T> : GlobalSingleton<T> where T : Global_LazyReferenceRepositoryDBManager<T>
    {
        private ILazyReferenceRepository<IModel> _Model_LazyReferenceRepository;
        private ILazyReferenceRepository<IPlainService> _PlainServices_LazyReferenceRepository;
        private ILazyReferenceRepository<IUnityService> _UnityService_LazyReferenceRepository;

        private void Awake()
        {
            base.Awake();

            this._Model_LazyReferenceRepository = new LazyReferenceRepository<IModel>();
            this._PlainServices_LazyReferenceRepository = new LazyReferenceRepository<IPlainService>();
            this._UnityService_LazyReferenceRepository = new LazyReferenceRepository<IUnityService>();
        }

        public ILazyReferenceRepository<IModel> Model_LazyReferenceRepository { get => _Model_LazyReferenceRepository; }
        public ILazyReferenceRepository<IPlainService> PlainServices_LazyReferenceRepository { get => _PlainServices_LazyReferenceRepository; }
        public ILazyReferenceRepository<IUnityService> UnityService_LazyReferenceRepository { get => _UnityService_LazyReferenceRepository; }
    }
}

namespace GameSystems.Models { public interface IModel { } }
namespace GameSystems.PlainServices { public interface IPlainService { } }
namespace GameSystems.UnityServices { public interface IUnityService { } }