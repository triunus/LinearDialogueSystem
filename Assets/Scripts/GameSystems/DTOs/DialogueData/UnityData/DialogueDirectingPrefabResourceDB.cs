/*using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.DTOs
{
    public interface IDialogueDirectingPrefabResourceDB
    {
        public bool TryGetPrefabData(string prefabKey, out DialogueDirectingPrefabData prefabData);
    }

    [System.Serializable]
    public class DialogueDirectingPrefabResourceDB : IDialogueDirectingPrefabResourceDB
    {
        [SerializeField] private List<DialogueDirectingPrefabData> DialogueDirectingPrefabDatas;

        public bool TryGetPrefabData(string prefabKey, out DialogueDirectingPrefabData prefabData)
        {
            prefabData = null;

            foreach (var data in this.DialogueDirectingPrefabDatas)
            {
                if(data.PrefabKey == prefabKey)
                {
                    prefabData = data;
                    return true;
                }
            }

            return false;
        }
    }

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
}*/