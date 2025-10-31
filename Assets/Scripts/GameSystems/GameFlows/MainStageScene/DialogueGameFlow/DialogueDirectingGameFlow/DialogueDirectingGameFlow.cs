using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using GameSystems.DTOs;
using GameSystems.UnityServices;
using GameSystems.Entities.MainStageScene;

namespace GameSystems.GameFlows.MainStageScene
{
    public interface IDialogueDirectingGameFlow
    {
        public void OperateDialogueDirecting(int currentDirectingIndex);
        public void OperateDialogueClickInteraction_ChoiceButton(int branchIndex);
        public void OperateDialogueClickInteraction();
    }

    public class DialogueDirectingGameFlow : IDialogueDirectingGameFlow
    {
        // Text ��� View
        private IDialogueTextDirectingView DialogueTextDirectingView;
        // ������ ��� View
        private IDialogueChoiceDirectingViewMediator DialogueChoiceDirectingViewMediator;
        // Image ���� Hub �ۻ��
        private IDialogueImageDirectingFacade DialogueImageDirectingFacade;

        // ���Ǹ� ���� View
        // Cutscene ��� View
        private IDialogueCutsceneDirectingView DialogueCutsceneDirectingView;

        // ����� �ٸ�, ��ȭ History ���� View
        private IDialogueHistoryGenerator DialogueHistoryGenerator;

        private ICoroutineRunner CoroutineRunner;

        private IDialogueDirectingModel DialogueDirectingModel;

        public DialogueDirectingGameFlow(
            IDialogueDirectingModel dialogueDirectingModel,

            IDialogueTextDirectingView DialogueTextDirectingView,
            IDialogueChoiceDirectingViewMediator DialogueChoiceDirectingViewMediator,
            IDialogueCutsceneDirectingView DialogueCutsceneDirectingView,

            IDialogueImageDirectingFacade DialogueImageDirectingFacade,

            IDialogueHistoryGenerator DialogueHistoryGenerator,

            ICoroutineRunner CoroutineRunner)
        {
            this.DialogueDirectingModel = dialogueDirectingModel;

            this.DialogueTextDirectingView = DialogueTextDirectingView;
            this.DialogueChoiceDirectingViewMediator = DialogueChoiceDirectingViewMediator;
            this.DialogueCutsceneDirectingView = DialogueCutsceneDirectingView;

            this.DialogueImageDirectingFacade = DialogueImageDirectingFacade;

            this.DialogueHistoryGenerator = DialogueHistoryGenerator;

            this.CoroutineRunner = CoroutineRunner;

            this.InitialSetting();
        }


