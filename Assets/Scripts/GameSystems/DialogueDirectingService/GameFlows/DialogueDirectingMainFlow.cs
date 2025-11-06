using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Foundations.PlugInHub;
using GameSystems.UnityServices;
using GameSystems.DialogueDirectingService.Datas;
using GameSystems.DialogueDirectingService.GameFlow;
using GameSystems.DialogueDirectingService.Views;

namespace GameSystems.DialogueDirectingService.GameFlow
{
    public interface IDialogueDirectingGameFlow
    {
        public void OperateDialogueDirecting(int currentDirectingIndex);
        public void OperateDialogueClickInteraction_ChoiceButton(int branchIndex);
        public void OperateDialogueClickInteraction();
    }

    public class DialogueDirectingMainFlow : IDialogueDirectingGameFlow
    {
        private IDialogueViewObjectDataHandler DialogueViewObjectDataHandler;

        // Praser
        private IDialogueViewFadeParser DialogueViewFadeParser;
        private IDialogueViewPositioner DialogueViewPositioner;
        private IDialogueViewTextParser DialogueViewTextParser;
        private IDialogueViewChoiceButtonParser DialogueViewChoiceButtonParser;

        // Image 연출 하위 flow

        private IDialogueDirectingControlData DialogueDirectingControlData;
        private ICoroutineRunner CoroutineRunner;

        // 자동 재생 기능 코루틴 제어 데이터.
        private AutoDialogueDirectingData AutoPlayDirectingData = new(2f);


        public DialogueDirectingMainFlow(
            IDialogueViewObjectDataHandler dialogueViewObjectDataHandler,
            IDialogueDirectingControlData dialogueDirectingControlData,
            ICoroutineRunner coroutineRunner)
        {
            this.DialogueViewObjectDataHandler = dialogueViewObjectDataHandler;

            this.DialogueViewFadeParser = new DialogueViewFadeParser();
            this.DialogueViewPositioner = new DialogueViewPositionParser();
            this.DialogueViewTextParser = new DialogueViewTextParser();
            this.DialogueViewChoiceButtonParser = new DialogueViewChoiceButtonParser();

            this.DialogueDirectingControlData = dialogueDirectingControlData;

            this.CoroutineRunner = coroutineRunner;
        }


/*        private void InitialSetting()
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
        }*/

        // 연출 형식 / 연출 대상 / 연출 내용
        public void OperateDialogueDirecting(int currentDirectingIndex)
        {
            // 이번에 수행하고자 하는 연출 데이터 가져옴.
            // 수행하고자 하는 연출 번호가 잘못된 경우, return.
            if (!this.DialogueDirectingControlData.TryGetDialogueDirectingData(currentDirectingIndex, out var dialogueDirectingData))
            {
                Debug.Log($"잘못된 연출 번호 : {currentDirectingIndex}");
                return;
            }

            string[] parsedContent = dialogueDirectingData.DirectingContent.Split('_');
            DirectingActionType DirectingActionType = (DirectingActionType)System.Enum.Parse(typeof(DirectingActionType), parsedContent[0]);

            BehaviourToken behaviourToken = null;
            IEnumerator actionCoroutine = null;

            switch (DirectingActionType)
            {
                case DirectingActionType.FadeIn:
                    if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<IFadeInAndOut>(parsedContent[1], out var fadeInView)) break;
                    if (!this.DialogueViewFadeParser.TryParseDuration(parsedContent[2], out var fadeInDuration)) break;

                    behaviourToken = new BehaviourToken(isRequestEnd: false);
                    actionCoroutine = fadeInView.FadeIn(fadeInDuration, behaviourToken);

                    break;
                case DirectingActionType.FadeOut:
                    if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<IFadeInAndOut>(parsedContent[1], out var fadeOutView)) break;
                    if (!this.DialogueViewFadeParser.TryParseDuration(parsedContent[2], out var fadeOutDuration)) break;

                    behaviourToken = new BehaviourToken(isRequestEnd: false);
                    actionCoroutine = fadeOutView.FadeOut(fadeOutDuration, behaviourToken);
                    break;
                case DirectingActionType.DirectShow:
                    if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<IActivation>(parsedContent[1], out var showView)) break;

