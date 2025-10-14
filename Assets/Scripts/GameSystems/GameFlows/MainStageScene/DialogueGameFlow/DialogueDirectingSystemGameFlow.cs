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
        private float AutoDuration = 3f;

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
                            new ActionDirectionData("DefaultBackGround", ActionType.FadeIn, string.Empty, "0.5")),
                        new SequenceStepData(0, SequenceDataType.CanvasUIUXActionType, false, true,
                            new ActionDirectionData("DefaultDialogueUIUX", ActionType.FadeIn, string.Empty, "0.5")),

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
                            new ActionDirectionData("DefaultTextPanel", ActionType.FadeOut, string.Empty, "0.5")),
                        new SequenceStepData(0, SequenceDataType.CanvasUIUXActionType, false, true,
                            new ActionDirectionData("DefaultDialogueUIUX", ActionType.FadeOut, string.Empty, "0.5")),
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
                if(this.ActionCoroutine == null && this.TextDisplayCoroutine == null)
                {
                    // ����� ��ٷ�����, ���� ��ȭ ���� ���� ��û.
                    if (waitedTime >= this.AutoDuration)
                    {
                        this.StartDialogueDirectingSystem();
                        waitedTime = 0;
                    }
                    // ���� ��� ���� �ڷ�ƾ�� ������ �ð� ����.
                    else
                    {
                        waitedTime += Time.deltaTime;
                    }
                }

                yield return Time.deltaTime;
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
}