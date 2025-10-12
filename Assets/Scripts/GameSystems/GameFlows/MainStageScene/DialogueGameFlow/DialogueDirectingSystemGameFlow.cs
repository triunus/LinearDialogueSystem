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
        // CanvasUIUX ��� �ۻ��.
        private DialogueCanvasUIUXVisualHub DialogueCanvasUIUXVisualHub;
        // BackGround ��� �ۻ��.
        private DialogueBackGroundVisualHub DialogueBackGroundVisualHub;
        // Actor ��� �ۻ��.
        private DialogueActorVisualHub DialogueActorVisualHub;

        // Text ��� View
        private DialogueTextDisplayView DialogueTextDisplayView;

        // CanvasUIUX, BackGround, Actor�� Action �ڷ�ƾ.
        private Coroutine ActionCoroutine;
        // Text ��� �ڷ�ƾ.
        private Coroutine TextDisplayCoroutine;

        // �ڵ� ��� ��� �ڷ�ƾ.
        private Coroutine AutoCoroutine;
        private float AutoDuration;

        // ������ ���
        private List<SequenceStepData> SequenceStepDatas;
        // ������ ��ŵ�� �� �ִ��� ����.
        private bool IsDialogueSkipable;
        // ���� ���� ���� ���� Row.
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
                            new SayDirectionTextData("Charactor01", "�ȳ� Character01 �̾�."),
                            new FaceDirectionData("DefaultCharacter01", FaceType.Default),  new ActionDirectionData("DefaultCharacter01", ActionType.None, "0", "0")),
                        new SequenceStepData(0, SequenceDataType.SayAndActorActionType, true, false,
                            new SayDirectionTextData("Charactor02", "Hi Character02 �̾�."),
                            new FaceDirectionData("DefaultCharacter02", FaceType.Default),  new ActionDirectionData("DefaultCharacter02", ActionType.None, "0", "0")),
                        new SequenceStepData(0, SequenceDataType.SayAndActorActionType, false, true,
                            new SayDirectionTextData("Charactor02", "�� ���� ����."),
                            new FaceDirectionData("DefaultCharacter02", FaceType.Default),  new ActionDirectionData("DefaultCharacter02", ActionType.Move, "2_0", "1")),
                        new SequenceStepData(0, SequenceDataType.ActorActionType, false, true,
                            new FaceDirectionData("DefaultCharacter02", FaceType.Default),  new ActionDirectionData("DefaultCharacter02", ActionType.FadeOut, "0", "1")),

                        new SequenceStepData(0, SequenceDataType.SayAndActorActionType, false, true,
                            new SayDirectionTextData("Charactor01", "���̰� �Ф�"),
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

            // ���� ����� ���.
            this.currentSequenceStepData = this.SequenceStepDatas[0];
            // ���� ��Ͽ��� ����.
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
                    // ���� ������ ���°ǵ�, �ѹ� �� �ݺ�. ( ������ SequenceStepDatas <= 0�� �Ǽ� ����� )
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
            Debug.Log($"����");
        }

        public void OnClicked_Mouse()
        {
            // �ش� ��ȭ ���� ��ŵ �������� Ȯ��.
            if (this.currentSequenceStepData.IsSkipable)
            {
                // Type ���� �ƴ϶��, ���� ������ ���� ����.
                if (this.ActionCoroutine == null && this.TextDisplayCoroutine == null)
                {
                    this.StartDialogueDirectingSystem();
                }
                // Type ���̶��, ���� �� ���� ���.
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
                //  ���� ��� ���� �ڷ�ƾ�� ������ �׳� �Ѿ.
                if(this.ActionCoroutine != null || this.TextDisplayCoroutine != null)
                {
                    yield return Time.deltaTime;
                }
                else
                {
                    // ����� ��ٷ�����, ���� ��ȭ ���� ���� ��û.
                    if (waitedTime >= this.AutoDuration)
                    {
                        this.StartDialogueDirectingSystem();

                    }
                    // ���� ��� ���� �ڷ�ƾ�� ������ �ð� ����.
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

            // ��ŵ ��� ���ҽ� �Ѿ.
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
        // Cutscene ���� ����.
        private IDialogueCutsceneMediatorView DialogueCutsceneMediatorView;
        // Default UIUX ���� ����.
        private IDialogueDefaultMediatorView DialogueDefaultMediatorView;
        // Text���� ����
        private IDialogueTextMediatorView DialogueTextMediatorView;
        // �Ϸ���Ʈ ���� ����
        private IDialogueIllustMediatorView DialogueIllustMediatorView;

        private Coroutine DialogueMainFlow;
        private Coroutine sequenceCoroutine;

        private Coroutine autoCoroutine;

        // ���� Ŭ���� ��� �ӵ��� ������.
        [SerializeField] private float charsPerSecond = 20f; // �ʴ� �� ����
        [SerializeField] private float AutoDuration = 2f;


        // �׽�Ʈ �߿� ����ϴ� ����.
        [SerializeField] private UnityEngine.UI.Button test;

        public List<SequenceStepData> sequenceStepDatas = new();
        private SequenceStepData currentSequenceStepData;

        private void Awake()
        {
            var LocalRepository = Repository.MainStageSceneRepository.Instance;
            var GlobalRepository = Repository.GlobalSceneRepository.Instance;

            // ���
            LocalRepository.GameFlow_LazyReferenceRepository.RegisterReference<DialogueSystemGameFlow>(this);

            // ����
            // Cutscene ����
            this.DialogueCutsceneMediatorView = LocalRepository.
                Entity_LazyReferenceRepository.GetOrWaitReference<DialogueCutsceneMediatorView>(x => this.DialogueCutsceneMediatorView = x);
            // Default UIUX ����
            this.DialogueDefaultMediatorView = LocalRepository.
                Entity_LazyReferenceRepository.GetOrWaitReference<DialogueDefaultMediatorView>(x => this.DialogueDefaultMediatorView = x);
            // Text ����
            this.DialogueTextMediatorView = LocalRepository.
                Entity_LazyReferenceRepository.GetOrWaitReference<DialogueTextMediatorView>(x => this.DialogueTextMediatorView = x);
            // �Ϸ���Ʈ ����.
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
                speaker : "Actor01", content : "�ȳ��ϼ���! �ӽ� ��ȭ�Դϴ�. ���� �ӵ��� �� ���ھ� ��Ÿ���� :)",
                isSkipable: true, isAutoable: false)
                );
            this.SequenceSteps.Enqueue(
                new SequenceStep(index: 5, sequenceStepType: SequenceStepType.Say,
                targetName: new string[] { "Illust0001" }, faceNumber: 0,
                speaker : "Actor02", content : "�ȳ��ϼ���! �� ��° �ӽ� ��ȭ�Դϴ�.:)",
                isSkipable: true, isAutoable: false)
                );
            this.SequenceSteps.Enqueue(
                new SequenceStep(index: 6, sequenceStepType: SequenceStepType.Say,
                targetName: new string[] { "Illust0001" }, faceNumber: 0,
                speaker : "Actor02", content : "�� ���� ����� ����.",
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

        // DialogueSystem ����.
        public void OperateDialogueSystem()
        {
            if (this.DialogueMainFlow != null) return;

            if (this.sequenceStepDatas.Count <= 0)
            {
                Debug.Log($"��ȭ Sequence �� ����.");
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
            // Cutscene �帧 ����.
            yield return StartCoroutine(this.CutsceneFlowOperation(1f, 1f, 1f, 1f, 0.5f));  

            // ��� ���
            yield return new WaitForSeconds(0.2f);

            // ��ȭ �ý����� ���� Default UIUX ��� Flow
            yield return StartCoroutine(this.DialogueFlowStartOperation());

            // ��� ���
            yield return new WaitForSeconds(0.1f);

            this.DialogueSequenceSetpOperation();

            this.DialogueMainFlow = null;
        }

        // Cutscene Flow
        private IEnumerator CutsceneFlowOperation(float backGroundFadeOutDuration, float titleFadeOutDuration, float waitDuration, float backGroundFadeInDuration, float titleFadeInDuration)
        {
            // ���� ȭ�� FadeIn
            yield return StartCoroutine(this.DialogueCutsceneMediatorView.FadeIn_CutsceneBackGround(backGroundFadeOutDuration));
            // Title FadeIn
            yield return StartCoroutine(this.DialogueCutsceneMediatorView.FadeIn_CutsceneTitle(titleFadeOutDuration));

            // ��� ���
            yield return new WaitForSeconds(waitDuration);

            // ���� ȭ��� Title FadeOut
            StartCoroutine(this.DialogueCutsceneMediatorView.FadeOut_CutsceneBackGround(backGroundFadeInDuration));
            StartCoroutine(this.DialogueCutsceneMediatorView.FadeOut_CutsceneTitle(titleFadeInDuration));
            yield return new WaitForSeconds(Mathf.Max(backGroundFadeInDuration, titleFadeInDuration));
        }
        private IEnumerator DialogueFlowStartOperation()
        {
            // ��ȭ �ý����� ���� Default UIUX ��� Flow
            yield return StartCoroutine(this.DialogueDefaultMediatorView.DefaultUIUX_FadeIn(1f));
        }
        private IEnumerator DialogueFlowEndOperation()
        {
            // ��ȭ �ý����� ���� Default UIUX ����� Flow
            yield return StartCoroutine(this.DialogueDefaultMediatorView.DefaultUIUX_FadeOut(1f));
        }

        // ��ȭ �ý��� ����.
        // �������¿� ����, �ٸ� �ڷ�ƾ ����.
        private void DialogueSequenceSetpOperation()
        {
            if (this.SequenceSteps.Count <= 0)
            {
                StartCoroutine(this.DialogueFlowEndOperation());
                return;
            }

            // ���� ���ڿ� ����.
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
            // �ִϸ��̼� �۵��� ���� ���, Skipable�� false�� �˴ϴ�. �� ���콺 �Է��� �����մϴ�.
            // skip�� ������ ���, �ڷ�ƾ�� �����ϰ� ��縦 ������ ����� �ݴϴ�.
            // �ڷ�ƾ�� �̹� ������ ���¶��, ���� Sequence�� �����մϴ�.
            if (this.currentSequenceStep.IsSkipable)
            {
                // Type ���� �ƴ϶��, ���� ������ ���� ����.
                if (this.sequenceCoroutine == null)
                {
                    this.DialogueSequenceSetpOperation();
                }
                // Type ���̶��, ���� �� ���� ���.
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

            // ���⼭ �Ϸ���Ʈ, TextUIUX�� ��Ȱ��ȭ �ؾ� ��.   
            StartCoroutine(this.DialogueFlowEndOperation());

            this.DialogueMainFlow = null;
            this.sequenceCoroutine = null;
            this.currentSequenceStep = null;
            
            // �ٸ� �ý������� �Ѿ���� ���.
        }

        private IEnumerator OperateDialogueAutoDisplay()
        {
            float waitedTime = 0;

            while (true)
            {
                // ����� ��ٷ�����, ���� ���ڿ� ��� �ڷ�ƾ ���� ��û.
                if (waitedTime >= this.AutoDuration)
                {
                    // ���� ���ڿ��� ������, break
                    if (this.SequenceSteps.Count <= 0) break;

                    this.DialogueSequenceSetpOperation();
                }

                // ���� ��� ���� ���� ��� �ڷ�ƾ�� ������, ���ð� ����.
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
            // ��� ����� ����ϴ� View ������ ������ return.
            if (this.DialogueTextMediatorView == null) yield break;

            // View���� Text �ʱ�ȭ ���� ��û.
            this.DialogueTextMediatorView.ClearText();

            // ������ġ: cps�� 0 �����̸� ��� ���
            if (charsPerSecond <= 0f)
            {
                this.DialogueTextMediatorView.UpdateContentText(content);
                yield break;
            }

            float delay = 1f / charsPerSecond;
            WaitForSeconds wait = new WaitForSeconds(delay);

            // ���� �ܼ��� ���: ���� �ϳ��� ���̱�
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