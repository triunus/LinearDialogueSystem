using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameSystems.DTOs;
using GameSystems.Entities.MainStageScene;

namespace GameSystems.GameFlows.MainStageScene
{
    public class DialogueDirectingSystemGameFlow : MonoBehaviour, IGameFlow
    {
        // CanvasUIUX 기능 퍼사드.
        private DialogueCanvasUIUXVisualHub DialogueCanvasUIUXVisualHub;
        // BackGround 기능 퍼사드.
        private DialogueBackGroundVisualHub DialogueBackGroundVisualHub;
        // Actor 기능 퍼사드.
        private DialogueActorVisualHub DialogueActorVisualHub;

        // Text 출력 View
        private DialogueTextDisplayView DialogueTextDisplayView;

        // CanvasUIUX, BackGround, Actor의 Action 코루틴.
        private Coroutine ActionCoroutine;
        // Text 출력 코루틴.
        private Coroutine TextDisplayCoroutine;

        // 자동 재생 기능 코루틴.
        private Coroutine AutoCoroutine;
        private float AutoDuration;

        // 연출할 목록
        private List<SequenceStepData> SequenceStepDatas;
        // 연출을 스킵할 수 있는지 여부.
        private bool IsDialogueSkipable;
        // 현재 진행 중인 연출 Row.
        private SequenceStepData currentSequenceStepData;

