using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using GameSystems.DTOs;
using GameSystems.Entities.MainStageScene;

namespace GameSystems.GameFlows.MainStageScene
{
    public class DialogueDirectingSystemGameFlow : MonoBehaviour, IGameFlow
    {
        // Text 출력 View
        private IDialogueTextDirectingView DialogueTextDirectingView;
        // 선택지 출력 View
        private IDialogueChoiceDirectingViewMediator DialogueChoiceDirectingViewMediator;
        // Cutscene 출력 View
        private IDialogueCutsceneDirectingView DialogueCutsceneDirectingView;
        // Image 연출 Hub 퍼사드
        private IDialogueImageDirectingFacade DialogueImageDirectingFacade;

        // Text 출력 코루틴.
        private Coroutine TextDisplayCoroutine;
        // CanvasUIUX, BackGround, Actor의 Action 코루틴.
        private Dictionary<int, Coroutine> ActionCoroutines = new();

        // 자동 재생 기능 코루틴.
        private Coroutine AutoCoroutine;
        private float AutoDuration = 3f;

        private List<DialogueDirectingData> TotalDialogueDirectingDatas;

        private bool currentIsSkipable = false;
        private bool currentIsAutoable = false;
        private string NextDirectiveCommand = default;

        private int currentDirectingIndex = 0;

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
                new DialogueDirectingData(0, "DialogueCutsceneDirectingType", "FadeInOut_Title01_Chapter01_1_1_1", false, false, true, "Next"),
                new DialogueDirectingData(1, "DialogueImageDirectingType", "FadeIn_DefaultBackGround_1", true, false, true, "Next"),
                new DialogueDirectingData(2, "DialogueImageDirectingType", "FadeIn_DefaultDialogueUIUX_1", false, false, true, "End")
            };
        }

