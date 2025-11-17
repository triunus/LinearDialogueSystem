using UnityEngine;

namespace GameSystems.InfrastructureLayer.DialogueDirectingSystem
{
    public class BaseObjectGenerator
    {
        private IPrefabSO PrefabSO;

        private IRuntimeBaseObjectDB runtimeBaseObjectDB;

        private string BaseObjectPrefabSOPath = "ScriptableObjects/DialogueDirectingSystem/BaseObjectPrefabSO";

        public BaseObjectGenerator(IRuntimeBaseObjectDB runtimeBaseObjectDB)
        {
            this.runtimeBaseObjectDB = runtimeBaseObjectDB;

            this.LoadScriptableObject();
        }

        private void LoadScriptableObject()
        {
            this.PrefabSO = Resources.Load<BaseObjectPrefabSO>(this.BaseObjectPrefabSOPath);

            // 파일 못찾으면 false 리턴.
            if (this.PrefabSO == null)
                Debug.LogError($"BaseObjectPrefabSO 불러오기 실패.");
        }

        public bool TryGenerateAll()
        {
            if (this.PrefabSO == null || this.runtimeBaseObjectDB == null)
            {
                Debug.LogError($"PrefabSO 또는 BaseObjectPrefabSO null 상태.");
                return false;
            }

            foreach (var data in this.PrefabSO.GetAllPrefabDatas)
            {
                if(data.Key == "WorldObjectRoot")
                {
                    var worldObjectRoot = MonoBehaviour.Instantiate(data.Prefab);

                    this.runtimeBaseObjectDB.BaseWorldObject = worldObjectRoot;
                }
                else if(data.Key == "CanvasObjectRoot")
                {
                    var canvasObjectRoot = MonoBehaviour.Instantiate(data.Prefab);

                    this.runtimeBaseObjectDB.BaseWorldObject = canvasObjectRoot;
                }
                else
                {
                    Debug.LogError($"{data.Key}와 Prefab의 종류가 의도와 맞지 않은 부분이 있습니다.");
                    return false;
                }
            }

            return true;
        }
    }


}
