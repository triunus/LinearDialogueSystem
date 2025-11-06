using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.DialogueDirectingService.Datas
{
    public interface IDialogueDirectingJsonDataSO
    {
        public bool TryGetDialogueDirectingDatas(int dialogueIndex, out TextAsset dialogueDirectingDataJsonFile);
    }

    [System.Serializable]
    [CreateAssetMenu(menuName = "ScriptableObjects/DialogueDirectingService/DialogueDirectingJsonDataSO", fileName = "DialogueDirectingJsonDataSO")]
    public class DialogueDirectingJsonDataSO : ScriptableObject, IDialogueDirectingJsonDataSO
    {
        [SerializeField] private List<DialogueDirectingJsonData> DialogueDirectingJsonDatas;

        public bool TryGetDialogueDirectingDatas(int dialogueIndex, out TextAsset jsonFile)
        {
            jsonFile = null;

            foreach (var data in this.DialogueDirectingJsonDatas)
            {
                if (data.DialogueDirectingIndex == dialogueIndex)
                {
                    jsonFile = data.DialogueDirectingJsonFile;
                    return true;
                }
            }

            return false;
        }
    }

    [System.Serializable]
    public class DialogueDirectingJsonData
    {
        [SerializeField] private int _DialogueDirectingIndex;
        [SerializeField] private TextAsset _DialogueDirectingJsonFile;

        public int DialogueDirectingIndex { get => _DialogueDirectingIndex; }
        public TextAsset DialogueDirectingJsonFile { get => _DialogueDirectingJsonFile; }
    }
}