using UnityEngine;

namespace GameSystems.InfrastructureLayer.DialogueDirectingSystem
{
    public class BackGroundGenerator
    {
        private IPrefabSO PrefabSO;

        private RuntimeBackGroundDB runtimeBackGroundDB;

        private string BackGroundSOPath = "ScriptableObject/DialogueDirectingSystem/BackGroundPrefabSO";

        public BackGroundGenerator(RuntimeBackGroundDB runtimeBackGroundDB)
        {
            this.runtimeBackGroundDB = runtimeBackGroundDB;

            this.LoadScriptableObject();
        }

        private void LoadScriptableObject()
        {
            this.PrefabSO = Resources.Load<BackGroundPrefabSO>(this.BackGroundSOPath);

            // 파일 못찾으면 false 리턴.
            if (this.PrefabSO == null)
                Debug.LogError($"BackGroundPrefabSO 불러오기 실패.");
        }

        public bool TryGenerate(string[] backGroundKeys)
        {
            if (this.PrefabSO == null || this.runtimeBackGroundDB == null)
            {
                Debug.LogError($"PrefabSO 또는 UniqueObjectPrefabSO null 상태.");
                return false;
            }

            foreach (string key in backGroundKeys)
            {
                if (!this.PrefabSO.TryGetPrefabData(key, out var prefab))
                    Debug.LogError($"BackGroundPrefabSO key에 해당되는 Prefab이 등록되지 않음.");

                var newObject = MonoBehaviour.Instantiate(prefab);

                this.runtimeBackGroundDB.RegisterBackGround(key, newObject);
            }

            return true;
        }
    }
}