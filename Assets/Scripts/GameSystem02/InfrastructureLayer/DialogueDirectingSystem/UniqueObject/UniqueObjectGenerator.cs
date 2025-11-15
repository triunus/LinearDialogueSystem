using UnityEngine;

namespace GameSystems.InfrastructureLayer.DialogueDirectingSystem
{
    public class UniqueObjectGenerator
    {
        private IPrefabSO PrefabSO;

        private RuntimeUniqueObjectDB runtimeUniqueObjectDB;

        private string uniqueObjectSOPath = "ScriptableObject/DialogueDirectingSystem/UniqueObjectPrefabSO";

        public UniqueObjectGenerator(RuntimeUniqueObjectDB runtimeUniqueObjectDB)
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

            foreach (string key in uniqueKeys)
            {
                if(!this.PrefabSO.TryGetPrefabData(key, out var prefab))
                    Debug.LogError($"UniqueObjectPrefabSO에 key에 해당되는 Prefab이 등록되지 않음.");

                var newObject = MonoBehaviour.Instantiate(prefab);

                this.runtimeUniqueObjectDB.RegisterUniqueObject(key, newObject);
            }

            return true;
        }
    }
}