                    showView.Show();
                    break;
                case DirectingActionType.DirectHide:
                    if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<IActivation>(parsedContent[1], out var hideView)) break;

                    hideView.Hide();
                    break;
                case DirectingActionType.SetAttitudeSprite:
                    if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<ISpriteSetter>(parsedContent[1], out var setAttitudeView)) break;

                    AttitudeType attitudeType = (AttitudeType)System.Enum.Parse(typeof(AttitudeType), parsedContent[2]);
                    setAttitudeView.SetAttitude(attitudeType);
                    break;
                case DirectingActionType.SetFaceSprite:
                    if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<ISpriteSetter>(parsedContent[1], out var setFaceView)) break;

                    FaceType faceType = (FaceType)System.Enum.Parse(typeof(FaceType), parsedContent[2]);
                    setFaceView.SetFace(faceType);
                    break;
                case DirectingActionType.SetPosition:
                    if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<IPositioner>(parsedContent[1], out var setPositionView)) break;
                    if (!this.DialogueViewPositioner.TryParsePosition(parsedContent[2], out var pos)) break;

                    setPositionView.DirectPosition(pos);
                    break;
                case DirectingActionType.Move:
                    if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<IPositioner>(parsedContent[1], out var setMoveView)) break;
                    if (!this.DialogueViewPositioner.TryParseMoveValue(parsedContent[2], out var parsedPositions, out var parsedDurations)) break;

                    behaviourToken = new BehaviourToken(isRequestEnd: false);
                    actionCoroutine = setMoveView.Move(parsedPositions, parsedDurations, behaviourToken);
                    break;
                case DirectingActionType.SetTextDisplay:
                    if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<ITextDisplayer>(parsedContent[1], out var setTextDisplayView)) break;
                    if (!this.DialogueViewTextParser.TryParseDirectingContent(parsedContent[2], out var spaker, out var content)) break;

                    behaviourToken = new BehaviourToken(isRequestEnd: false);
                    actionCoroutine = setTextDisplayView.TextDisplay(spaker, content, behaviourToken);
                    break;
                case DirectingActionType.SetButton:
                    if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<IChoiceButtonSetter>(parsedContent[1], out var setButtonView)) break;
                    if (!this.DialogueViewChoiceButtonParser.TryParseChoiceButtonContent(parsedContent[1], out var buttonKey, out var needConditions, out var buttonContent, out int nextBranchPoint)) break;

                    // 버튼 설정.
                    behaviourToken = new BehaviourToken(isRequestEnd: false);
                    actionCoroutine = setButtonView.OperateChoiceButtonDisplay(buttonContent, nextBranchPoint, behaviourToken);

                    // 조건이 들어간 버튼인 경우, 조건 설정.
                    if (needConditions != null)
                        setButtonView.SetCondition(needConditions);

                    break;
                case DirectingActionType.None:
                    break;
                default:
                    Debug.Log($"ActionType의 값이 잘못되었던가, enum으로 Parsing이 제대로 되지 않았음.");
                    break;
            }

            // 코루틴 동작이 필요한 경우, 코루틴 실행 후, 제어할 수 있도록 등록.
            if(actionCoroutine != null && behaviourToken != null)
            {
                DialogueDirectingCoroutineControlData newDirectingCoroutineControlData = new DialogueDirectingCoroutineControlData();
                this.DialogueDirectingControlData.RegisterCoroutineControlData(newDirectingCoroutineControlData);

                newDirectingCoroutineControlData.DirectingIndex = dialogueDirectingData.Index;
                newDirectingCoroutineControlData.BehaviourCoroutine = this.CoroutineRunner.Run(actionCoroutine);
                newDirectingCoroutineControlData.ControlCoroutine = this.CoroutineRunner.Run(this.OperateActionCoroutine(newDirectingCoroutineControlData.DirectingIndex));
                newDirectingCoroutineControlData.BehaviourToken = behaviourToken;
            }

            // 다음 연출 실행에 필요한 데이터 등록.
            this.DialogueDirectingControlData.LastDirectingData.Index = dialogueDirectingData.Index;
            this.DialogueDirectingControlData.LastDirectingData.IsSkipable = dialogueDirectingData.IsSkipable;
            this.DialogueDirectingControlData.LastDirectingData.IsAutoable = dialogueDirectingData.IsAutoable;
            this.DialogueDirectingControlData.LastDirectingData.NextDirectiveCommand = dialogueDirectingData.NextDirectiveCommand;

            // 연출을 이어서 재생하고 싶은 경우, 다음 연출도 실행 요청.
            if (dialogueDirectingData.IsChainWithNext)
            {
                if (this.TryGetNextDirectingIndex(dialogueDirectingData.NextDirectiveCommand, out var nextIndex))
                {
                    this.OperateDialogueDirecting(nextIndex);
                    return;
                }
            }
        }
        public bool TrySetSpeakerAndListenerColor(string directingContent)
        {
            string[] parsedContent = directingContent.Split('_');
            if (parsedContent.Length != 3) return false;

            string speakerKey = parsedContent[0];

            if (!this.DialogueViewObjectDataHandler.TryGetAllPlugIn<ISpriteSetter>(out var viewObjects)) return false;

            if (speakerKey.Equals("Player"))
            {
                foreach (var plugIn in viewObjects)
                {
                    plugIn.SetListenerColor();
                }
            }
            else
            {
                if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<ISpriteSetter>(speakerKey, out var viewObject)) return false;

                foreach (var plugIn in viewObjects)
                {
                    plugIn.SetListenerColor();
                }

                viewObject.SetSpeakerColor();
            }

            return true;
        }
        public bool TryUpdateChoiceButtonView(string selectedButtonKey)
        {
            if (!this.DialogueViewObjectDataHandler.TryGetAllPlugIn<IChoiceButtonSetter>(out var viewObjects)) return false;

            foreach (var buttonView in viewObjects)
            {
                buttonView.UpdateChoiceButtonView(selectedButtonKey);
            }

            return true;
        }






        public void OperateDialogueClickInteraction_ChoiceButton(int branchIndex)
        {
            this.OperateDialogueDirecting(branchIndex);
        }

        // Action 코루틴 제어
        private IEnumerator OperateActionCoroutine(int directingIndex)
        {
            if (!this.DialogueDirectingControlData.TryGetCoroutineControlData(directingIndex, out var dialogueDirectingCoroutineControlData)) yield break;

            yield return dialogueDirectingCoroutineControlData.BehaviourCoroutine;

            dialogueDirectingCoroutineControlData.BehaviourCoroutine = null;
            dialogueDirectingCoroutineControlData.ControlCoroutine = null;
            this.DialogueDirectingControlData.RemoveCoroutineControlData(dialogueDirectingCoroutineControlData);

            this.RequestNextDirecting();
        }
        // 1개의 연출 이후, 다음 연출을 이어서 수행할지 결정하는 부분.
        private void RequestNextDirecting()
        {
            // 자동 재생 가능한가? + Text 출력 작업 끝났는가? + Action 동작 끝났는가?
            if (this.DialogueDirectingControlData.LastDirectingData.IsAutoable && this.DialogueDirectingControlData.IsCoroutinesCompleted())
            {
                if (this.TryGetNextDirectingIndex(this.DialogueDirectingControlData.LastDirectingData.NextDirectiveCommand, out var index))
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

            switch (nextDirectiveCommandType)
            {
                case NextDirectiveCommandType.Next:
                    nextDirectingIndex = this.DialogueDirectingControlData.LastDirectingData.Index + 1;
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
            if (!this.DialogueDirectingControlData.LastDirectingData.IsSkipable) return;

            // 연출 중이 아니라면, 다음 시퀀스 스탭 시작.
            if (this.DialogueDirectingControlData.IsCoroutinesCompleted())
            {
                // 다음에 수행할 DirectingIndex 추출 확인.
                if (this.TryGetNextDirectingIndex(this.DialogueDirectingControlData.LastDirectingData.NextDirectiveCommand, out var index))
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
                if (this.DialogueDirectingControlData.IsCoroutinesCompleted())
                {
                    // 충분히 기다렸으면, 다음 대화 연출 수행 요청.
                    if (this.AutoPlayDirectingData.CurrentWaitDuration >= this.AutoPlayDirectingData.AutoWaitDuration)
                    {
                        if (this.TryGetNextDirectingIndex(this.DialogueDirectingControlData.LastDirectingData.NextDirectiveCommand, out var index))
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
            if (!this.DialogueDirectingControlData.LastDirectingData.IsSkipable) return;

            this.StopDialogueDirection();
        }


        // 진행 중인, 코루틴 동작이 있다면, 토큰을 통하여 자연스럽게 마무리가 되도록 유도한다.
        private void StopDialogueDirection()
        {
            foreach (var controlData in this.DialogueDirectingControlData.CoroutineControlDataList)
            {
                controlData.BehaviourToken.IsRequestEnd = true;
            }
        }
    }

    public class AutoDialogueDirectingData
    {
        public Coroutine AutoCoroutine { get; set; }

        public float AutoWaitDuration { get; set; }
        public float CurrentWaitDuration { get; set; }

        public bool IsRequestEnd { get; set; }

        public AutoDialogueDirectingData(float duration)
        {
            this.AutoWaitDuration = duration;
        }

        public void Reset()
        {
            this.AutoCoroutine = null;
            this.CurrentWaitDuration = 0f;
            this.IsRequestEnd = false;
        }
    }
}