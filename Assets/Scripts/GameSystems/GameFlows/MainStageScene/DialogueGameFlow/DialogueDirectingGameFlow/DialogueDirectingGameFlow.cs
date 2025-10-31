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
        // Text 출력 View
        private IDialogueTextDirectingView DialogueTextDirectingView;
        // 선택지 출력 View
        private IDialogueChoiceDirectingViewMediator DialogueChoiceDirectingViewMediator;
        // Image 연출 Hub 퍼사드
        private IDialogueImageDirectingFacade DialogueImageDirectingFacade;

        // 편의를 위한 View
        // Cutscene 출력 View
        private IDialogueCutsceneDirectingView DialogueCutsceneDirectingView;

        // 연출과 다른, 대화 History 관련 View
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
                new DialogueDirectingData(8, "DialogueTextDirectingType", "ActorA_Actor A_안녕 Actor A야", false, true, false, "Next"),
                new DialogueDirectingData(9, "DialogueTextDirectingType", "ActorB_Actor B_Hi Actor B야", false, true, false, "Next"),
                new DialogueDirectingData(10, "DialogueTextDirectingType", "ActorB_ActorB_몇 개 기능을 실험해 볼게", false, true, false, "Next"),
                new DialogueDirectingData(11, "DialogueImageDirectingType", "FadeOut_ActorB_1.5", false, false, true, "Next"),
                new DialogueDirectingData(12, "DialogueImageDirectingType", "SetAttitudeSprite_ActorB_Test", true, false, true, "Next"),
                new DialogueDirectingData(13, "DialogueImageDirectingType", "FadeIn_ActorB_1.5", false, false, true, "Next"),
                new DialogueDirectingData(14, "DialogueTextDirectingType", "ActorB_Actor B_자, 나 뭐 바뀐거 없어?", false, true, false, "Next"),
                new DialogueDirectingData(15, "DialogueTextDirectingType", "Player_Player_갑자기 PTSD가 온다... 뭐가 바뀐거지?", true, false, false, "Next"),
                new DialogueDirectingData(16, "DialogueImageDirectingType", "FadeIn_ChoiceBackGroundUIUX_1", true, false, false, "Next"),
                new DialogueDirectingData(17, "DialogueChoiceDirectingType", "테두리가 바뀌었다_20", true, false, false, "Next"),
                new DialogueDirectingData(18, "DialogueChoiceDirectingType", "내부 이미지가 바뀌었다_24", true, false, false, "Next"),
                new DialogueDirectingData(19, "DialogueChoiceDirectingType", "똑같다_28", false, false, false, "Stop"),
                new DialogueDirectingData(20, "DialogueImageDirectingType", "FadeOut_ChoiceBackGroundUIUX_1", true, false, false, "Next"),
                new DialogueDirectingData(21, "DialogueTextDirectingType", "Player_Player_테두리가 바뀐거 같아", false, false, true, "Next"),
                new DialogueDirectingData(22, "DialogueTextDirectingType", "ActorB_Actor B_맞아", false, true, false, "Next"),
                new DialogueDirectingData(23, "DialogueTextDirectingType", "Player_Player_휴… 다행이다.", false, true, false, "Jump_32"),
                new DialogueDirectingData(24, "DialogueImageDirectingType", "FadeOut_ChoiceBackGroundUIUX_1", true, false, false, "Next"),
                new DialogueDirectingData(25, "DialogueTextDirectingType", "Player_Player_내부 아이콘이 바뀐거 같아", false, false, true, "Next"),
                new DialogueDirectingData(26, "DialogueTextDirectingType", "ActorB_Actor B_아니야, 다시 기회를 줄게.", false, true, false, "Next"),
                new DialogueDirectingData(27, "DialogueTextDirectingType", "Player_Player_내부 아이콘은 아닌거 같다. 뭐가 바뀐거지?", true, false, false, "Jump_16"),
                new DialogueDirectingData(28, "DialogueImageDirectingType", "FadeOut_ChoiceBackGroundUIUX_1", true, false, false, "Next"),
                new DialogueDirectingData(29, "DialogueTextDirectingType", "Player_Player_아까랑 똑같은거 같아.", false, true, false, "Next"),
                new DialogueDirectingData(30, "DialogueTextDirectingType", "ActorB_Actor B_아니야, 다시 기회를 줄게.", false, true, false, "Next"),
                new DialogueDirectingData(31, "DialogueTextDirectingType", "Player_Player_똑같지는 않은거 같다. 뭐가 바뀐거지?", true, false, false, "Jump_16"),
                new DialogueDirectingData(32, "DialogueTextDirectingType", "ActorA_Actor A_일단 여기서 마무리할게", false, true, false, "Next"),
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
            // 이번에 수행하고자 하는 연출 데이터 가져옴.
            // 수행하고자 하는 연출 번호가 잘못된 경우, return.
            if (!this.DialogueDirectingModel.TryGetDialogueDirectingData(currentDirectingIndex, out var dialogueDirectingData))
            {
                Debug.Log($"잘못된 연출 번호 : {currentDirectingIndex}");
                return;
            }

            // 연출 타입 도출.
            DirectingType currentDirectingType = (DirectingType)System.Enum.Parse(typeof(DirectingType), dialogueDirectingData.DirectingType);

            // 연출 타입에 따라, 다른 연출 기능 수행.
            switch (currentDirectingType)
            {
                case DirectingType.DialogueTextDirectingType:
 //                   Debug.Log($"호출됨 3");
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
//                    Debug.Log($"호출됨 4");
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
 //                   Debug.Log($"호출됨 5");
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
                    Debug.Log($"호출됨 6");
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

        // Action 코루틴 제어
        private IEnumerator OperateActionCoroutine(int directingIndex)
        {
            DialogueDirectingCoroutineControlData thisDirectingCoroutineControlData = this.ActionDirectingCoroutineControlDatas.Find(x => x.DirectingIndex == directingIndex);
           
            yield return thisDirectingCoroutineControlData.BehaviourCoroutine;

            thisDirectingCoroutineControlData.BehaviourCoroutine = null;
            thisDirectingCoroutineControlData.ControlCoroutine = null;
            this.ActionDirectingCoroutineControlDatas.Remove(thisDirectingCoroutineControlData);

            this.RequestNextDirecting();
        }
        // Text 코루틴 제어
        private IEnumerator OperateTextDisplayCoroutine()
        {
            yield return this.DialogueDirectingModel.TextDirectingCoroutineControlData.BehaviourCoroutine;

            this.DialogueDirectingModel.TextDirectingCoroutineControlData.BehaviourCoroutine = null;
            this.DialogueDirectingModel.TextDirectingCoroutineControlData.ControlCoroutine = null;

            this.RequestNextDirecting();
        }
        // 1개의 연출 이후, 다음 연출을 이어서 수행할지 결정하는 부분.
        private void RequestNextDirecting()
        {
            // 자동 재생 가능한가? + Text 출력 작업 끝났는가? + Action 동작 끝났는가?
            if (this.LastDirectingData.IsAutoable && this.DialogueDirectingModel.TextDirectingCoroutineControlData.IsOperationEnd() && this.ActionDirectingCoroutineControlDatas.Count == 0)
            {
                if (this.TryGetNextDirectingIndex(this.LastDirectingData.NextDirectiveCommand, out var index))
                {
                    this.OperateDialogueDirecting(index);
                }
            }
        }
        // 다음 수행 연출 번호를 정하는 기능
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
                    Debug.Log($"끝남");
                    return false;
                case NextDirectiveCommandType.None:
                case NextDirectiveCommandType.Stop:
                    nextDirectingIndex = default;
                    Debug.Log($"잠시 정지");
                    return false;
                default:
                    Debug.Log($"NextDirectiveCommandType 변환이 잘못됨.");
                    nextDirectingIndex = default;
                    return false;
            }
        }



        // 마우스 입력 기능
        // '마우스 클릭'을 통해 현재 연출을 정상적으로 마지막 상태 값을 갖도록 한다.
        public void OperateDialogueClickInteraction()
        {
            // 해당 연출이 마우스클릭을 통해 생략이 가능한지 여부.
            if (!this.LastDirectingData.IsSkipable) return;

            // 연출 중이 아니라면, 다음 시퀀스 스탭 시작.
            if (this.ActionDirectingCoroutineControlDatas.Count == 0 && this.DialogueDirectingModel.TextDirectingCoroutineControlData.IsOperationEnd())
            {
                // 다음에 수행할 DirectingIndex 추출 확인.
                if (this.TryGetNextDirectingIndex(this.LastDirectingData.NextDirectiveCommand, out var index))
                {
                    // 추출하였을 시, 해당 연출 수행.
                    this.OperateDialogueDirecting(index);
                    this.AutoPlayDirectingData.CurrentWaitDuration = 0;
                }
            }
            // 연출 중이라면, 중지 및 전부 출력.
            else
            {
                this.StopDialogueDirection();
            }
        }


        // Auto 재생 기능
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

                //  현재 재생 중인 코루틴이 있으면 그냥 넘어감.
                if (this.ActionDirectingCoroutineControlDatas.Count == 0 && this.DialogueDirectingModel.TextDirectingCoroutineControlData.IsOperationEnd())
                {
                    // 충분히 기다렸으면, 다음 대화 연출 수행 요청.
                    if (this.AutoPlayDirectingData.CurrentWaitDuration >= this.AutoPlayDirectingData.AutoWaitDuration)
                    {
                        if (this.TryGetNextDirectingIndex(this.LastDirectingData.NextDirectiveCommand, out var index))
                        {
                            this.OperateDialogueDirecting(index);
                        }

                        this.AutoPlayDirectingData.CurrentWaitDuration = 0;
                    }
                    // 현재 재생 중인 코루틴이 없으면 시간 증가.
                    else
                    {
                        this.AutoPlayDirectingData.CurrentWaitDuration += Time.deltaTime;
                    }
                }

                yield return Time.deltaTime;
            }

            // Auto 코루틴 초기화.
            this.AutoPlayDirectingData.Reset();
        }


        // Skip 기능
        public void OnClickedSkipButton()
        {
            // 스킵 허용 안할시 넘어감.
            if (!this.LastDirectingData.IsSkipable) return;

            this.StopDialogueDirection();
        }


        // 진행 중인, 코루틴 동작이 있다면, 토큰을 통하여 자연스럽게 마무리가 되도록 유도한다.
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