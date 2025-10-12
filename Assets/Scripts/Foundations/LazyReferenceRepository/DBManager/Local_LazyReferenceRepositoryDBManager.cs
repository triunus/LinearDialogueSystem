using Foundations.Patterns.Singleton;

using GameSystems.Entities;
using GameSystems.GameFlows;

namespace Foundations.LazyReferenceRepository
{
    public class Local_LazyReferenceRepositoryDBManager<T> : LocalSingleton<T> where T : Local_LazyReferenceRepositoryDBManager<T>
    {
        private ILazyReferenceRepository<IEntity> _Entity_LazyReferenceRepository;
        private ILazyReferenceRepository<IGameFlow> _GameFlow_LazyReferenceRepository;

        private void Awake()
        {
            base.Awake();

            this._Entity_LazyReferenceRepository = new LazyReferenceRepository<IEntity>();
            this._GameFlow_LazyReferenceRepository = new LazyReferenceRepository<IGameFlow>();
        }

        public ILazyReferenceRepository<IEntity> Entity_LazyReferenceRepository { get => _Entity_LazyReferenceRepository; }
        public ILazyReferenceRepository<IGameFlow> GameFlow_LazyReferenceRepository { get => _GameFlow_LazyReferenceRepository; }
    }
}

namespace GameSystems.Entities { public interface IEntity { } }
namespace GameSystems.GameFlows { public interface IGameFlow { } }