        private void InitialSetting()
        {
            this.TotalDialogueDirectingDatas = new()
            {
                new DialogueDirectingData(0, "DialogueCutsceneDirectingType", "Title01_Chapter01_1_1_1", false, false, true, "Next"),
                new DialogueDirectingData(1, "DialogueImageDirectingType", "FadeIn_DefaultBackGround_1", true, false, true, "Next"),
                new DialogueDirectingData(2, "DialogueImageDirectingType", "FadeIn_DefaultDialogueUIUX_1", false, false, true, "Next"),
                new DialogueDirectingData(3, "DialogueImageDirectingType", "SetPosition_ActorA_2", true, false, true, "Next"),
                new DialogueDirectingData(4, "DialogueImageDirectingType", "FadeIn_ActorA_3", false, true, true, "Next"),
                new DialogueDirectingData(5, "DialogueImageDirectingType", "Move_ActorB_10-8/3", false, true, true, "Next"),
                new DialogueDirectingData(6, "DialogueImageDirectingType", "Move_ActorA_2-3-2-3-2/1.5-1.5-1.5-1.5", false, true, true, "Next"),
                new DialogueDirectingData(7, "DialogueImageDirectingType", "FadeIn_DefaultTextPanel_1", false, false, true, "Next"),
                new DialogueDirectingData(8, "DialogueTextDirectingType", "ActorA_Actor A_�ȳ� Actor A��", false, true, false, "Next"),
                new DialogueDirectingData(9, "DialogueTextDirectingType", "ActorB_Actor B_Hi Actor B��", false, true, false, "Next"),
                new DialogueDirectingData(10, "DialogueTextDirectingType", "ActorB_ActorB_�� �� ����� ������ ����", false, true, false, "Next"),
                new DialogueDirectingData(11, "DialogueImageDirectingType", "FadeOut_ActorB_1.5", false, false, true, "Next"),
                new DialogueDirectingData(12, "DialogueImageDirectingType", "SetAttitudeSprite_ActorB_Test", true, false, true, "Next"),
                new DialogueDirectingData(13, "DialogueImageDirectingType", "FadeIn_ActorB_1.5", false, false, true, "Next"),
                new DialogueDirectingData(14, "DialogueTextDirectingType", "ActorB_Actor B_��, �� �� �ٲ�� ����?", false, true, false, "Next"),
                new DialogueDirectingData(15, "DialogueTextDirectingType", "Player_Player_���ڱ� PTSD�� �´�... ���� �ٲ����?", true, false, false, "Next"),
                new DialogueDirectingData(16, "DialogueImageDirectingType", "FadeIn_ChoiceBackGroundUIUX_1", true, false, false, "Next"),
                new DialogueDirectingData(17, "DialogueChoiceDirectingType", "�׵θ��� �ٲ����_20", true, false, false, "Next"),
                new DialogueDirectingData(18, "DialogueChoiceDirectingType", "���� �̹����� �ٲ����_24", true, false, false, "Next"),
                new DialogueDirectingData(19, "DialogueChoiceDirectingType", "�Ȱ���_28", false, false, false, "Stop"),
                new DialogueDirectingData(20, "DialogueImageDirectingType", "FadeOut_ChoiceBackGroundUIUX_1", true, false, false, "Next"),
                new DialogueDirectingData(21, "DialogueTextDirectingType", "Player_Player_�׵θ��� �ٲ�� ����", false, false, true, "Next"),
                new DialogueDirectingData(22, "DialogueTextDirectingType", "ActorB_Actor B_�¾�", false, true, false, "Next"),
                new DialogueDirectingData(23, "DialogueTextDirectingType", "Player_Player_�ޡ� �����̴�.", false, true, false, "Jump_32"),
                new DialogueDirectingData(24, "DialogueImageDirectingType", "FadeOut_ChoiceBackGroundUIUX_1", true, false, false, "Next"),
                new DialogueDirectingData(25, "DialogueTextDirectingType", "Player_Player_���� �������� �ٲ�� ����", false, false, true, "Next"),
                new DialogueDirectingData(26, "DialogueTextDirectingType", "ActorB_Actor B_�ƴϾ�, �ٽ� ��ȸ�� �ٰ�.", false, true, false, "Next"),
                new DialogueDirectingData(27, "DialogueTextDirectingType", "Player_Player_���� �������� �ƴѰ� ����. ���� �ٲ����?", true, false, false, "Jump_16"),
                new DialogueDirectingData(28, "DialogueImageDirectingType", "FadeOut_ChoiceBackGroundUIUX_1", true, false, false, "Next"),
                new DialogueDirectingData(29, "DialogueTextDirectingType", "Player_Player_�Ʊ�� �Ȱ����� ����.", false, true, false, "Next"),
                new DialogueDirectingData(30, "DialogueTextDirectingType", "ActorB_Actor B_�ƴϾ�, �ٽ� ��ȸ�� �ٰ�.", false, true, false, "Next"),
                new DialogueDirectingData(31, "DialogueTextDirectingType", "Player_Player_�Ȱ����� ������ ����. ���� �ٲ����?", true, false, false, "Jump_16"),
                new DialogueDirectingData(32, "DialogueTextDirectingType", "ActorA_Actor A_�ϴ� ���⼭ �������Ұ�", false, true, false, "Next"),
                new DialogueDirectingData(33, "DialogueImageDirectingType", "FadeOut_DefaultTextPanel_1", false, false, true, "Next"),
                new DialogueDirectingData(34, "DialogueImageDirectingType", "FadeOut_ActorA_1", true, false, true, "Next"),
                new DialogueDirectingData(35, "DialogueImageDirectingType", "FadeOut_ActorB_1", false, false, true, "Next"),
                new DialogueDirectingData(36, "DialogueImageDirectingType", "FadeOut_DefaultBackGround_1", true, false, true, "Next"),
                new DialogueDirectingData(37, "DialogueImageDirectingType", "FadeOut_DefaultDialogueUIUX_1", false, false, true, "End"),
//                new DialogueDirectingData(35, "DialogueImageDirectingType", "SetFaceSprite_ActorA_Test", true, false, true, "End"),
            };
        }

