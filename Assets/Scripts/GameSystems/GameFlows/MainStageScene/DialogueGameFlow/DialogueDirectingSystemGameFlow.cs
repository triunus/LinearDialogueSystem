using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using GameSystems.DTOs;
using GameSystems.Entities.MainStageScene;

namespace GameSystems.DTOs
{
    public class DirectingCoroutineControlData
    {
        public int DirectingIndex { get; set; }

        public Coroutine ControlCoroutine { get; set; }
        public Coroutine BehaviourCoroutine { get; set; }

        public BehaviourToken BehaviourToken { get; set; }

        public bool IsOperationEnd()
        {
            if (this.ControlCoroutine == null && this.BehaviourCoroutine == null) return true;
            else return false;
        }
    }

    public class BehaviourToken
    {
        public BehaviourToken(bool isRequestEnd)
        {
            IsRequestEnd = isRequestEnd;
        }

        public bool IsRequestEnd { get; set; }
    }
}

namespace GameSystems.GameFlows.MainStageScene
{
    public interface IDialogueDirectingSystemGameFlow
    {
        public void OperateDialogueDirecting(int currentDirectingIndex);

        public void OperateDialogueClickInteraction();
    }

    public class DialogueDirectingSystemGameFlow : MonoBehaviour, IGameFlow, IDialogueDirectingSystemGameFlow
    {
        // Text ��� View
        private IDialogueTextDirectingView DialogueTextDirectingView;
        // ������ ��� View
        private IDialogueChoiceDirectingViewMediator DialogueChoiceDirectingViewMediator;
        // Cutscene ��� View
        private IDialogueCutsceneDirectingView DialogueCutsceneDirectingView;
        // Image ���� Hub �ۻ��
        private IDialogueImageDirectingFacade DialogueImageDirectingFacade;

        // Text ��� �ڷ�ƾ ��� ���� ������.
        private DirectingCoroutineControlData TextDirectingCoroutineControlData = new();
        // CanvasUIUX, BackGround, Actor�� Action �ڷ�ƾ ��� ���� ������.
        private List<DirectingCoroutineControlData> ActionDirectingCoroutineControlDatas = new();

        // �ڵ� ��� ��� �ڷ�ƾ.
        private Coroutine AutoCoroutine;
        private float AutoDuration = 2f;
        private float currentWaitDuration = 0f;

        private List<DialogueDirectingData> TotalDialogueDirectingDatas;

        private bool currentIsSkipable = false;
        private bool currentIsAutoable = false;
        private string NextDirectiveCommand = default;

        private int currentDirectingIndex = 0;

        // �ݺ����� �׽�Ʈ�� ���� �ӽð�.
        private bool IsActivated_Temp = false;

        private void Awake()
        {
            var LocalRepository = Repository.MainStageSceneRepository.Instance;

            LocalRepository.GameFlow_LazyReferenceRepository.RegisterReference<DialogueDirectingSystemGameFlow>(this);

            var entityReferenceRepository = LocalRepository.Entity_LazyReferenceRepository;

            this.DialogueTextDirectingView =
                entityReferenceRepository.GetOrWaitReference<DialogueTextDirectingView>(x => this.DialogueTextDirectingView = x);
            this.DialogueChoiceDirectingViewMediator =
                entityReferenceRepository.GetOrWaitReference<DialogueChoiceDirectingViewMediator>(x => this.DialogueChoiceDirectingViewMediator = x);
            this.DialogueCutsceneDirectingView =
                entityReferenceRepository.GetOrWaitReference<DialogueCutsceneDirectingView>(x => this.DialogueCutsceneDirectingView = x);
            this.DialogueImageDirectingFacade =
                entityReferenceRepository.GetOrWaitReference<DialogueImageDirectingFacade>(x => this.DialogueImageDirectingFacade = x);

            this.InitialSetting();
//            this.InitialSetting_Parsing();
        }

