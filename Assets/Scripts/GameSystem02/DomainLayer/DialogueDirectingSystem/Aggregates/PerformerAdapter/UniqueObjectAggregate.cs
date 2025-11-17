
using GameSystems.InfrastructureLayer.DialogueDirectingSystem;
using GameSystems.OutputLayer.DialogueDirectingSystem;

namespace GameSystems.DomainLayer.DialogueDirectingSystem
{
    public interface IUniqueObjectAggregate
    {
        public bool TrySetCutSceneAdapter(IRuntimeUniqueObjectDB runtimeUniqueObjectDB);
        public bool TrySetFixedBottomTextUIAdapter(IRuntimeUniqueObjectDB runtimeUniqueObjectDB);

        public ICutSceneAdapter CutSceneAdapter { get; }
        public IFixedBottomTextUIPerformerAdapter FixedBottomTextUIPerformerAdapter { get; }
    }

    public class UniqueObjectAggregate : IUniqueObjectAggregate
    {
        private ICutSceneAdapter _CutSceneAdapter;
        private IFixedBottomTextUIPerformerAdapter _FixedBottomTextUIPerformerAdapter;

        public bool TrySetCutSceneAdapter(IRuntimeUniqueObjectDB runtimeUniqueObjectDB)
        {
            if (runtimeUniqueObjectDB.CutSceneObject.TryGetComponent<ICutSceneAdapter>(out var cutSceneAdapter))
            {
                this._CutSceneAdapter = cutSceneAdapter;
                return true;
            }

            return false;
        }
        public bool TrySetFixedBottomTextUIAdapter(IRuntimeUniqueObjectDB runtimeUniqueObjectDB)
        {
            if (runtimeUniqueObjectDB.FixedBottomTextUIObject.TryGetComponent<IFixedBottomTextUIPerformerAdapter>(out var fixedBottomTextUIPerformerAdapter))
            {
                this._FixedBottomTextUIPerformerAdapter = fixedBottomTextUIPerformerAdapter;
                return true;
            }

            return false;
        }

        public ICutSceneAdapter CutSceneAdapter => this._CutSceneAdapter;
        public IFixedBottomTextUIPerformerAdapter FixedBottomTextUIPerformerAdapter => this._FixedBottomTextUIPerformerAdapter;
    }
}