/*        private async void InitialSetting_Parsing()
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
        }*/

        public void SetSequenceStepDatas(DialogueDirectingData[] dialogueDirectingDatas)
        {
            this.TotalDialogueDirectingDatas = new List<DialogueDirectingData>(dialogueDirectingDatas);
        }
        public bool IsDialogueSystemVaild()
        {
            if (this.TotalDialogueDirectingDatas == null || this.TotalDialogueDirectingDatas.Count <= 0) return false;
            else return true;
        }

        public void TestButtonOperation()
        {
            this.OperateDialogueDirecting(this.currentDirectingIndex);
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

            Debug.Log($"호출됨 1");

            // 이번에 수행할 연출 데이터 가져옴.
            DialogueDirectingData currentDialogueDirectingData = this.TotalDialogueDirectingDatas[currentDirectingIndex];

            DirectingType currentDirectingType = (DirectingType)System.Enum.Parse(typeof(DirectingType), currentDialogueDirectingData.DirectingType);

            Debug.Log($"호출됨 2");

            switch (currentDirectingType)
            {
                case DirectingType.DialogueTextDirectingType:
                    Debug.Log($"호출됨 3");
                    if (this.DialogueTextDirectingView.TryDirectTextDisplayOperation(currentDialogueDirectingData.DirectingContent, out var textDisplayCo) && textDisplayCo != null)
                        this.TextDisplayCoroutine = StartCoroutine(this.OperateTextDisplayCoroutine(textDisplayCo));
                    break;
                case DirectingType.DialogueChoiceDirectingType:
                    Debug.Log($"호출됨 4");
                    if (this.DialogueChoiceDirectingViewMediator.TryDirectChoiceViewOperation(currentDialogueDirectingData.DirectingContent, out var choiceDisplayCo) && choiceDisplayCo != null)
                    {
                        Coroutine tempCoroutine = StartCoroutine(this.OperateActionCoroutine(currentDialogueDirectingData.Index, choiceDisplayCo));

                        this.ActionCoroutines.Add(currentDialogueDirectingData.Index, tempCoroutine);
                    }
                    break;
                case DirectingType.DialogueCutsceneDirectingType:
                    Debug.Log($"호출됨 5");
                    if (this.DialogueCutsceneDirectingView.TryDirectCutsceneDisplayOperation(currentDialogueDirectingData.DirectingContent, out var cutsceneCo) && cutsceneCo != null)
                    {
                        Debug.Log($"호출됨 5-1");
                        Coroutine tempCoroutine = StartCoroutine(this.OperateActionCoroutine(currentDialogueDirectingData.Index, cutsceneCo));

                        this.ActionCoroutines.Add(currentDialogueDirectingData.Index, tempCoroutine);
                    }
                    break;
                case DirectingType.DialogueImageDirectingType:
                    Debug.Log($"호출됨 6");
                    if (this.DialogueImageDirectingFacade.TryAction(currentDialogueDirectingData.DirectingContent, out var ImageDirecting) && ImageDirecting != null)
                    {
                        Coroutine tempCoroutine = StartCoroutine(this.OperateActionCoroutine(currentDialogueDirectingData.Index, ImageDirecting));

                        this.ActionCoroutines.Add(currentDialogueDirectingData.Index, tempCoroutine);
                    }
                    break;
                default:
                    break;
            }

            Debug.Log($"호출됨 7");
            if (currentDialogueDirectingData.IsChainWithNext)
            {
                Debug.Log($"호출됨 8");
                if (this.TryGetNextDirectingIndex(currentDialogueDirectingData.NextDirectiveCommand, out var index))
                {
                    this.OperateDialogueDirecting(index);
                    return;
                }
            }

            Debug.Log($"호출됨 9");
            this.currentIsSkipable = currentDialogueDirectingData.IsSkipable;
            this.currentIsAutoable = currentDialogueDirectingData.IsAutoAble;
            this.NextDirectiveCommand = currentDialogueDirectingData.NextDirectiveCommand;
        }
        public void StopDialogueDirection()
        {
            this.StopCoroutine(this.TextDisplayCoroutine);
            this.TextDisplayCoroutine = null;

            for(int i = 0; i < this.ActionCoroutines.Count; ++i)
            {
                this.StopCoroutine(this.ActionCoroutines[i]);
                this.ActionCoroutines[i] = null;
            }

            this.ActionCoroutines.Clear();
        }
        public void EndDialogueDirectingSystem()
        {
            Debug.Log($"종료");
        }

        public void OnClicked_Mouse()
        {
            // 해당 대화 연출 스킵 가능한지 확인.
            if (this.currentIsSkipable)
            {
                // Type 중이 아니라면, 다음 시퀀스 스탭 시작.
                if (this.ActionCoroutines.Count == 0 && this.TextDisplayCoroutine == null)
                {
                    if (this.TryGetNextDirectingIndex(this.NextDirectiveCommand, out var index))
                    {
                        this.OperateDialogueDirecting(index);
                    }
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
                if(this.ActionCoroutines.Count == 0 && this.TextDisplayCoroutine == null)
                {
                    // 충분히 기다렸으면, 다음 대화 연출 수행 요청.
                    if (waitedTime >= this.AutoDuration)
                    {
                        if (this.TryGetNextDirectingIndex(this.NextDirectiveCommand, out var index))
                        {
                            this.OperateDialogueDirecting(index);
                            waitedTime = 0;
                        }
                    }
                    // 현재 재생 중인 코루틴이 없으면 시간 증가.
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
            // 스킵 허용 안할시 넘어감.
            if (!this.currentIsSkipable) return;

            this.StopDialogueDirection();
        }

        public IEnumerator OperateActionCoroutine(int directingIndex, IEnumerator actionOperation)
        {
            yield return StartCoroutine(actionOperation);

            this.ActionCoroutines[directingIndex] = null;
            this.ActionCoroutines.Remove(directingIndex);

            if (this.currentIsAutoable && this.ActionCoroutines.Count == 0 && this.TextDisplayCoroutine == null)
            {
                if (this.TryGetNextDirectingIndex(this.NextDirectiveCommand, out var index))
                {
                    this.OperateDialogueDirecting(index);
                }
            }
        }
        public IEnumerator OperateTextDisplayCoroutine(IEnumerator textDisplayOperation)
        {
            yield return StartCoroutine(textDisplayOperation);

            this.TextDisplayCoroutine = null;

            if (this.currentIsAutoable && this.ActionCoroutines.Count == 0)
            {
                if(this.TryGetNextDirectingIndex(this.NextDirectiveCommand, out var index))
                {
                    this.OperateDialogueDirecting(index);
                }
            }
        }
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
                    Debug.Log($"끝남");
                    return false;
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
    }
}