        private void Awake()
        {
            var LocalRepository = Repository.MainStageSceneRepository.Instance;

            LocalRepository.GameFlow_LazyReferenceRepository.RegisterReference<DialogueDirectingSystemGameFlow>(this);

            this.DialogueCanvasUIUXVisualHub = LocalRepository.Entity_LazyReferenceRepository.
                GetOrWaitReference<DialogueCanvasUIUXVisualHub>(x => this.DialogueCanvasUIUXVisualHub = x);
            this.DialogueBackGroundVisualHub = LocalRepository.Entity_LazyReferenceRepository.
                GetOrWaitReference<DialogueBackGroundVisualHub>(x => this.DialogueBackGroundVisualHub = x);
            this.DialogueActorVisualHub = LocalRepository.Entity_LazyReferenceRepository.
                GetOrWaitReference<DialogueActorVisualHub>(x => this.DialogueActorVisualHub = x);

            this.DialogueTextDisplayView = LocalRepository.Entity_LazyReferenceRepository.
                GetOrWaitReference<DialogueTextDisplayView>(x => this.DialogueTextDisplayView = x);

            this.InitialSetting();
        }
        private void InitialSetting()
        {
            this.SetSequenceStepDatas
                (
                    new SequenceStepData[]
                    {
                        new SequenceStepData(0, SequenceDataType.CanvasUIUXActionType, false, true,
                            new ActionDirectionData("DefaultCutscene", ActionType.FadeIn, string.Empty, "1")),
                        new SequenceStepData(0, SequenceDataType.CanvasUIUXActionType, false, true,
                            new ActionDirectionData("DefaultTitle", ActionType.FadeIn, string.Empty, "1")),
                        new SequenceStepData(0, SequenceDataType.CanvasUIUXActionType, false, true,
                            new ActionDirectionData("DefaultTitle", ActionType.FadeOut, string.Empty, "1")),
                        new SequenceStepData(0, SequenceDataType.CanvasUIUXActionType, false, true,
                            new ActionDirectionData("DefaultCutscene", ActionType.FadeOut, string.Empty, "1")),

                        new SequenceStepData(0, SequenceDataType.BackGroundActionType, false, true,
                            new ActionDirectionData("DefaultBackGround", ActionType.FadeIn, string.Empty, "1")),

                        new SequenceStepData(0, SequenceDataType.ActorActionType, false, true,
                            new FaceDirectionData("DefaultCharacter01", FaceType.Default),  new ActionDirectionData("DefaultCharacter01", ActionType.FadeIn, "8", "1")),
                        new SequenceStepData(0, SequenceDataType.ActorActionType, false, true,
                            new FaceDirectionData("DefaultCharacter02", FaceType.Default),  new ActionDirectionData("DefaultCharacter02", ActionType.Move, "0_2", "1")),

                        new SequenceStepData(0, SequenceDataType.CanvasUIUXActionType, false, true,
                            new ActionDirectionData("DefaultTextPanel", ActionType.FadeIn, string.Empty, "1")),

                        new SequenceStepData(0, SequenceDataType.SayAndActorActionType, true, false,
                            new SayDirectionTextData("Charactor01", "안녕 Character01 이야."),
                            new FaceDirectionData("DefaultCharacter01", FaceType.Default),  new ActionDirectionData("DefaultCharacter01", ActionType.None, "0", "0")),
                        new SequenceStepData(0, SequenceDataType.SayAndActorActionType, true, false,
                            new SayDirectionTextData("Charactor02", "Hi Character02 이야."),
                            new FaceDirectionData("DefaultCharacter02", FaceType.Default),  new ActionDirectionData("DefaultCharacter02", ActionType.None, "0", "0")),
                        new SequenceStepData(0, SequenceDataType.SayAndActorActionType, false, true,
                            new SayDirectionTextData("Charactor02", "난 먼저 갈겡."),
                            new FaceDirectionData("DefaultCharacter02", FaceType.Default),  new ActionDirectionData("DefaultCharacter02", ActionType.Move, "2_0", "1")),
                        new SequenceStepData(0, SequenceDataType.ActorActionType, false, true,
                            new FaceDirectionData("DefaultCharacter02", FaceType.Default),  new ActionDirectionData("DefaultCharacter02", ActionType.FadeOut, "0", "1")),

                        new SequenceStepData(0, SequenceDataType.SayAndActorActionType, false, true,
                            new SayDirectionTextData("Charactor01", "같이가 ㅠㅠ"),
                            new FaceDirectionData("DefaultCharacter01", FaceType.Default),  new ActionDirectionData("DefaultCharacter01", ActionType.Move, "8_0", "1")),
                        new SequenceStepData(0, SequenceDataType.ActorActionType, false, true,
                            new FaceDirectionData("DefaultCharacter01", FaceType.Default),  new ActionDirectionData("DefaultCharacter01", ActionType.FadeOut, "0", "1")),


                        new SequenceStepData(0, SequenceDataType.CanvasUIUXActionType, false, true,
                            new ActionDirectionData("DefaultTextPanel", ActionType.FadeOut, string.Empty, "1")),
                        new SequenceStepData(0, SequenceDataType.BackGroundActionType, false, true,
                            new ActionDirectionData("DefaultBackGround", ActionType.FadeOut, string.Empty, "1")),
                    },
                    false
                );
        }

        public void SetSequenceStepDatas(SequenceStepData[] sequenceStepDatas, bool isDialogueSkipable)
        {
            this.SequenceStepDatas = new(sequenceStepDatas);
            this.IsDialogueSkipable = isDialogueSkipable;
        }
        public bool IsDialogueSystemVaild()
        {
            if (this.SequenceStepDatas == null || this.SequenceStepDatas.Count <= 0) return false;
            else return true;
        }

        public void TestButtonOperation()
        {
            this.StartDialogueDirectingSystem();
        }

