
using GameSystems.InfrastructureLayer.DialogueDirectingSystem;
using GameSystems.OutputLayer.DialogueDirectingSystem;

namespace GameSystems.DomainLayer.DialogueDirectingSystem
{
    public interface IUniqueObjectAggregate
    {
        public void UpdateUniqueObjectAdapter(RuntimeUniqueObjectDB runtimeUniqueObjectDB);

        public ICutSceneAdapter CutSceneAdapter { get; }
        public IFixedBottomTextUIPerformerAdapter FixedBottomTextUIPerformerAdapter { get; }
    }

    public class UniqueObjectAggregate : IUniqueObjectAggregate
    {
        private ICutSceneAdapter _CutSceneAdapter;
        private IFixedBottomTextUIPerformerAdapter _FixedBottomTextUIPerformerAdapter;

        public void UpdateUniqueObjectAdapter(RuntimeUniqueObjectDB runtimeUniqueObjectDB)
        {
            foreach(var uniqueObject in runtimeUniqueObjectDB.GetAllUniqueObjects())
            {
                if (uniqueObject.TryGetComponent<ICutSceneAdapter>(out var cutSceneAdapter))
                {
                    this._CutSceneAdapter = cutSceneAdapter;
                }
                else if (uniqueObject.TryGetComponent<IFixedBottomTextUIPerformerAdapter>(out var fixedBottomTextUIPerformerAdapter))
                {
                    this._FixedBottomTextUIPerformerAdapter = fixedBottomTextUIPerformerAdapter;
                }
            }
        }

        public ICutSceneAdapter CutSceneAdapter => this._CutSceneAdapter;
        public IFixedBottomTextUIPerformerAdapter FixedBottomTextUIPerformerAdapter => this._FixedBottomTextUIPerformerAdapter;
    }
}