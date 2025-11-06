using UnityEngine;
using System.Collections.Generic;

namespace GameSystems.DialogueDirectingService.Datas
{
    public interface IDialogueDirectingResourceJsonDataSO
    {
        public bool TryGetDialogueDirectingResourceJsonData(int dialogueDirectingIndex, out DialogueDirectingResourceJsonData dialogueDirectingResourceJsonData);
    }

    // 특정 연출 Index에 대응되는 필요한 리소스들을 담은 Json 파일들을 모아둔 SO 파일.
    [System.Serializable]
    [CreateAssetMenu(menuName = "ScriptableObject/DialogueDirectingService/ResourceJsonDataSO", fileName = "ResourceJsonDataSO")]
    public class DialogueDirectingResourceJsonDataSO : ScriptableObject, IDialogueDirectingResourceJsonDataSO
    {
        [SerializeField] private List<DialogueDirectingResourceJsonData> DialogueDirectingResourceJsonDatas;

        public bool TryGetDialogueDirectingResourceJsonData(int dialogueDirectingIndex, out DialogueDirectingResourceJsonData dialogueDirectingResourceJsonData)
        {
            dialogueDirectingResourceJsonData = null;
            foreach (var data in this.DialogueDirectingResourceJsonDatas)
            {
                if(data.DialogueDirectingIndex == dialogueDirectingIndex)
                {
                    dialogueDirectingResourceJsonData = data;
                    return true;
                }
            }

            return false;
        }
    }

    // 해당 연출 Index에 대응되는 필요한 리소스들을 담은 Json 파일
    [System.Serializable]
    public class DialogueDirectingResourceJsonData
    {
        [SerializeField] private int dialogueDirectingIndex;

        [SerializeField] private TextAsset jsonFile;

        public int DialogueDirectingIndex { get => dialogueDirectingIndex; }

        public TextAsset JsonFile { get => jsonFile; }
    }

}
