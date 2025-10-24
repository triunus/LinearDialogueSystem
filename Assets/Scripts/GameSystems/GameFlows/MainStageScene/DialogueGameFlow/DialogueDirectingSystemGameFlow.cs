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
        // Text 출력 View
        private IDialogueTextDirectingView DialogueTextDirectingView;
        // 선택지 출력 View
        private IDialogueChoiceDirectingViewMediator DialogueChoiceDirectingViewMediator;
        // Cutscene 출력 View
        private IDialogueCutsceneDirectingView DialogueCutsceneDirectingView;
        // Image 연출 Hub 퍼사드
        private IDialogueImageDirectingFacade DialogueImageDirectingFacade;

        // Text 출력 코루틴 제어를 위한 데이터.
        private DirectingCoroutineControlData TextDirectingCoroutineControlData = new();
        // CanvasUIUX, BackGround, Actor의 Action 코루틴 제어를 위한 데이터.
        private List<DirectingCoroutineControlData> ActionDirectingCoroutineControlDatas = new();

        // 자동 재생 기능 코루틴.
        private Coroutine AutoCoroutine;
        private float AutoDuration = 2f;
        private float currentWaitDuration = 0f;

        private List<DialogueDirectingData> TotalDialogueDirectingDatas;

        private bool currentIsSkipable = false;
        private bool currentIsAutoable = false;
        private string NextDirectiveCommand = default;

        private int currentDirectingIndex = 0;

        // 반복적인 테스트를 위한 임시값.
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
                new DialogueDirectingData(32, "DialogueTextDirectingType", "ActorA_Actor A_이번엔 다른 기능을 확인해 볼게.", false, true, false, "Next"),
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

        // 테스트를 위해 임시로 시작하기 위한 기능
        public void TestButtonOperation()
        {
            // 반복적인 재생 테스트를 위한 임시값.
            if (this.IsActivated_Temp) return;
            else this.IsActivated_Temp = true;

            this.OperateDialogueDirecting(0);
        }
        public void OperateDialogueDirecting(int currentDirectingIndex)
        {
            // 수행하고자 하는 연출 번호가 잘못된 경우, return.
            if (this.TotalDialogueDirectingDatas.Count <= currentDirectingIndex || this.TotalDialogueDirectingDatas[currentDirectingIndex] == null)
            {
                Debug.Log($"잘못된 연출 번호 : {currentDirectingIndex}");
                return;
            }

            this.currentDirectingIndex = currentDirectingIndex;

//            Debug.Log($"호출됨 1");

            // 이번에 수행할 연출 데이터 가져옴.
            DialogueDirectingData currentDialogueDirectingData = this.TotalDialogueDirectingDatas[currentDirectingIndex];

            DirectingType currentDirectingType = (DirectingType)System.Enum.Parse(typeof(DirectingType), currentDialogueDirectingData.DirectingType);

//            Debug.Log($"호출됨 2");

            switch (currentDirectingType)
            {
                case DirectingType.DialogueTextDirectingType:
 //                   Debug.Log($"호출됨 3");
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
//                    Debug.Log($"호출됨 4");
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
 //                   Debug.Log($"호출됨 5");
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
                    Debug.Log($"호출됨 6");
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

//            Debug.Log($"호출됨 7");
            if (currentDialogueDirectingData.IsChainWithNext)
            {
//                Debug.Log($"호출됨 8");
                if (this.TryGetNextDirectingIndex(currentDialogueDirectingData.NextDirectiveCommand, out var index))
                {
                    this.OperateDialogueDirecting(index);
                    return;
                }
            }

//            Debug.Log($"호출됨 9");
            this.currentIsSkipable = currentDialogueDirectingData.IsSkipable;
            this.currentIsAutoable = currentDialogueDirectingData.IsAutoAble;
            this.NextDirectiveCommand = currentDialogueDirectingData.NextDirectiveCommand;
        }

        // 진행 중인, 코루틴 동작이 있다면, 토큰을 통하여 자연스럽게 마무리가 되도록 유도한다.
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

        // Action 코루틴 제어
        public IEnumerator OperateActionCoroutine(int directingIndex)
        {
            DirectingCoroutineControlData thisDirectingCoroutineControlData = this.ActionDirectingCoroutineControlDatas.Find(x => x.DirectingIndex == directingIndex);
           
            yield return thisDirectingCoroutineControlData.BehaviourCoroutine;

            thisDirectingCoroutineControlData.BehaviourCoroutine = null;
            thisDirectingCoroutineControlData.ControlCoroutine = null;
            this.ActionDirectingCoroutineControlDatas.Remove(thisDirectingCoroutineControlData);

            this.RequestNextDirecting();
        }
        // Text 코루틴 제어
        public IEnumerator OperateTextDisplayCoroutine()
        {
            yield return this.TextDirectingCoroutineControlData.BehaviourCoroutine;

            this.TextDirectingCoroutineControlData.BehaviourCoroutine = null;
            this.TextDirectingCoroutineControlData.ControlCoroutine = null;

            this.RequestNextDirecting();
        }
        // 1개의 연출 이후, 다음 연출을 이어서 수행할지 결정하는 부분.
        private void RequestNextDirecting()
        {
            // 자동 재생 가능한가? + Text 출력 작업 끝났는가? + Action 동작 끝났는가?
            if (this.currentIsAutoable && this.TextDirectingCoroutineControlData.IsOperationEnd() && this.ActionDirectingCoroutineControlDatas.Count == 0)
            {
                if (this.TryGetNextDirectingIndex(this.NextDirectiveCommand, out var index))
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
                    nextDirectingIndex = this.currentDirectingIndex + 1;
                    return true;
                case NextDirectiveCommandType.Jump:
                    nextDirectingIndex = int.Parse(parsedData[1]);
                    return true;
                case NextDirectiveCommandType.End:
                    nextDirectingIndex = default;
                    this.IsActivated_Temp = false;
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
                    this.IsActivated_Temp = false;
                    return false;
            }
        }

        // 마우스 입력 기능
        // '마우스 클릭'을 통해 현재 연출을 정상적으로 마지막 상태 값을 갖도록 한다.
        public void OperateDialogueClickInteraction()
        {
            Debug.Log($"OnClicked_Mouse - 0");
            // 해당 연출이 마우스클릭을 통해 생략이 가능한지 여부.
            if (!this.currentIsSkipable) return;

            // 연출 중이 아니라면, 다음 시퀀스 스탭 시작.
            if (this.ActionDirectingCoroutineControlDatas.Count == 0 && this.TextDirectingCoroutineControlData.IsOperationEnd())
            {
                Debug.Log($"OnClicked_Mouse - 2");
                // 다음에 수행할 DirectingIndex 추출 확인.
                if (this.TryGetNextDirectingIndex(this.NextDirectiveCommand, out var index))
                {
                    Debug.Log($"OnClicked_Mouse - 3");
                    // 추출하였을 시, 해당 연출 수행.
                    this.OperateDialogueDirecting(index);
                    this.currentWaitDuration = 0;
                }
            }
            // 연출 중이라면, 중지 및 전부 출력.
            else
            {
                Debug.Log($"OnClicked_Mouse - 4");
                this.StopDialogueDirection();
            }
        }

        // Auto 재생 기능
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
                //  현재 재생 중인 코루틴이 있으면 그냥 넘어감.
                if (this.ActionDirectingCoroutineControlDatas.Count == 0 && this.TextDirectingCoroutineControlData.IsOperationEnd())
                {
                    // 충분히 기다렸으면, 다음 대화 연출 수행 요청.
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
                    // 현재 재생 중인 코루틴이 없으면 시간 증가.
                    else
                    {
                        this.currentWaitDuration += Time.deltaTime;
                    }
                }

                yield return Time.deltaTime;
            }
        }

        // Skip 기능
        public void OnClickedSkipButton()
        {
            // 스킵 허용 안할시 넘어감.
            if (!this.currentIsSkipable) return;

            this.StopDialogueDirection();
        }
    }
}