        public void StartDialogueDirectingSystem()
        {

            if (!(this.TextDisplayCoroutine == null && this.ActionCoroutine == null)) return;

            if (this.SequenceStepDatas.Count <= 0)
                this.EndDialogueDirectingSystem();

            // 현재 멤버로 등록.
            this.currentSequenceStepData = this.SequenceStepDatas[0];
            // 수행 목록에서 삭제.
            this.SequenceStepDatas.RemoveAt(0);

            switch (this.currentSequenceStepData.SequenceDataType)
            {
                case SequenceDataType.BackGroundActionType:
                    if (this.DialogueBackGroundVisualHub.TryGetAction(this.currentSequenceStepData.ActionDirectionData, out var backGroundCo) &&
                        backGroundCo != null) this.ActionCoroutine = StartCoroutine(this.OperateActionCoroutine(backGroundCo));
                    break;
                case SequenceDataType.CanvasUIUXActionType:
                    if (this.DialogueCanvasUIUXVisualHub.TryGetAction(this.currentSequenceStepData.ActionDirectionData, out var canvasUIUXCo) &&
                        canvasUIUXCo != null) this.ActionCoroutine = StartCoroutine(this.OperateActionCoroutine(canvasUIUXCo));
                    break;
                case SequenceDataType.ActorActionType:
                    if(this.DialogueActorVisualHub.TrySetFace(this.currentSequenceStepData.FaceDirectionData) &&
                        this.DialogueActorVisualHub.TryGetAction(this.currentSequenceStepData.ActionDirectionData, out var actorCo01) &&
                        actorCo01 != null) this.ActionCoroutine = StartCoroutine(this.OperateActionCoroutine(actorCo01));
                    break;
                case SequenceDataType.SayAndActorActionType:
                    if (this.DialogueActorVisualHub.TrySetFace(this.currentSequenceStepData.FaceDirectionData) &&
                        this.DialogueActorVisualHub.TryGetAction(this.currentSequenceStepData.ActionDirectionData, out var actorCo02) &&
                        this.DialogueTextDisplayView.TryGetTextDisplayOperation(this.currentSequenceStepData.SayDirectionTextData, out var textDisplayCo) &&
                        textDisplayCo != null)
                    {
                        if(actorCo02 != null)
                        {
                            this.ActionCoroutine = StartCoroutine(this.OperateActionCoroutine(actorCo02));
                        }

                        this.TextDisplayCoroutine = StartCoroutine(this.OperateTextDisplayCoroutine(textDisplayCo));
                    }
                    break;
                default:
                    // 뭔가 오류가 들어온건데, 한번 더 반복. ( 언젠가 SequenceStepDatas <= 0이 되서 종료됨 )
                    this.StartDialogueDirectingSystem();
                    break;
            }
        }
        public void StopDialogueDirection()
        {
            if (this.currentSequenceStepData == null) return;

            switch (this.currentSequenceStepData.SequenceDataType)
            {
                case SequenceDataType.BackGroundActionType:
                    this.StopCoroutine(this.ActionCoroutine);
                    this.ActionCoroutine = null;
                    break;
                case SequenceDataType.CanvasUIUXActionType:
                    this.StopCoroutine(this.ActionCoroutine);
                    this.ActionCoroutine = null;
                    break;
                case SequenceDataType.ActorActionType:
                    this.StopCoroutine(this.ActionCoroutine);
                    this.ActionCoroutine = null;
                    break;
                case SequenceDataType.SayAndActorActionType:
                    this.StopCoroutine(this.ActionCoroutine);
                    this.StopCoroutine(this.TextDisplayCoroutine);
                    this.ActionCoroutine = null;
                    this.TextDisplayCoroutine = null;
                    break;
                default:
                    break;
            }
        }
        public void EndDialogueDirectingSystem()
        {
            Debug.Log($"종료");
        }

        public void OnClicked_Mouse()
        {
            // 해당 대화 연출 스킵 가능한지 확인.
            if (this.currentSequenceStepData.IsSkipable)
            {
                // Type 중이 아니라면, 다음 시퀀스 스탭 시작.
                if (this.ActionCoroutine == null && this.TextDisplayCoroutine == null)
                {
                    this.StartDialogueDirectingSystem();
                }
                // Type 중이라면, 중지 및 전부 출력.
                else
                {
                    this.StopDialogueDirection();
                }
            }
        }

