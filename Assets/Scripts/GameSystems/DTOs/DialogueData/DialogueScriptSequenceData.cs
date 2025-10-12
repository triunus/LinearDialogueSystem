using System.Collections.Generic;

namespace GameSystems.DTOs
{
    public class DialogueSequenceDataList
    {
        public List<SequenceStepData> SequenceStepDatas = new();

        public bool IsSkipable;
    }

    public class SequenceStepData
    {
        public int Index;
        public SequenceDataType SequenceDataType;

        public SayDirectionTextData SayDirectionTextData;
        public FaceDirectionData FaceDirectionData;
        public ActionDirectionData ActionDirectionData;

        public bool IsSkipable;
        public bool IsAutoable;

        // 대화 + 표정 + Actor 행동
        public SequenceStepData(int index, SequenceDataType sequenceDataType, bool isSkipable, bool isAutoable, SayDirectionTextData SayDirectionTextData, FaceDirectionData FaceDirectionData, ActionDirectionData ActionDirectionData)
        {
            Index = index;
            SequenceDataType = sequenceDataType;

            IsSkipable = isSkipable;
            IsAutoable = isAutoable;

            this.SayDirectionTextData = SayDirectionTextData;
            this.FaceDirectionData = FaceDirectionData;
            this.ActionDirectionData = ActionDirectionData;
        }

        // 대화 없이 표정과 액션만
        public SequenceStepData(int index, SequenceDataType sequenceDataType, bool isSkipable, bool isAutoable, FaceDirectionData FaceDirectionData, ActionDirectionData ActionDirectionData)
        {
            Index = index;
            SequenceDataType = sequenceDataType;

            IsSkipable = isSkipable;
            IsAutoable = isAutoable;

            this.SayDirectionTextData = null;
            this.FaceDirectionData = FaceDirectionData;
            this.ActionDirectionData = ActionDirectionData;
        }

        // 액션만, CanvasUIUX or 일러스트
        public SequenceStepData(int index, SequenceDataType sequenceDataType, bool isSkipable, bool isAutoable, ActionDirectionData ActionDirectionData)
        {
            Index = index;
            SequenceDataType = sequenceDataType;

            IsSkipable = isSkipable;
            IsAutoable = isAutoable;

            this.SayDirectionTextData = null;
            this.FaceDirectionData = null;
            this.ActionDirectionData = ActionDirectionData;
        }

    }

    public class SayDirectionTextData
    {
        public string SpeakerName;
        public string Content;

        public SayDirectionTextData(string speakerName, string content)
        {
            SpeakerName = speakerName;
            Content = content;
        }
    }
    public class FaceDirectionData
    {
        public string TargetName;
        public FaceType FaceType;

        public FaceDirectionData(string targetName, FaceType faceType)
        {
            TargetName = targetName;
            FaceType = faceType;
        }
    }
    public class ActionDirectionData
    {
        public string TargetName;

        public ActionType ActionType;
        public string ActionPosition;
        public string ActionTime;

        public ActionDirectionData(string targetName, ActionType actionType, string actionPosition, string actionTime)
        {
            TargetName = targetName;
            ActionType = actionType;
            ActionPosition = actionPosition;
            ActionTime = actionTime;
        }
    }

    public enum SequenceDataType
    {
        SayAndActorActionType,
        ActorActionType,
        CanvasUIUXActionType,
        BackGroundActionType
    }

    public enum FaceType
    {
        Default = 0,
    }

    public enum ActionType
    {
        None,

        FadeIn,
        FadeOut,
        DirectShow,
        DirectHide,

        Move,

        Emotion_Embarrassed
    }
}