        public void OperateDialogueDirecting(int currentDirectingIndex)
        {
            // �̹��� �����ϰ��� �ϴ� ���� ������ ������.
            // �����ϰ��� �ϴ� ���� ��ȣ�� �߸��� ���, return.
            if (!this.DialogueDirectingModel.TryGetDialogueDirectingData(currentDirectingIndex, out var dialogueDirectingData))
            {
                Debug.Log($"�߸��� ���� ��ȣ : {currentDirectingIndex}");
                return;
            }

            // ���� Ÿ�� ����.
            DirectingType currentDirectingType = (DirectingType)System.Enum.Parse(typeof(DirectingType), dialogueDirectingData.DirectingType);

            // ���� Ÿ�Կ� ����, �ٸ� ���� ��� ����.
            switch (currentDirectingType)
            {
                case DirectingType.DialogueTextDirectingType:
 //                   Debug.Log($"ȣ��� 3");
                    if (this.DialogueTextDirectingView.TryDirectTextDisplayOperation(dialogueDirectingData.DirectingContent, out var textDisplayCo, out var textDisplayToken) && textDisplayCo != null
                        && this.DialogueImageDirectingFacade.TrySetSpeakerAndListenerColor(dialogueDirectingData.DirectingContent))
                    {
                        this.DialogueDirectingModel.TextDirectingCoroutineControlData.DirectingIndex = dialogueDirectingData.Index;
                        this.DialogueDirectingModel.TextDirectingCoroutineControlData.BehaviourCoroutine = this.CoroutineRunner.Run(textDisplayCo);
                        this.DialogueDirectingModel.TextDirectingCoroutineControlData.ControlCoroutine = this.CoroutineRunner.Run(this.OperateTextDisplayCoroutine());
                        this.DialogueDirectingModel.TextDirectingCoroutineControlData.BehaviourToken = textDisplayToken;

                        this.DialogueHistoryGenerator.AddDialogueHistory(dialogueDirectingData.Index, dialogueDirectingData.DirectingContent);
                    }
                    break;
                case DirectingType.DialogueChoiceDirectingType:
//                    Debug.Log($"ȣ��� 4");
                    if (this.DialogueChoiceDirectingViewMediator.TryDirectChoiceViewOperation(dialogueDirectingData.DirectingContent, out var choiceDisplayCo, out var choiceActionToken) && choiceDisplayCo != null)
                    {
                        DialogueDirectingCoroutineControlData newDirectingCoroutineControlData = new DialogueDirectingCoroutineControlData();
                        this.ActionDirectingCoroutineControlDatas.Add(newDirectingCoroutineControlData);

                        newDirectingCoroutineControlData.DirectingIndex = dialogueDirectingData.Index;
                        newDirectingCoroutineControlData.BehaviourCoroutine = this.CoroutineRunner.Run(choiceDisplayCo);
                        newDirectingCoroutineControlData.ControlCoroutine = this.CoroutineRunner.Run(this.OperateActionCoroutine(newDirectingCoroutineControlData.DirectingIndex));
                        newDirectingCoroutineControlData.BehaviourToken = choiceActionToken;
                    }
                    break;
                case DirectingType.DialogueCutsceneDirectingType:
 //                   Debug.Log($"ȣ��� 5");
                    if (this.DialogueCutsceneDirectingView.TryDirectCutsceneDisplayOperation(dialogueDirectingData.DirectingContent, out var cutsceneCo, out var cutsceneActionToken) && cutsceneCo != null)
                    {
                        DialogueDirectingCoroutineControlData newDirectingCoroutineControlData = new DialogueDirectingCoroutineControlData();
                        this.ActionDirectingCoroutineControlDatas.Add(newDirectingCoroutineControlData);

                        newDirectingCoroutineControlData.DirectingIndex = dialogueDirectingData.Index;
                        newDirectingCoroutineControlData.BehaviourCoroutine = this.CoroutineRunner.Run(cutsceneCo);
                        newDirectingCoroutineControlData.ControlCoroutine = this.CoroutineRunner.Run(this.OperateActionCoroutine(newDirectingCoroutineControlData.DirectingIndex));
                        newDirectingCoroutineControlData.BehaviourToken = cutsceneActionToken;
                    }
                    break;
                case DirectingType.DialogueImageDirectingType:
                    Debug.Log($"ȣ��� 6");
                    if (this.DialogueImageDirectingFacade.TryAction(dialogueDirectingData.DirectingContent, out var ImageDirectingCo, out var imageBehaviourToken) && ImageDirectingCo != null)
                    {
                        DialogueDirectingCoroutineControlData newDirectingCoroutineControlData = new DialogueDirectingCoroutineControlData();
                        this.ActionDirectingCoroutineControlDatas.Add(newDirectingCoroutineControlData);

                        newDirectingCoroutineControlData.DirectingIndex = dialogueDirectingData.Index;
                        newDirectingCoroutineControlData.BehaviourCoroutine = this.CoroutineRunner.Run(ImageDirectingCo);
                        newDirectingCoroutineControlData.ControlCoroutine = this.CoroutineRunner.Run(this.OperateActionCoroutine(newDirectingCoroutineControlData.DirectingIndex));
                        newDirectingCoroutineControlData.BehaviourToken = imageBehaviourToken;

                        Debug.Log($"DirectingIndex : {newDirectingCoroutineControlData.DirectingIndex}, BehaviourCoroutine : {newDirectingCoroutineControlData.BehaviourCoroutine}," +
                            $" BehaviourCoroutine : {newDirectingCoroutineControlData.BehaviourCoroutine}, BehaviourToken : {newDirectingCoroutineControlData.BehaviourToken.IsRequestEnd}");
                    }
                    break;
                default:
                    break;
            }

            this.LastDirectingData.Index = dialogueDirectingData.Index;
            this.LastDirectingData.IsSkipable = dialogueDirectingData.IsSkipable;
            this.LastDirectingData.IsAutoable = dialogueDirectingData.IsAutoable;
            this.LastDirectingData.NextDirectiveCommand = dialogueDirectingData.NextDirectiveCommand;

            if (dialogueDirectingData.IsChainWithNext)
            {
                if (this.TryGetNextDirectingIndex(dialogueDirectingData.NextDirectiveCommand, out var nextIndex))
                {
                    this.OperateDialogueDirecting(nextIndex);
                    return;
                }
            }
        }
        public void OperateDialogueClickInteraction_ChoiceButton(int branchIndex)
        {
            this.OperateDialogueDirecting(branchIndex);
        }