        public void OnClickedLogButton()
        {
            //            this.DialogueMediatorView.Dialogue_LogPanel_Activation(true);
        }
        public void OnClicked_ExitLogButton()
        {
            //            this.DialogueMediatorView.Dialogue_LogPanel_Activation(false);
        }

        public void OnClickedAutoButton()
        {
            if (this.AutoCoroutine == null)
            {
                this.AutoCoroutine = StartCoroutine(this.OperateDialogueAutoDisplay());
            }
            else
            {
                StopCoroutine(this.AutoCoroutine);
                this.AutoCoroutine = null;
            }
        }
        private IEnumerator OperateDialogueAutoDisplay()
        {
            float waitedTime = 0;

            while (true)
            {
                //  현재 재생 중인 코루틴이 있으면 그냥 넘어감.
                if(this.ActionCoroutine != null || this.TextDisplayCoroutine != null)
                {
                    yield return Time.deltaTime;
                }
                else
                {
                    // 충분히 기다렸으면, 다음 대화 연출 수행 요청.
                    if (waitedTime >= this.AutoDuration)
                    {
                        this.StartDialogueDirectingSystem();

                    }
                    // 현재 재생 중인 코루틴이 없으면 시간 증가.
                    else
                    {
                        waitedTime += Time.deltaTime;
                    }
                }
            }
        }
        public void OnClickedSkipButton()
        {
            Debug.Log($"OnClickedSkipButton - IsDialogueSkipable : {this.IsDialogueSkipable}");

            // 스킵 허용 안할시 넘어감.
            if (!this.IsDialogueSkipable) return;

            this.StopDialogueDirection();
        }

