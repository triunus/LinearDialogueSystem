using UnityEngine;

// SO
namespace GameSystems.DialogueDirectingService.Datas
{
    // 해당 Key에 대응되는 Prefab 데이터.
    [System.Serializable]
    public class DialogueDirectingPrefabData
    {
        [SerializeField] private string prefabKey;

        [SerializeField] private Transform prefabParent;
        [SerializeField] private GameObject prefab;

        public string PrefabKey { get => prefabKey; }

        public Transform PrefabParent { get => prefabParent; }
        public GameObject Prefab { get => prefab; }
    }
}