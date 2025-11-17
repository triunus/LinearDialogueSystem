using UnityEngine;

using GameSystems.InfrastructureLayer.DialogueDirectingSystem;
using GameSystems.OutputLayer.DialogueDirectingSystem;

namespace GameSystems.DomainLayer.DialogueDirectingSystem
{
    public interface IObjectParentAggregate
    {
        public void UpdateBaseObjectParentAdapter(IRuntimeBaseObjectDB runtimeBaseObjectDB);

        public void SetParentWorldObject(string parentObjectKey, Transform childObjectTransform);
        public void SetParentCanvasObject(string parentObjectKey, RectTransform childObjectRectTransform);
    }

    public class ObjectParentAggregate : IObjectParentAggregate
    {
        private IBaseWorldObjectAdapter BaseWorldObjectAdapter;
        private IBaseCanvasObjectAdapter BaseCanvasObjectAdapter;

        public void UpdateBaseObjectParentAdapter(IRuntimeBaseObjectDB runtimeBaseObjectDB)
        {
            this.BaseWorldObjectAdapter = runtimeBaseObjectDB.BaseWorldObject.GetComponent<IBaseWorldObjectAdapter>();
            if (this.BaseWorldObjectAdapter == null)
                Debug.LogError($"UpdateBaseObjectParentAdapter - BaseWorldObjectAdapter 연결 에러");

            this.BaseCanvasObjectAdapter = runtimeBaseObjectDB.BaseCanvasObject.GetComponent<IBaseCanvasObjectAdapter>();
            if (this.BaseCanvasObjectAdapter == null)
                Debug.LogError($"UpdateBaseObjectParentAdapter - BaseCanvasObjectAdapter 연결 에러");
        }

        public void SetParentWorldObject(string parentObjectKey, Transform childObjectTransform)
            => this.BaseWorldObjectAdapter.SetParent(parentObjectKey, childObjectTransform);

        public void SetParentCanvasObject(string parentObjectKey, RectTransform childObjectRectTransform)
            => this.BaseCanvasObjectAdapter.SetParent(parentObjectKey, childObjectRectTransform);
    }
}