        public IEnumerator OperateActionCoroutine(IEnumerator actionOperation)
        {
            yield return StartCoroutine(actionOperation);

            this.ActionCoroutine = null;

            if (this.currentSequenceStepData.IsAutoable && this.TextDisplayCoroutine == null)
                this.StartDialogueDirectingSystem();
        }
        public IEnumerator OperateTextDisplayCoroutine(IEnumerator textDisplayOperation)
        {
            yield return StartCoroutine(textDisplayOperation);

            this.TextDisplayCoroutine = null;

            if (this.currentSequenceStepData.IsAutoable && this.ActionCoroutine == null)
                this.StartDialogueDirectingSystem();
        }
    }



/*    public interface IDialogueSystemGameFlow
    {
        public void OnClicked_Mouse();
        public void OnClickedLogButton();
        public void OnClickedAutoButton();
        public void OnClickedSkipButton();

        public void OnClicked_ExitLogButton();
    }

    public class DialogueSystemGameFlow : MonoBehaviour, IGameFlow, IDialogueSystemGameFlow
    {
        // Cutscene 관련 제어.
        private IDialogueCutsceneMediatorView DialogueCutsceneMediatorView;
        // Default UIUX 관련 제어.
        private IDialogueDefaultMediatorView DialogueDefaultMediatorView;
        // Text관련 제어
        private IDialogueTextMediatorView DialogueTextMediatorView;
        // 일러스트 관련 제어
        private IDialogueIllustMediatorView DialogueIllustMediatorView;

        private Coroutine DialogueMainFlow;
        private Coroutine sequenceCoroutine;

        private Coroutine autoCoroutine;

        // 값이 클수록 출력 속도가 빨라짐.
        [SerializeField] private float charsPerSecond = 20f; // 초당 몇 글자
        [SerializeField] private float AutoDuration = 2f;


        // 테스트 중에 사용하는 값들.
        [SerializeField] private UnityEngine.UI.Button test;

        public List<SequenceStepData> sequenceStepDatas = new();
        private SequenceStepData currentSequenceStepData;

        private void Awake()
        {
            var LocalRepository = Repository.MainStageSceneRepository.Instance;
            var GlobalRepository = Repository.GlobalSceneRepository.Instance;

            // 등록
            LocalRepository.GameFlow_LazyReferenceRepository.RegisterReference<DialogueSystemGameFlow>(this);

            // 참조
            // Cutscene 관련
            this.DialogueCutsceneMediatorView = LocalRepository.
                Entity_LazyReferenceRepository.GetOrWaitReference<DialogueCutsceneMediatorView>(x => this.DialogueCutsceneMediatorView = x);
            // Default UIUX 관련
            this.DialogueDefaultMediatorView = LocalRepository.
                Entity_LazyReferenceRepository.GetOrWaitReference<DialogueDefaultMediatorView>(x => this.DialogueDefaultMediatorView = x);
            // Text 관련
            this.DialogueTextMediatorView = LocalRepository.
                Entity_LazyReferenceRepository.GetOrWaitReference<DialogueTextMediatorView>(x => this.DialogueTextMediatorView = x);
            // 일러스트 관련.
            this.DialogueIllustMediatorView = LocalRepository.
                Entity_LazyReferenceRepository.GetOrWaitReference<DialogueIllustMediatorView>(x => this.DialogueIllustMediatorView = x);

            test.onClick.AddListener(this.OperateDialogueSystem);
            this.TemporaryScriptsSetting();
        }

        private void TemporaryScriptsSetting()
        {
            this.SequenceSteps.Enqueue(
                new SequenceStep(index: 0, sequenceStepType: SequenceStepType.UIUXAction,
                targetName: new string[] { "TitleBackGroundUIUX" },
                actionType: ActionType.FadeIn, waitTime: 1f,
                isSkipable: false, isAutoable: true)
                );
            this.SequenceSteps.Enqueue(
                new SequenceStep(index: 1, sequenceStepType: SequenceStepType.UIUXAction,
                targetName: new string[] { "TitleUIUX" },
                actionType: ActionType.FadeIn, waitTime: 1f,
                isSkipable: false, isAutoable: true)
                );
            this.SequenceSteps.Enqueue(
                new SequenceStep(index: 1, sequenceStepType: SequenceStepType.UIUXAction,
                targetName: new string[] { "TitleBackGroundUIUX", "TitleUIUX" },
                actionType: ActionType.FadeOut, waitTime: 1f,
                isSkipable: false, isAutoable: true)
                );

            this.SequenceSteps.Enqueue(
                new SequenceStep(index : 0, sequenceStepType : SequenceStepType.ActorAction,
                targetName: new string[] { "Illust0000" }, faceNumber : 0,
                actionType : ActionType.FadeIn, positionLayer : 2, waitTime : 1f,
                isSkipable : false, isAutoable : true)
                );
            this.SequenceSteps.Enqueue(
                new SequenceStep(index: 1, sequenceStepType: SequenceStepType.ActorAction,
                targetName: new string[] { "Illust0001" }, faceNumber: 0,
                actionType: ActionType.DirectShow, positionLayer: 10, waitTime: 0.5f,
                isSkipable: false, isAutoable: true)
                );
            this.SequenceSteps.Enqueue(
                new SequenceStep(index: 2, sequenceStepType: SequenceStepType.ActorAction,
                targetName: new string[] { "Illust0001" }, faceNumber: 0,
                actionType: ActionType.Move, positionLayer: 10_8, waitTime: 0.5f,
                isSkipable: false, isAutoable: true)
                );

            this.SequenceSteps.Enqueue(
                new SequenceStep(index : 3, sequenceStepType : SequenceStepType.UIUXAction,
                targetName: new string[] { "TextUIUX" },
                actionType : ActionType.FadeIn, waitTime : 1f,
                isSkipable : false, isAutoable :  true)
                );

            this.SequenceSteps.Enqueue(
                new SequenceStep(index: 4, sequenceStepType: SequenceStepType.Say,
                targetName: new string[] { "Illust0000" }, faceNumber: 0,
                speaker : "Actor01", content : "안녕하세요! 임시 대화입니다. 일정 속도로 한 글자씩 나타나요 :)",
                isSkipable: true, isAutoable: false)
                );
            this.SequenceSteps.Enqueue(
                new SequenceStep(index: 5, sequenceStepType: SequenceStepType.Say,
                targetName: new string[] { "Illust0001" }, faceNumber: 0,
                speaker : "Actor02", content : "안녕하세요! 두 번째 임시 대화입니다.:)",
                isSkipable: true, isAutoable: false)
                );
            this.SequenceSteps.Enqueue(
                new SequenceStep(index: 6, sequenceStepType: SequenceStepType.Say,
                targetName: new string[] { "Illust0001" }, faceNumber: 0,
                speaker : "Actor02", content : "자 이제 사라져 볼게.",
                isSkipable: true, isAutoable: false)
                );
            this.SequenceSteps.Enqueue(
                new SequenceStep(index: 7, sequenceStepType: SequenceStepType.Say,
                 targetName: new string[] { }, faceNumber: 0,
                speaker : string.Empty, content : string.Empty,
                isSkipable: true, isAutoable: true)
                );

            this.SequenceSteps.Enqueue(
                new SequenceStep(index: 0, sequenceStepType: SequenceStepType.ActorAction,
                targetName: new string[] { "Illust0000" }, faceNumber: 0,
                actionType: ActionType.Move, positionLayer: 2_0, waitTime: 1f,
                isSkipable: false, isAutoable: true)
                );
            this.SequenceSteps.Enqueue(
                new SequenceStep(index: 0, sequenceStepType: SequenceStepType.ActorAction,
                targetName: new string[] { "Illust0000" }, faceNumber: 0,
                actionType: ActionType.DirectHide, positionLayer: 0, waitTime: 1f,
                isSkipable: false, isAutoable: true)
                );
            this.SequenceSteps.Enqueue(
                new SequenceStep(index: 0, sequenceStepType: SequenceStepType.ActorAction,
                targetName: new string[] { "Illust0001" }, faceNumber: 0,
                actionType: ActionType.FadeOut, positionLayer: 8, waitTime: 1f,
                isSkipable: false, isAutoable: true)
                );

            this.SequenceSteps.Enqueue(
                new SequenceStep(index: 3, sequenceStepType: SequenceStepType.UIUXAction,
                targetName: new string[] { "TextUIUX" },
                actionType: ActionType.FadeOut, waitTime: 1f,
                isSkipable: false, isAutoable: true)
                );
        }

        // DialogueSystem 시작.
        public void OperateDialogueSystem()
        {
            if (this.DialogueMainFlow != null) return;

            if (this.sequenceStepDatas.Count <= 0)
            {
                Debug.Log($"대화 Sequence 다 끝남.");
                return;
            }

            this.currentSequenceStepData = this.sequenceStepDatas[0];
            this.sequenceStepDatas.RemoveAt(0);

            switch (this.currentSequenceStepData.SequenceDataType)
            {
                case SequenceDataType.ConversationDirectionType:
                    this.DialogueMainFlow = StartCoroutine();
                    break;
                case SequenceDataType.UIUXActionDirectionType:
                    break;
                case SequenceDataType.ActorActionDirectionType:
                    break;
                case SequenceDataType.IllustActionType:
                    break;
                case SequenceDataType.ForkDirectionType:
                    break;
            }
        }

        private IEnumerator DialogueOperation()
        {
            // Cutscene 흐름 실행.
            yield return StartCoroutine(this.CutsceneFlowOperation(1f, 1f, 1f, 1f, 0.5f));  

            // 잠시 대기
            yield return new WaitForSeconds(0.2f);

            // 대화 시스템을 위한 Default UIUX 출력 Flow
            yield return StartCoroutine(this.DialogueFlowStartOperation());

            // 잠시 대기
            yield return new WaitForSeconds(0.1f);

            this.DialogueSequenceSetpOperation();

            this.DialogueMainFlow = null;
        }

        // Cutscene Flow
        private IEnumerator CutsceneFlowOperation(float backGroundFadeOutDuration, float titleFadeOutDuration, float waitDuration, float backGroundFadeInDuration, float titleFadeInDuration)
        {
            // 검은 화면 FadeIn
            yield return StartCoroutine(this.DialogueCutsceneMediatorView.FadeIn_CutsceneBackGround(backGroundFadeOutDuration));
            // Title FadeIn
            yield return StartCoroutine(this.DialogueCutsceneMediatorView.FadeIn_CutsceneTitle(titleFadeOutDuration));

            // 잠시 대기
            yield return new WaitForSeconds(waitDuration);

            // 검은 화면과 Title FadeOut
            StartCoroutine(this.DialogueCutsceneMediatorView.FadeOut_CutsceneBackGround(backGroundFadeInDuration));
            StartCoroutine(this.DialogueCutsceneMediatorView.FadeOut_CutsceneTitle(titleFadeInDuration));
            yield return new WaitForSeconds(Mathf.Max(backGroundFadeInDuration, titleFadeInDuration));
        }
        private IEnumerator DialogueFlowStartOperation()
        {
            // 대화 시스템을 위한 Default UIUX 출력 Flow
            yield return StartCoroutine(this.DialogueDefaultMediatorView.DefaultUIUX_FadeIn(1f));
        }
        private IEnumerator DialogueFlowEndOperation()
        {
            // 대화 시스템을 위한 Default UIUX 비출력 Flow
            yield return StartCoroutine(this.DialogueDefaultMediatorView.DefaultUIUX_FadeOut(1f));
        }

        // 대화 시스템 진행.
        // 연출형태에 따라서, 다른 코루틴 실행.
        private void DialogueSequenceSetpOperation()
        {
            if (this.SequenceSteps.Count <= 0)
            {
                StartCoroutine(this.DialogueFlowEndOperation());
                return;
            }

            // 다음 문자열 시행.
            this.currentSequenceStep = this.SequenceSteps.Dequeue();

            switch (this.currentSequenceStep.SequenceStepType)
            {
                case SequenceStepType.UIUXAction:
                    this.sequenceCoroutine = StartCoroutine(this.OperateDialogueTextUIUXAction());
                    break;
                case SequenceStepType.Say:
                    this.DialogueIllustMediatorView.SetSpeakerColor(this.currentSequenceStep.TargetName);
                    this.DialogueTextMediatorView.UpdateSpeaker(this.currentSequenceStep.Speaker);
                    this.sequenceCoroutine = StartCoroutine(this.OperateDialogueTextDisplay(this.currentSequenceStep.Content));
                    break;
                case SequenceStepType.ActorAction:
                    this.DialogueIllustMediatorView.SetSpeakerColor(this.currentSequenceStep.TargetName);
                    this.DialogueTextMediatorView.UpdateSpeaker(this.currentSequenceStep.Speaker);
                    this.sequenceCoroutine = StartCoroutine(this.OperateDialogueIllustAction());
                    break;
                default:
                    break;
            }
        }

        public void OnClicked_Mouse()
        {
            // 애니메이션 작동이 들어가는 경우, Skipable이 false가 됩니다. 즉 마우스 입력을 무시합니다.
            // skip이 가능한 경우, 코루틴을 중지하고 대사를 끝까지 출력해 줍니다.
            // 코루틴이 이미 중지된 상태라면, 다음 Sequence를 수행합니다.
            if (this.currentSequenceStep.IsSkipable)
            {
                // Type 중이 아니라면, 다음 시퀀스 스탭 시작.
                if (this.sequenceCoroutine == null)
                {
                    this.DialogueSequenceSetpOperation();
                }
                // Type 중이라면, 중지 및 전부 출력.
                else
                {
                    StopCoroutine(this.sequenceCoroutine);

                    this.DialogueTextMediatorView.UpdateContentText(this.currentSequenceStep.Content);
                    this.currentSequenceStep = null;
                    this.sequenceCoroutine = null;
                }
            }
        }

        public void OnClickedLogButton()
        {
//            this.DialogueMediatorView.Dialogue_LogPanel_Activation(true);
        }
        public void OnClicked_ExitLogButton()
        {
//            this.DialogueMediatorView.Dialogue_LogPanel_Activation(false);
        }

        public void OnClickedAutoButton()
        {
            if (this.autoCoroutine == null)
            {
                this.autoCoroutine = StartCoroutine(this.OperateDialogueAutoDisplay());
            }
            else
            {
                StopCoroutine(this.autoCoroutine);
                this.autoCoroutine = null;
            }
        }
        public void OnClickedSkipButton()
        {
            Debug.Log($"OnClickedSkipButton");

            // 여기서 일러스트, TextUIUX도 비활성화 해야 됨.   
            StartCoroutine(this.DialogueFlowEndOperation());

            this.DialogueMainFlow = null;
            this.sequenceCoroutine = null;
            this.currentSequenceStep = null;
            
            // 다른 시스템으로 넘어가도록 명시.
        }

        private IEnumerator OperateDialogueAutoDisplay()
        {
            float waitedTime = 0;

            while (true)
            {
                // 충분히 기다렸으면, 다음 문자열 출력 코루틴 실행 요청.
                if (waitedTime >= this.AutoDuration)
                {
                    // 다음 문자열이 없으면, break
                    if (this.SequenceSteps.Count <= 0) break;

                    this.DialogueSequenceSetpOperation();
                }

                // 현재 재생 중인 글자 출력 코루틴이 없으면, 대기시간 증가.
                if (this.sequenceCoroutine == null)
                {
                    waitedTime += Time.deltaTime;
                }

                yield return Time.deltaTime;
            }

            this.autoCoroutine = null;
        }

        private IEnumerator OperateDialogueTextUIUXAction()
        {
            switch (this.currentSequenceStep.ActionType)
            {
                case ActionType.TextUIUX_FadeIn:
                    yield return StartCoroutine(this.DialogueTextMediatorView.TextUIUX_FadeIn(1f));
                    break;
                case ActionType.TextUIUX_FadeOut:
                    yield return StartCoroutine(this.DialogueTextMediatorView.TextUIUX_FadeOut(1f));
                    break;
                case ActionType.TextUIUX_DirectShow:
                    this.DialogueTextMediatorView.Show();
                    break;
                case ActionType.TextUIUX_DirectHide:
                    this.DialogueTextMediatorView.Hide();
                    break;
                default:
                    break;
            }

            this.sequenceCoroutine = null;

            this.DialogueSequenceSetpOperation();
        }
        private IEnumerator OperateDialogueTextDisplay(string content)
        {
            // 출력 기능을 담당하는 View 참조가 없으면 return.
            if (this.DialogueTextMediatorView == null) yield break;

            // View한테 Text 초기화 수행 요청.
            this.DialogueTextMediatorView.ClearText();

            // 안전장치: cps가 0 이하이면 즉시 출력
            if (charsPerSecond <= 0f)
            {
                this.DialogueTextMediatorView.UpdateContentText(content);
                yield break;
            }

            float delay = 1f / charsPerSecond;
            WaitForSeconds wait = new WaitForSeconds(delay);

            // 가장 단순한 방식: 글자 하나씩 붙이기
            for (int i = 0; i < content.Length; i++)
            {
                this.DialogueTextMediatorView.AddText(content[i]);
                yield return wait;
            }

            this.sequenceCoroutine = null;
        }
        private IEnumerator OperateDialogueIllustAction()
        {
            yield return StartCoroutine(this.DialogueIllustMediatorView.IllustAction(this.currentSequenceStep.TargetName, this.currentSequenceStep.ActionType, 1f));

            this.sequenceCoroutine = null;

            this.DialogueSequenceSetpOperation();
        }
    }*/
}