        private void InitialSetting()
        {
            this.TotalDialogueDirectingDatas = new()
            {
                new DialogueDirectingData(0, "DialogueCutsceneDirectingType", "Title01_Chapter01_1_1_1", false, false, true, "Next"),
                new DialogueDirectingData(1, "DialogueImageDirectingType", "FadeIn_DefaultBackGround_1", true, false, true, "Next"),
                new DialogueDirectingData(2, "DialogueImageDirectingType", "FadeIn_DefaultDialogueUIUX_1", false, false, true, "Next"),
                new DialogueDirectingData(3, "DialogueImageDirectingType", "SetPosition_ActorA_2", true, false, true, "Next"),
                new DialogueDirectingData(4, "DialogueImageDirectingType", "FadeIn_ActorA_3", false, false, true, "Next"),
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
                new DialogueDirectingData(32, "DialogueTextDirectingType", "ActorA_Actor A_�̹��� �ٸ� ����� Ȯ���� ����.", false, true, false, "Next"),
                new DialogueDirectingData(33, "DialogueImageDirectingType", "SetFaceSprite_ActorA_Test", true, false, true, "End"),
            };
        }
/*
                private async void InitialSetting_Parsing()
                {
                    var GlobalRepository = Repository.GlobalSceneRepository.Instance;

                    PlainServices.IResourcesPathResolver resourcesPathResolver =
                        GlobalRepository.PlainServices_LazyReferenceRepository.GetOrCreate<PlainServices.ResourcesPathResolver>();
                    PlainServices.IJsonReadAndWriteService jsonReadAndWriteService =
                        GlobalRepository.PlainServices_LazyReferenceRepository.GetOrCreate<PlainServices.JsonReadAndWriteService>();

                    string filePath = resourcesPathResolver.GetDialogueStoryPath(DialogueStoryType.CookingStoryType, 0, DialoguePhaseType.Intro);

                    this.DialogueSequenceStepDataList_Temp = await jsonReadAndWriteService.ReadAsync<DialogueSequenceStepDataList_ForJson>(filePath);

                    this.SequenceStepDatas = new();
                    foreach (var data in DialogueSequenceStepDataList_Temp.DialogueSequenceStepDatas)
                    {
                        this.SequenceStepDatas.Add(new(data));
                    }
                }
        public void SetSequenceStepDatas(DialogueDirectingData[] dialogueDirectingDatas)
        {
            this.TotalDialogueDirectingDatas = new List<DialogueDirectingData>(dialogueDirectingDatas);
        }

        public bool IsDialogueSystemVaild()
        {
            if (this.TotalDialogueDirectingDatas == null || this.TotalDialogueDirectingDatas.Count <= 0) return false;
            else return true;
        }
*/

