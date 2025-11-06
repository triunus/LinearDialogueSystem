using UnityEngine;

// SO
namespace GameSystems.DialogueDirectingService.Datas
{
    // 해당 Key에 대응되는 Prefab 데이터.
    [System.Serializable]
    public class DialogueDirectingPrefabData
    {
        [SerializeField] private string prefabKey;

        [SerializeField] private string prefabParentKey;
        [SerializeField] private GameObject prefab;

        public string PrefabKey { get => prefabKey; }

        public string PrefabParentKey { get => prefabParentKey; }
        public GameObject Prefab { get => prefab; }
    }
}