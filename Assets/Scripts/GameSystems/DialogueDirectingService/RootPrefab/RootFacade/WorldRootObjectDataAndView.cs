using System.Collections.Generic;
using UnityEngine;

using GameSystems.DialogueDirectingService.Datas;

namespace GameSystems.DialogueDirectingService.Views
{
    public class WorldRootObjectDataAndView : MonoBehaviour, IParentGameObjectDataHandler
    {
        [SerializeField] private string _ParentGameObjectDataHandlerKey;
        // 부모 GameObject 목록.
        [SerializeField] private List<IParentGameObjectData> ParentGameObjects;

        private void Awake()
        {
            this.ParentGameObjects = new();

            IParentGameObjectData[] parentObjects = GetComponentsInChildren<IParentGameObjectData>(includeInactive: true);
            // 리스트에 등록
            foreach (var data in parentObjects)
            {
                this.ParentGameObjects.Add(data);
            }
        }
        public IEnumerable<IParentGameObjectData> ParentGameObjectDataList => this.ParentGameObjects;

        public string ParentGameObjectDataHandlerKey => this._ParentGameObjectDataHandlerKey;
    }
}