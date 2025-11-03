using System.Collections.Generic;
using UnityEngine;

// SO
namespace GameSystems.DialogueDirectingService.Datas
{
    public interface IDialogueDirectingPrefabDataSO
    {
        public bool TryGetPrefabData(string prefabKey, out DialogueDirectingPrefabData prefabData);
    }

    // 연출에 사용될 수 있는 모든 Prefab들을 모아놓은 SO파일.
    [SerializeField]
    [CreateAssetMenu(menuName = "DialogueDirectingService/PrefabDataSO", fileName = "PrefabDataSO")]
    public class DialogueDirectingPrefabDataSO : ScriptableObject, IDialogueDirectingPrefabDataSO
    {
        [SerializeField] private List<DialogueDirectingPrefabData> DialogueDirectingPrefabDatas;

        public bool TryGetPrefabData(string prefabKey, out DialogueDirectingPrefabData prefabData)
        {
            prefabData = null;

            foreach (var data in this.DialogueDirectingPrefabDatas)
            {
                if (data.PrefabKey == prefabKey)
                {
                    prefabData = data;
                    return true;
                }
            }

            return false;
        }
    }
}