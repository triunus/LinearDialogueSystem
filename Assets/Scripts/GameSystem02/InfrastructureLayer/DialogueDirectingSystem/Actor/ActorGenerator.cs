using UnityEngine;

namespace GameSystems.InfrastructureLayer.DialogueDirectingSystem
{
    public class ActorGenerator
    {
        private IPrefabSO PrefabSO;

        private RuntimeActorDB runtimeActorDB;

        private string ActorSOPath = "ScriptableObject/DialogueDirectingSystem/ActorPrefabSO";

        public ActorGenerator(RuntimeActorDB runtimeActorDB)
        {
            this.runtimeActorDB = runtimeActorDB;

            this.LoadScriptableObject();
        }

        private void LoadScriptableObject()
        {
            this.PrefabSO = Resources.Load<ActorPrefabSO>(this.ActorSOPath);

            // 파일 못찾으면 false 리턴.
            if (this.PrefabSO == null)
                Debug.LogError($"ActorPrefabSO 불러오기 실패.");
        }

        public bool TryGenerate(string[] actorKeys)
        {
            if (this.PrefabSO == null || this.runtimeActorDB == null)
            {
                Debug.LogError($"PrefabSO 또는 UniqueObjectPrefabSO null 상태.");
                return false;
            }

            foreach (string key in actorKeys)
            {
                if (!this.PrefabSO.TryGetPrefabData(key, out var prefab))
                    Debug.LogError($"ActorPrefabSO key에 해당되는 Prefab이 등록되지 않음.");

                var newObject = MonoBehaviour.Instantiate(prefab);

                this.runtimeActorDB.RegisterActor(key, newObject);
            }

            return true;
        }
    }
}