        // Action �ڷ�ƾ ����
        private IEnumerator OperateActionCoroutine(int directingIndex)
        {
            DialogueDirectingCoroutineControlData thisDirectingCoroutineControlData = this.ActionDirectingCoroutineControlDatas.Find(x => x.DirectingIndex == directingIndex);
           
            yield return thisDirectingCoroutineControlData.BehaviourCoroutine;

            thisDirectingCoroutineControlData.BehaviourCoroutine = null;
            thisDirectingCoroutineControlData.ControlCoroutine = null;
            this.ActionDirectingCoroutineControlDatas.Remove(thisDirectingCoroutineControlData);

            this.RequestNextDirecting();
        }
        // Text �ڷ�ƾ ����
        private IEnumerator OperateTextDisplayCoroutine()
        {
            yield return this.DialogueDirectingModel.TextDirectingCoroutineControlData.BehaviourCoroutine;

            this.DialogueDirectingModel.TextDirectingCoroutineControlData.BehaviourCoroutine = null;
            this.DialogueDirectingModel.TextDirectingCoroutineControlData.ControlCoroutine = null;

            this.RequestNextDirecting();
        }
        // 1���� ���� ����, ���� ������ �̾ �������� �����ϴ� �κ�.
        private void RequestNextDirecting()
        {
            // �ڵ� ��� �����Ѱ�? + Text ��� �۾� �����°�? + Action ���� �����°�?
            if (this.LastDirectingData.IsAutoable && this.DialogueDirectingModel.TextDirectingCoroutineControlData.IsOperationEnd() && this.ActionDirectingCoroutineControlDatas.Count == 0)
            {
                if (this.TryGetNextDirectingIndex(this.LastDirectingData.NextDirectiveCommand, out var index))
                {
                    this.OperateDialogueDirecting(index);
                }
            }
        }
        // ���� ���� ���� ��ȣ�� ���ϴ� ���
        private bool TryGetNextDirectingIndex(string nextDirectiveCommand, out int nextDirectingIndex)
        {
            string[] parsedData = nextDirectiveCommand.Split('_');

            NextDirectiveCommandType nextDirectiveCommandType = (NextDirectiveCommandType)System.Enum.Parse(typeof(NextDirectiveCommandType), parsedData[0]);

            switch(nextDirectiveCommandType)
            {
                case NextDirectiveCommandType.Next:
                    nextDirectingIndex = this.LastDirectingData.Index + 1;
                    return true;
                case NextDirectiveCommandType.Jump:
                    nextDirectingIndex = int.Parse(parsedData[1]);
                    return true;
                case NextDirectiveCommandType.End:
                    nextDirectingIndex = default;
                    Debug.Log($"����");
                    return false;
                case NextDirectiveCommandType.None:
                case NextDirectiveCommandType.Stop:
                    nextDirectingIndex = default;
                    Debug.Log($"��� ����");
                    return false;
                default:
                    Debug.Log($"NextDirectiveCommandType ��ȯ�� �߸���.");
                    nextDirectingIndex = default;
                    return false;
            }
        }



