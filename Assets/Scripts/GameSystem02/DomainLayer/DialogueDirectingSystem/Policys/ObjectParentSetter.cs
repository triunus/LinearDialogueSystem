using UnityEngine;

using GameSystems.InfrastructureLayer.DialogueDirectingSystem;

namespace GameSystems.DomainLayer.DialogueDirectingSystem
{
    public class ObjectParentSetter
    {
        private string actorParentKey = "ActorParentRoot";
        private string backGroundParentKey = "BackGroundParentRoot";
        private string cutSceneParentKey = "CutSceneParentRoot";
        private string fixedBottomTextUIParentKey = "FixedBottomTextUIParentRoot";

        public void SetActorParentObject(IObjectParentAggregate objectParentAggregate, IRuntimeActorDB runtimeActorDB)
        {
            foreach(var gameObject in runtimeActorDB.GetAllActors())
            {
                objectParentAggregate.SetParentWorldObject(this.actorParentKey, gameObject.transform);
            }
        }

        public void SetBackGroundParentObject(IObjectParentAggregate objectParentAggregate, IRuntimeBackGroundDB runtimeBackGroundDB)
        {
            foreach (var gameObject in runtimeBackGroundDB.GetAllBackGrounds())
            {
                objectParentAggregate.SetParentWorldObject(this.backGroundParentKey, gameObject.transform);
            }
        }

        public void SetCutSceneParentObject(IObjectParentAggregate objectParentAggregate, IRuntimeUniqueObjectDB runtimeUniqueObjectDB)
        {
            objectParentAggregate.SetParentCanvasObject(this.cutSceneParentKey, runtimeUniqueObjectDB.CutSceneObject.GetComponent<RectTransform>());
        }

        public void SetFixedBottomTextUIParentObject(IObjectParentAggregate objectParentAggregate, IRuntimeUniqueObjectDB runtimeUniqueObjectDB)
        {
            objectParentAggregate.SetParentCanvasObject(this.fixedBottomTextUIParentKey, runtimeUniqueObjectDB.FixedBottomTextUIObject.GetComponent<RectTransform>());
        }
    }
}
