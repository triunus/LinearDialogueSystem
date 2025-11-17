using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.InfrastructureLayer.DialogueDirectingSystem
{
    public interface IDialogueDirectingJsonDataSO
    {
        public bool TryGetDialogueDirectingJsonData(string dialogueIndex, out DialogueDirectingJsonData dialogueDirectingJsonData);
    }

    [Serializable]
    [CreateAssetMenu(menuName = "ScriptableObjects/DialogueDirectingSystem/DialogueDirectingJsonDataSO", fileName = "DialogueDirectingJsonDataSO")]
    public class DialogueDirectingJsonDataSO : ScriptableObject, IDialogueDirectingJsonDataSO
    {
        [SerializeField] private List<DialogueDirectingJsonData> DialogueDirectingJsonDatas;

        public bool TryGetDialogueDirectingJsonData(string dialogueIndex, out DialogueDirectingJsonData dialogueDirectingJsonData)
        {
            foreach(var data in this.DialogueDirectingJsonDatas)
            {
                if(data.DialogueIndex == dialogueIndex)
                {
                    dialogueDirectingJsonData = data;
                    return true;
                }
            }

            dialogueDirectingJsonData = null;
            return false;
        }
    }

    [Serializable]
    public class DialogueDirectingJsonData
    {
        [SerializeField] private string dialogueIndex;
        [SerializeField] private TextAsset dialogueJson;

        public string DialogueIndex { get => dialogueIndex; }
        public TextAsset DialogueJson { get => dialogueJson; }
    }
}