        // ���콺 �Է� ���
        // '���콺 Ŭ��'�� ���� ���� ������ ���������� ������ ���� ���� ������ �Ѵ�.
        public void OperateDialogueClickInteraction()
        {
            // �ش� ������ ���콺Ŭ���� ���� ������ �������� ����.
            if (!this.LastDirectingData.IsSkipable) return;

            // ���� ���� �ƴ϶��, ���� ������ ���� ����.
            if (this.ActionDirectingCoroutineControlDatas.Count == 0 && this.DialogueDirectingModel.TextDirectingCoroutineControlData.IsOperationEnd())
            {
                // ������ ������ DirectingIndex ���� Ȯ��.
                if (this.TryGetNextDirectingIndex(this.LastDirectingData.NextDirectiveCommand, out var index))
                {
                    // �����Ͽ��� ��, �ش� ���� ����.
                    this.OperateDialogueDirecting(index);
                    this.AutoPlayDirectingData.CurrentWaitDuration = 0;
                }
            }
            // ���� ���̶��, ���� �� ���� ���.
            else
            {
                this.StopDialogueDirection();
            }
        }


        // Auto ��� ���
        public void OnClickedAutoButton()
        {
            if (this.AutoPlayDirectingData.AutoCoroutine == null)
            {
                this.CoroutineRunner.Run(this.OperateDialogueAutoDisplay());
            }
            else
            {
                this.AutoPlayDirectingData.IsRequestEnd = true;
                this.AutoPlayDirectingData.Reset();
            }
        }
        private IEnumerator OperateDialogueAutoDisplay()
        {
            while (true)
            {
                if (this.AutoPlayDirectingData.IsRequestEnd) break;

                //  ���� ��� ���� �ڷ�ƾ�� ������ �׳� �Ѿ.
                if (this.ActionDirectingCoroutineControlDatas.Count == 0 && this.DialogueDirectingModel.TextDirectingCoroutineControlData.IsOperationEnd())
                {
                    // ����� ��ٷ�����, ���� ��ȭ ���� ���� ��û.
                    if (this.AutoPlayDirectingData.CurrentWaitDuration >= this.AutoPlayDirectingData.AutoWaitDuration)
                    {
                        if (this.TryGetNextDirectingIndex(this.LastDirectingData.NextDirectiveCommand, out var index))
                        {
                            this.OperateDialogueDirecting(index);
                        }

                        this.AutoPlayDirectingData.CurrentWaitDuration = 0;
                    }
                    // ���� ��� ���� �ڷ�ƾ�� ������ �ð� ����.
                    else
                    {
                        this.AutoPlayDirectingData.CurrentWaitDuration += Time.deltaTime;
                    }
                }

                yield return Time.deltaTime;
            }

            // Auto �ڷ�ƾ �ʱ�ȭ.
            this.AutoPlayDirectingData.Reset();
        }


        // Skip ���
        public void OnClickedSkipButton()
        {
            // ��ŵ ��� ���ҽ� �Ѿ.
            if (!this.LastDirectingData.IsSkipable) return;

            this.StopDialogueDirection();
        }


        // ���� ����, �ڷ�ƾ ������ �ִٸ�, ��ū�� ���Ͽ� �ڿ������� �������� �ǵ��� �����Ѵ�.
        private void StopDialogueDirection()
        {
            if (!this.DialogueDirectingModel.TextDirectingCoroutineControlData.IsOperationEnd())
            {
                this.DialogueDirectingModel.TextDirectingCoroutineControlData.BehaviourToken.IsRequestEnd = true;
            }

            foreach (var controlData in this.ActionDirectingCoroutineControlDatas)
            {
                controlData.BehaviourToken.IsRequestEnd = true;
            }
        }
    }
}