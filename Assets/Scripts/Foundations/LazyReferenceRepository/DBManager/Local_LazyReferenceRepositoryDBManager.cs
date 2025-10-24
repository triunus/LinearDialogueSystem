using Foundations.Patterns.Singleton;

using GameSystems.Entities;
using GameSystems.GameFlows;
using GameSystems.InputControllers;

namespace Foundations.LazyReferenceRepository
{
    public class Local_LazyReferenceRepositoryDBManager<T> : LocalSingleton<T> where T : Local_LazyReferenceRepositoryDBManager<T>
    {
        private ILazyReferenceRepository<IEntity> _Entity_LazyReferenceRepository;
        private ILazyReferenceRepository<IGameFlow> _GameFlow_LazyReferenceRepository;

        private ILazyReferenceRepository<IInputController> _InputController_LazyReferenceRepository;

        private void Awake()
        {
            base.Awake();

            this._Entity_LazyReferenceRepository = new LazyReferenceRepository<IEntity>();
            this._GameFlow_LazyReferenceRepository = new LazyReferenceRepository<IGameFlow>();
            this._InputController_LazyReferenceRepository = new LazyReferenceRepository<IInputController>(); ;
        }

        public ILazyReferenceRepository<IEntity> Entity_LazyReferenceRepository { get => _Entity_LazyReferenceRepository; }
        public ILazyReferenceRepository<IGameFlow> GameFlow_LazyReferenceRepository { get => _GameFlow_LazyReferenceRepository; }
        public ILazyReferenceRepository<IInputController> InputController_LazyReferenceRepository { get => _InputController_LazyReferenceRepository; }
    }
}

namespace GameSystems.Entities { public interface IEntity { } }
namespace GameSystems.GameFlows { public interface IGameFlow { } }
namespace GameSystems.InputControllers { public interface IInputController { } }