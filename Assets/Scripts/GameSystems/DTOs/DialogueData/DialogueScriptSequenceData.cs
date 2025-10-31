using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.DTOs
{
    public interface IDialogueDirectingJsonDataDB
    {
        public bool TryGetDialogueDirectingData(string key);
    }

    [System.Serializable]
    public class DialogueDirectingJsonDataDB
    {
        List<DialogueDirectingJsonData> DialogueDirectingJsonDatas;
    }

    [System.Serializable]
    public class DialogueDirectingJsonData
    {
        public string Key;
        public TextAsset JsonFile;
    }

    [System.Serializable]
    public class DialogueDirectingData
    {
        public int Index;

        public string DirectingType;
        public string DirectingContent;
        public bool IsChainWithNext;
        public bool IsSkipable;
        public bool IsAutoable;
        public string NextDirectiveCommand;

        public DialogueDirectingData() { }

        public DialogueDirectingData(int index, string directingType, string directingContent, bool isChainWithNext, bool isSkipable, bool isAutoAble, string nextDirectiveCommand)
        {
            Index = index;
            DirectingType = directingType;
            DirectingContent = directingContent;
            IsChainWithNext = isChainWithNext;
            IsSkipable = isSkipable;
            IsAutoable = isAutoAble;
            NextDirectiveCommand = nextDirectiveCommand;
        }
    }

    [System.Serializable]
    public enum DirectingType
    {
        DialogueTextDirectingType,
        DialogueChoiceDirectingType,
        DialogueCutsceneDirectingType,
        DialogueImageDirectingType
    }
    [System.Serializable]
    public enum ActionType
    {
        None = 0,

        FadeIn = 50,
        FadeOut = 51,
        DirectShow = 1,
        DirectHide = 2,

        SetPosition = 100,
        Move = 110,

        SetFaceSprite = 200,
        SetAttitudeSprite = 210,
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