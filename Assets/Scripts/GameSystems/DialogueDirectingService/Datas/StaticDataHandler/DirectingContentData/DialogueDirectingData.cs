using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.DialogueDirectingService.Datas
{
    public interface IDialogueDirectingDataGroup
    {
        public bool TryGetDialogueDirectingData(int directingIndex, out DialogueDirectingData dialogueDirectingData);
    }

    [System.Serializable]
    public class DialogueDirectingDataGroup
    {
        [SerializeField] private List<DialogueDirectingData> DialogueDirectingDatas;

        public int Count => this.DialogueDirectingDatas.Count;

        public bool TryGetDialogueDirectingData(int directingIndex, out DialogueDirectingData dialogueDirectingData)
        {
            dialogueDirectingData = null;

            foreach (var data in this.DialogueDirectingDatas)
            {
                if(data.Index == directingIndex)
                {
                    dialogueDirectingData = data;
                    return true;
                }
            }

            return false;
        }
    }

    [System.Serializable]
    public class DialogueDirectingData
    {
        public int Index;

        public string DirectingActionType;
        public string DirectingTarget;
        public string DirectingContent;
        public bool IsChainWithNext;
        public bool IsSkipable;
        public bool IsAutoable;
        public string NextDirectiveCommand;

        public DialogueDirectingData() { }

        public DialogueDirectingData(int index, string directingActionType, string directingTarget, string directingContent, bool isChainWithNext, bool isSkipable, bool isAutoAble, string nextDirectiveCommand)
        {
            Index = index;
            DirectingActionType = directingActionType;
            DirectingTarget = directingTarget;
            DirectingContent = directingContent;
            IsChainWithNext = isChainWithNext;
            IsSkipable = isSkipable;
            IsAutoable = isAutoAble;
            NextDirectiveCommand = nextDirectiveCommand;
        }
    }

    [System.Serializable]
    public enum DirectingActionType
    {
        None = 0,

        DirectShow = 1,
        DirectHide = 2,

        FadeIn = 50,
        FadeOut = 51,

        SetFaceSprite = 200,
        SetAttitudeSprite = 210,

        SetPosition = 100,
        Move = 110,

        SetTextDisplay = 300,

        SetButton = 350,
    }
    [System.Serializable]
    public enum NextDirectiveCommandType
    {
        None,
        Stop,
        Next,
        Jump,
        End
    }

    [System.Serializable]
    public enum AttitudeType
    {
        None = 0,
        Default = 1,

        Test
    }
    [System.Serializable]
    public enum FaceType
    {
        None = 0,
        Default = 1,

        Test
    }
}