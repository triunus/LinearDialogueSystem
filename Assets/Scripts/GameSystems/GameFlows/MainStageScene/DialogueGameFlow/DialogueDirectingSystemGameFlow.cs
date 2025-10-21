using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using GameSystems.DTOs;
using GameSystems.Entities.MainStageScene;

namespace GameSystems.GameFlows.MainStageScene
{
    public class DialogueDirectingSystemGameFlow : MonoBehaviour, IGameFlow
    {
        // Text ��� View
        private IDialogueTextDirectingView DialogueTextDirectingView;
        // ������ ��� View
        private IDialogueChoiceDirectingViewMediator DialogueChoiceDirectingViewMediator;
        // Cutscene ��� View
        private IDialogueCutsceneDirectingView DialogueCutsceneDirectingView;
        // Image ���� Hub �ۻ��
        private IDialogueImageDirectingFacade DialogueImageDirectingFacade;

        // Text ��� �ڷ�ƾ.
        private Coroutine TextDisplayCoroutine;
        // CanvasUIUX, BackGround, Actor�� Action �ڷ�ƾ.
        private Dictionary<int, Coroutine> ActionCoroutines = new();

        // �ڵ� ��� ��� �ڷ�ƾ.
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
            // �����ϰ��� �ϴ� ���� ��ȣ�� �߸��� ���, return.
            if (this.TotalDialogueDirectingDatas.Count <= currentDirectingIndex || this.TotalDialogueDirectingDatas[currentDirectingIndex] == null)
            {
                Debug.Log($"�߸��� ���� ��ȣ : {currentDirectingIndex}");
                return;
            }

            this.currentDirectingIndex = currentDirectingIndex;

            Debug.Log($"ȣ��� 1");

            // �̹��� ������ ���� ������ ������.
            DialogueDirectingData currentDialogueDirectingData = this.TotalDialogueDirectingDatas[currentDirectingIndex];

            DirectingType currentDirectingType = (DirectingType)System.Enum.Parse(typeof(DirectingType), currentDialogueDirectingData.DirectingType);

            Debug.Log($"ȣ��� 2");

            switch (currentDirectingType)
            {
                case DirectingType.DialogueTextDirectingType:
                    Debug.Log($"ȣ��� 3");
                    if (this.DialogueTextDirectingView.TryDirectTextDisplayOperation(currentDialogueDirectingData.DirectingContent, out var textDisplayCo) && textDisplayCo != null)
                        this.TextDisplayCoroutine = StartCoroutine(this.OperateTextDisplayCoroutine(textDisplayCo));
                    break;
                case DirectingType.DialogueChoiceDirectingType:
                    Debug.Log($"ȣ��� 4");
                    if (this.DialogueChoiceDirectingViewMediator.TryDirectChoiceViewOperation(currentDialogueDirectingData.DirectingContent, out var choiceDisplayCo) && choiceDisplayCo != null)
                    {
                        Coroutine tempCoroutine = StartCoroutine(this.OperateActionCoroutine(currentDialogueDirectingData.Index, choiceDisplayCo));

                        this.ActionCoroutines.Add(currentDialogueDirectingData.Index, tempCoroutine);
                    }
                    break;
                case DirectingType.DialogueCutsceneDirectingType:
                    Debug.Log($"ȣ��� 5");
                    if (this.DialogueCutsceneDirectingView.TryDirectCutsceneDisplayOperation(currentDialogueDirectingData.DirectingContent, out var cutsceneCo) && cutsceneCo != null)
                    {
                        Debug.Log($"ȣ��� 5-1");
                        Coroutine tempCoroutine = StartCoroutine(this.OperateActionCoroutine(currentDialogueDirectingData.Index, cutsceneCo));

                        this.ActionCoroutines.Add(currentDialogueDirectingData.Index, tempCoroutine);
                    }
                    break;
                case DirectingType.DialogueImageDirectingType:
                    Debug.Log($"ȣ��� 6");
                    if (this.DialogueImageDirectingFacade.TryAction(currentDialogueDirectingData.DirectingContent, out var ImageDirecting) && ImageDirecting != null)
                    {
                        Coroutine tempCoroutine = StartCoroutine(this.OperateActionCoroutine(currentDialogueDirectingData.Index, ImageDirecting));

                        this.ActionCoroutines.Add(currentDialogueDirectingData.Index, tempCoroutine);
                    }
                    break;
                default:
                    break;
            }

            Debug.Log($"ȣ��� 7");
            if (currentDialogueDirectingData.IsChainWithNext)
            {
                Debug.Log($"ȣ��� 8");
                if (this.TryGetNextDirectingIndex(currentDialogueDirectingData.NextDirectiveCommand, out var index))
                {
                    this.OperateDialogueDirecting(index);
                    return;
                }
            }

            Debug.Log($"ȣ��� 9");
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
            Debug.Log($"����");
        }

        public void OnClicked_Mouse()
        {
            // �ش� ��ȭ ���� ��ŵ �������� Ȯ��.
            if (this.currentIsSkipable)
            {
                // Type ���� �ƴ϶��, ���� ������ ���� ����.
                if (this.ActionCoroutines.Count == 0 && this.TextDisplayCoroutine == null)
                {
                    if (this.TryGetNextDirectingIndex(this.NextDirectiveCommand, out var index))
                    {
                        this.OperateDialogueDirecting(index);
                    }
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
                if(this.ActionCoroutines.Count == 0 && this.TextDisplayCoroutine == null)
                {
                    // ����� ��ٷ�����, ���� ��ȭ ���� ���� ��û.
                    if (waitedTime >= this.AutoDuration)
                    {
                        if (this.TryGetNextDirectingIndex(this.NextDirectiveCommand, out var index))
                        {
                            this.OperateDialogueDirecting(index);
                            waitedTime = 0;
                        }
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
            // ��ŵ ��� ���ҽ� �Ѿ.
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
                    Debug.Log($"����");
                    return false;
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
    }
}