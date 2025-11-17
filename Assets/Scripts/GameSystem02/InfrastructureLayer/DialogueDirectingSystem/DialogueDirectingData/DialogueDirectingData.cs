using System;

namespace GameSystems.InfrastructureLayer.DialogueDirectingSystem
{
    public interface IDialogueDirectingDataList
    {
        public bool TryGetDialogueDirectingData(int directingIndex, out DialogueDirectingData dialogueDirectingData);
    }

    [Serializable]
    public class DialogueDirectingDataList : IDialogueDirectingDataList
    {
        private DialogueDirectingData[] DialogueDirectingDatas;

        public bool TryGetDialogueDirectingData(int directingIndex, out DialogueDirectingData dialogueDirectingData)
        {
            for(int i = 0; i < this.DialogueDirectingDatas.Length; ++i)
            {
                if(this.DialogueDirectingDatas[i].DirectingIndex == directingIndex)
                {
                    dialogueDirectingData = this.DialogueDirectingDatas[i];
                    return true;
                }
            }

            dialogueDirectingData = null;
            return false;
        }
    }

    [Serializable]
    public class DialogueDirectingData
    {
        private int directingIndex;

        private ActorType actorType;
        private string actorKey;
        private ActionType actionType;
        private string actionContent;
        private int isChainWithNext;
        private string nextDirectiveCommand;

        public DialogueDirectingData(int directingIndex, ActorType actorType, string actorKey, ActionType actionType, string actionContent, int isChainWithNext, string nextDirectiveCommand)
        {
            this.directingIndex = directingIndex;

            this.actorType = actorType;
            this.actorKey = actorKey;
            this.actionType = actionType;
            this.actionContent = actionContent;
            this.isChainWithNext = isChainWithNext;
            this.nextDirectiveCommand = nextDirectiveCommand;
        }

        public int DirectingIndex { get => directingIndex; }
        public ActorType ActorType { get => actorType; }
        public string ActorKey { get => actorKey; }
        public ActionType ActionType { get => actionType; }
        public string ActionContent { get => actionContent; }
        public int IsChainWithNext { get => isChainWithNext; }
        public string NextDirectiveCommand { get => nextDirectiveCommand; }
    }

    [Serializable]
    public enum ActorType
    {
        Actor,
        BackGround,
        CutScene,
        FixedBottomTextUI
    }

    [Serializable]
    public enum ActionType
    {
        Default,
        DirectShow,
        DirectHide,
        FadeIn,
        FadeOut,
        SetPosition,
        SetMove,
        SetBaseSprite,
        SetDetailSprite
    }

}