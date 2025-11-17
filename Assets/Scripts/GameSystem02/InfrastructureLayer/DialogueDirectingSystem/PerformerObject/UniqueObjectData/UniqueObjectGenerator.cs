using UnityEngine;

namespace GameSystems.InfrastructureLayer.DialogueDirectingSystem
{
    public interface IUniqueObjectGenerator
    {
        public bool TryGenerate(string[] uniqueKeys);
    }

    public class UniqueObjectGenerator : IUniqueObjectGenerator
    {
        private IPrefabSO PrefabSO;

        private IRuntimeUniqueObjectDB runtimeUniqueObjectDB;

        private string uniqueObjectSOPath = "ScriptableObjects/DialogueDirectingSystem/UniqueObjectPrefabSO";

        public UniqueObjectGenerator(IRuntimeUniqueObjectDB runtimeUniqueObjectDB)
        {
            this.runtimeUniqueObjectDB = runtimeUniqueObjectDB;

            this.LoadScriptableObject();
        }

        private void LoadScriptableObject()
        {
            this.PrefabSO = Resources.Load<UniqueObjectPrefabSO>(this.uniqueObjectSOPath);

            // 파일 못찾으면 false 리턴.
            if (this.PrefabSO == null)
                Debug.LogError($"UniqueObjectPrefabSO 불러오기 실패.");
        }

        public bool TryGenerate(string[] uniqueKeys)
        {
            if(this.PrefabSO == null || this.runtimeUniqueObjectDB == null)
            {
                Debug.LogError($"PrefabSO 또는 UniqueObjectPrefabSO null 상태.");
                return false;
            }

            foreach (var data in this.PrefabSO.GetAllPrefabDatas)
            {
                if (data.Key == "CutScene")
                {
                    var cutSceneObject = MonoBehaviour.Instantiate(data.Prefab);

                    this.runtimeUniqueObjectDB.CutSceneObject = cutSceneObject;
                }
                else if (data.Key == "FixedBottomTextUI")
                {
                    var fixedBottomTextUIObject = MonoBehaviour.Instantiate(data.Prefab);

                    this.runtimeUniqueObjectDB.FixedBottomTextUIObject = fixedBottomTextUIObject;
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