        // �׽�Ʈ�� ���� �ӽ÷� �����ϱ� ���� ���
        public void TestButtonOperation()
        {
            // �ݺ����� ��� �׽�Ʈ�� ���� �ӽð�.
            if (this.IsActivated_Temp) return;
            else this.IsActivated_Temp = true;

            this.OperateDialogueDirecting(0);
        }
        public void OperateDialogueDirecting(int currentDirectingIndex)
        {
            // �����ϰ��� �ϴ� ���� ��ȣ�� �߸��� ���, return.
            if (this.TotalDialogueDirectingDatas.Count <= currentDirectingIndex || this.TotalDialogueDirectingDatas[currentDirectingIndex] == null)
            {
                Debug.Log($"�߸��� ���� ��ȣ : {currentDirectingIndex}");
                return;
            }

            this.currentDirectingIndex = currentDirectingIndex;

//            Debug.Log($"ȣ��� 1");

            // �̹��� ������ ���� ������ ������.
            DialogueDirectingData currentDialogueDirectingData = this.TotalDialogueDirectingDatas[currentDirectingIndex];

            DirectingType currentDirectingType = (DirectingType)System.Enum.Parse(typeof(DirectingType), currentDialogueDirectingData.DirectingType);

//            Debug.Log($"ȣ��� 2");

            switch (currentDirectingType)
            {
                case DirectingType.DialogueTextDirectingType:
 //                   Debug.Log($"ȣ��� 3");
                    if (this.DialogueTextDirectingView.TryDirectTextDisplayOperation(currentDialogueDirectingData.DirectingContent, out var textDisplayCo, out var textDisplayToken) && textDisplayCo != null
                        && this.DialogueImageDirectingFacade.TrySetSpeakerAndListenerColor(currentDialogueDirectingData.DirectingContent))
                    {
                        this.TextDirectingCoroutineControlData.DirectingIndex = currentDialogueDirectingData.Index;
                        this.TextDirectingCoroutineControlData.BehaviourCoroutine = StartCoroutine(textDisplayCo);
                        this.TextDirectingCoroutineControlData.ControlCoroutine = StartCoroutine(this.OperateTextDisplayCoroutine());
                        this.TextDirectingCoroutineControlData.BehaviourToken = textDisplayToken;
                    }
                    break;
                case DirectingType.DialogueChoiceDirectingType:
//                    Debug.Log($"ȣ��� 4");
                    if (this.DialogueChoiceDirectingViewMediator.TryDirectChoiceViewOperation(currentDialogueDirectingData.DirectingContent, out var choiceDisplayCo, out var choiceActionToken) && choiceDisplayCo != null)
                    {
                        DirectingCoroutineControlData newDirectingCoroutineControlData = new DirectingCoroutineControlData();
                        this.ActionDirectingCoroutineControlDatas.Add(newDirectingCoroutineControlData);

                        newDirectingCoroutineControlData.DirectingIndex = currentDialogueDirectingData.Index;
                        newDirectingCoroutineControlData.BehaviourCoroutine = StartCoroutine(choiceDisplayCo);
                        newDirectingCoroutineControlData.ControlCoroutine = StartCoroutine(this.OperateActionCoroutine(newDirectingCoroutineControlData.DirectingIndex));
                        newDirectingCoroutineControlData.BehaviourToken = choiceActionToken;
                    }
                    break;
                case DirectingType.DialogueCutsceneDirectingType:
 //                   Debug.Log($"ȣ��� 5");
                    if (this.DialogueCutsceneDirectingView.TryDirectCutsceneDisplayOperation(currentDialogueDirectingData.DirectingContent, out var cutsceneCo, out var cutsceneActionToken) && cutsceneCo != null)
                    {
                        DirectingCoroutineControlData newDirectingCoroutineControlData = new DirectingCoroutineControlData();
                        this.ActionDirectingCoroutineControlDatas.Add(newDirectingCoroutineControlData);

                        newDirectingCoroutineControlData.DirectingIndex = currentDialogueDirectingData.Index;
                        newDirectingCoroutineControlData.BehaviourCoroutine = StartCoroutine(cutsceneCo);
                        newDirectingCoroutineControlData.ControlCoroutine = StartCoroutine(this.OperateActionCoroutine(newDirectingCoroutineControlData.DirectingIndex));
                        newDirectingCoroutineControlData.BehaviourToken = cutsceneActionToken;
                    }
                    break;
                case DirectingType.DialogueImageDirectingType:
                    Debug.Log($"ȣ��� 6");
                    if (this.DialogueImageDirectingFacade.TryAction(currentDialogueDirectingData.DirectingContent, out var ImageDirectingCo, out var imageBehaviourToken) && ImageDirectingCo != null)
                    {
                        DirectingCoroutineControlData newDirectingCoroutineControlData = new DirectingCoroutineControlData();
                        this.ActionDirectingCoroutineControlDatas.Add(newDirectingCoroutineControlData);

                        newDirectingCoroutineControlData.DirectingIndex = currentDialogueDirectingData.Index;
                        newDirectingCoroutineControlData.BehaviourCoroutine = StartCoroutine(ImageDirectingCo);
                        newDirectingCoroutineControlData.ControlCoroutine = StartCoroutine(this.OperateActionCoroutine(currentDialogueDirectingData.Index));
                        newDirectingCoroutineControlData.BehaviourToken = imageBehaviourToken;

                        Debug.Log($"DirectingIndex : {newDirectingCoroutineControlData.DirectingIndex}, BehaviourCoroutine : {newDirectingCoroutineControlData.BehaviourCoroutine}," +
                            $" BehaviourCoroutine : {newDirectingCoroutineControlData.BehaviourCoroutine}, BehaviourToken : {newDirectingCoroutineControlData.BehaviourToken.IsRequestEnd}");
                    }
                    break;
                default:
                    break;
            }

//            Debug.Log($"ȣ��� 7");
            if (currentDialogueDirectingData.IsChainWithNext)
            {
//                Debug.Log($"ȣ��� 8");
                if (this.TryGetNextDirectingIndex(currentDialogueDirectingData.NextDirectiveCommand, out var index))
                {
                    this.OperateDialogueDirecting(index);
                    return;
                }
            }

//            Debug.Log($"ȣ��� 9");
            this.currentIsSkipable = currentDialogueDirectingData.IsSkipable;
            this.currentIsAutoable = currentDialogueDirectingData.IsAutoAble;
            this.NextDirectiveCommand = currentDialogueDirectingData.NextDirectiveCommand;
        }

        // ���� ����, �ڷ�ƾ ������ �ִٸ�, ��ū�� ���Ͽ� �ڿ������� �������� �ǵ��� �����Ѵ�.
        public void StopDialogueDirection()
        {
            if (!this.TextDirectingCoroutineControlData.IsOperationEnd())
            {
                this.TextDirectingCoroutineControlData.BehaviourToken.IsRequestEnd = true;
            }

            foreach (var controlData in this.ActionDirectingCoroutineControlDatas)
            {
                controlData.BehaviourToken.IsRequestEnd = true;
            }
        }

        // Action �ڷ�ƾ ����
        public IEnumerator OperateActionCoroutine(int directingIndex)
        {
            DirectingCoroutineControlData thisDirectingCoroutineControlData = this.ActionDirectingCoroutineControlDatas.Find(x => x.DirectingIndex == directingIndex);
           
            yield return thisDirectingCoroutineControlData.BehaviourCoroutine;

            thisDirectingCoroutineControlData.BehaviourCoroutine = null;
            thisDirectingCoroutineControlData.ControlCoroutine = null;
            this.ActionDirectingCoroutineControlDatas.Remove(thisDirectingCoroutineControlData);

            this.RequestNextDirecting();
        }
        // Text �ڷ�ƾ ����
        public IEnumerator OperateTextDisplayCoroutine()
        {
            yield return this.TextDirectingCoroutineControlData.BehaviourCoroutine;

            this.TextDirectingCoroutineControlData.BehaviourCoroutine = null;
            this.TextDirectingCoroutineControlData.ControlCoroutine = null;

            this.RequestNextDirecting();
        }
        // 1���� ���� ����, ���� ������ �̾ �������� �����ϴ� �κ�.
        private void RequestNextDirecting()
        {
            // �ڵ� ��� �����Ѱ�? + Text ��� �۾� �����°�? + Action ���� �����°�?
            if (this.currentIsAutoable && this.TextDirectingCoroutineControlData.IsOperationEnd() && this.ActionDirectingCoroutineControlDatas.Count == 0)
            {
                if (this.TryGetNextDirectingIndex(this.NextDirectiveCommand, out var index))
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
                    nextDirectingIndex = this.currentDirectingIndex + 1;
                    return true;
                case NextDirectiveCommandType.Jump:
                    nextDirectingIndex = int.Parse(parsedData[1]);
                    return true;
                case NextDirectiveCommandType.End:
                    nextDirectingIndex = default;
                    this.IsActivated_Temp = false;
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
                    this.IsActivated_Temp = false;
                    return false;
            }
        }

        // ���콺 �Է� ���
        // '���콺 Ŭ��'�� ���� ���� ������ ���������� ������ ���� ���� ������ �Ѵ�.
        public void OperateDialogueClickInteraction()
        {
            Debug.Log($"OnClicked_Mouse - 0");
            // �ش� ������ ���콺Ŭ���� ���� ������ �������� ����.
            if (!this.currentIsSkipable) return;

            // ���� ���� �ƴ϶��, ���� ������ ���� ����.
            if (this.ActionDirectingCoroutineControlDatas.Count == 0 && this.TextDirectingCoroutineControlData.IsOperationEnd())
            {
                Debug.Log($"OnClicked_Mouse - 2");
                // ������ ������ DirectingIndex ���� Ȯ��.
                if (this.TryGetNextDirectingIndex(this.NextDirectiveCommand, out var index))
                {
                    Debug.Log($"OnClicked_Mouse - 3");
                    // �����Ͽ��� ��, �ش� ���� ����.
                    this.OperateDialogueDirecting(index);
                    this.currentWaitDuration = 0;
                }
            }
            // ���� ���̶��, ���� �� ���� ���.
            else
            {
                Debug.Log($"OnClicked_Mouse - 4");
                this.StopDialogueDirection();
            }
        }

        // Auto ��� ���
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
                this.currentWaitDuration = 0;
            }
        }
        private IEnumerator OperateDialogueAutoDisplay()
        {
            this.currentWaitDuration = 0;

            while (true)
            {
                //  ���� ��� ���� �ڷ�ƾ�� ������ �׳� �Ѿ.
                if (this.ActionDirectingCoroutineControlDatas.Count == 0 && this.TextDirectingCoroutineControlData.IsOperationEnd())
                {
                    // ����� ��ٷ�����, ���� ��ȭ ���� ���� ��û.
                    if (this.currentWaitDuration >= this.AutoDuration)
                    {
                        if (this.TryGetNextDirectingIndex(this.NextDirectiveCommand, out var index))
                        {
                            this.OperateDialogueDirecting(index);
                            this.currentWaitDuration = 0;
                        }
                        else
                        {
                            this.currentWaitDuration = 0;
                        }
                    }
                    // ���� ��� ���� �ڷ�ƾ�� ������ �ð� ����.
                    else
                    {
                        this.currentWaitDuration += Time.deltaTime;
                    }
                }

                yield return Time.deltaTime;
            }
        }

        // Skip ���
        public void OnClickedSkipButton()
        {
            // ��ŵ ��� ���ҽ� �Ѿ.
            if (!this.currentIsSkipable) return;

            this.StopDialogueDirection();
        }
    }
}