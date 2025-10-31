using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using GameSystems.DTOs;
using GameSystems.UnityServices;
using GameSystems.Entities;
using GameSystems.Entities.MainStageScene;
using GameSystems.PlainServices;

namespace GameSystems.GameFlows.MainStageScene
{
    public interface IDialogueDirectingResourceDataDB
    {
        public bool TryGetDialogueDirectingResourceData(string key, out DialogueDirectingResourceData dialogueDirectingResourceData);
    }
    [System.Serializable]
    public class DialogueDirectingResourceDataDB
    {
        [SerializeField] private List<DialogueDirectingResourceData> DialogueDirectingResourceDatas;

        public bool TryGetDialogueDirectingResourceData(string key, out DialogueDirectingResourceData dialogueDirectingResourceData)
        {
            dialogueDirectingResourceData = null;
            if (this.DialogueDirectingResourceDatas == null || this.DialogueDirectingResourceDatas.Count == 0) return false;

            foreach(var data in this.DialogueDirectingResourceDatas)
            {
                if(data.Key == key)
                {
                    dialogueDirectingResourceData = data;
                    return true;
                }
            }

            return false;
        }
    }
    [System.Serializable]
    public class DialogueDirectingResourceData
    {
        private string key;
        private List<string> ActorKeys;
        private List<string> SpriteKeys;
        private List<string> CanvasUIUXKeys;

        public string Key { get => key; }
        public bool TryGetActorKeys(out string[] actorKeys)
        {
            actorKeys = default;
            if (this.ActorKeys == null) return false;

            actorKeys = this.ActorKeys.ToArray();
            return true;
        }
        public bool TryGetSpriteKeys(out string[] spriteKeys)
        {
            spriteKeys = default;
            if (this.SpriteKeys == null) return false;

            spriteKeys = this.SpriteKeys.ToArray();
            return true;
        }
        public bool TryGetCanvasUIUXKeys(out string[] canvasUIUXKeys)
        {
            canvasUIUXKeys = default;
            if (this.CanvasUIUXKeys == null) return false;

            canvasUIUXKeys = this.CanvasUIUXKeys.ToArray();
            return true;
        }
    }

    public interface IDialogueDirectingPrefabResourceDB
    {
        public bool TryGetPrefabData(string prefabKey, out DialogueDirectingPrefabData prefabData);
    }
    [System.Serializable]
    public class DialogueDirectingPrefabResourceDB : MonoBehaviour
    {
        [SerializeField] private List<DialogueDirectingPrefabData> DialogueDirectingPrefabDatas;

        public bool TryGetPrefabData(string prefabKey, out DialogueDirectingPrefabData prefabData)
        {
            prefabData = null;

            foreach (var data in this.DialogueDirectingPrefabDatas)
            {
                if(data.PrefabKey == prefabKey)
                {
                    prefabData = data;
                    return true;
                }
            }

            return false;
        }
    }
    [System.Serializable]
    public class DialogueDirectingPrefabData
    {
        [SerializeField] private string prefabKey;

        [SerializeField] private Transform prefabParent;
        [SerializeField] private GameObject prefab;

        public string PrefabKey { get => prefabKey; }

        public Transform PrefabParent { get => prefabParent; }
        public GameObject Prefab { get => prefab; }
    }

    public class DialogueDirectingService
    {
        private IDialogueDirectingPrefabResourceDB DialoguePrefabResourceDB;
        private ICoroutineRunner CoroutineRunner;

        private DialogueViewModel DialogueViewModel;

        private DialogueDirectingSystemGameFlow DialogueDirectingSystemGameFlow;

        [SerializeField] private DialogueTextDirectingView DialogueTextDirectingView;
        [SerializeField] private DialogueChoiceDirectingViewMediator DialogueChoiceDirectingViewMediator;

        [SerializeField] private DialogueCutsceneDirectingView DialogueCutsceneDirectingView;
        [SerializeField] private DialogueHistoryGenerator DialogueHistoryGenerator;

        private DialogueImageDirectingFacade DialogueImageDirectingFacade;
 
        private DialogueViewActivator DialogueViewActivator;
        private DialogueViewFader DialogueViewFader;
        private DialogueViewSpriteSetter DialogueViewSpriteSetter;
        private DialogueViewPositioner DialogueViewPositioner;

        private DialogueGenerator DialogueGenerator;

        // 반복적인 테스트를 위한 임시값.
        private bool IsActivated_Temp = false;

        public DialogueDirectingService(IDialogueDirectingPrefabResourceDB dialoguePrefabResourceDB, ICoroutineRunner coroutineRunner)
        {
            this.DialogueViewModel = new();

            this.DialogueDirectingSystemGameFlow = new();
            this.DialogueGenerator = new();
            this.DialogueImageDirectingFacade = new();

            this.DialogueViewActivator = new();
            this.DialogueViewFader = new();
            this.DialogueViewSpriteSetter = new();
            this.DialogueViewPositioner = new();

        }

        public void BindDialogueDirecting()
        {
            this.DialogueImageDirectingFacade.InitialSetting(this.DialogueViewActivator, this.DialogueViewFader, this.DialogueViewSpriteSetter, this.DialogueViewPositioner);
            this.DialogueGenerator.InitialSetting(this.DialogueViewActivator, this.DialogueViewFader, this.DialogueViewSpriteSetter, this.DialogueViewPositioner);


        }


        // 테스트를 위해 임시로 시작하기 위한 기능
        public void TestButtonOperation()
        {
            // 반복적인 재생 테스트를 위한 임시값.
            if (this.IsActivated_Temp) return;
            else this.IsActivated_Temp = true;

            this.DialogueDirectingSystemGameFlow.OperateDialogueDirecting(0);
        }
    }

    public interface IDialogueViewModel : IMultiPlugInHub
    {
        public void RegisterViewObject(string key, GameObject viewObject);
        public void RemoveViewObject(string key);
        public bool TryGetViewObject(string key, out GameObject viewObject);
    }
    public class DialogueViewModel : IDialogueViewModel
    {
        private MultiPlugInHub MultiPlugInHub;

        private Dictionary<string, GameObject> ViewObjects;

        public DialogueViewModel()
        {
            this.MultiPlugInHub = new();
            this.ViewObjects = new();
        }

        // T 위임된 등록/해제/Get 기능
        public void Register<T>(string key, T plugIn) where T : class => this.MultiPlugInHub.Register<T>(key, plugIn);
        public void Remove<T>(string key) where T : class => this.MultiPlugInHub.Remove<T>(key);
        public bool TryGet<T>(string key, out T plugIn) where T : class => this.MultiPlugInHub.TryGet<T>(key, out plugIn);
        public bool TryGetAll<T>(out T[] plugIns) where T : class => this.MultiPlugInHub.TryGetAll<T>(out plugIns);

        // ActorObjects 등록/해제
        public void RegisterViewObject(string key, GameObject viewObject)
        {
            if (this.ViewObjects.ContainsKey(key)) return;

            this.ViewObjects.Add(key, viewObject);
        }
        public void RemoveViewObject(string key)
        {
            if (!this.ViewObjects.ContainsKey(key)) return;

            this.ViewObjects.Remove(key);
        }
        public bool TryGetViewObject(string key, out GameObject viewObject)
        {
            viewObject = null;
            if (!this.ViewObjects.ContainsKey(key)) return false;

            viewObject = this.ViewObjects[key];
            return true;
        }
    }

    // 런타임 내, 생성되는 객체의 경우, 생성과 동시에, Bind가 이루어져야 됨.
    // Generator 객체가 해당 역할을 담당함. 즉, 생명주기를 담당한다는 거지.
    // 또한, Generator의 생성자로, Bind에 사용될 참조가 전달되면서, 일종의 DI의 역할도 수행해.
    // 일종의 작은 Factory 인거지.
    public class DialogueGenerator
    {
        private IDialogueDirectingResourceDataDB DialogueResourceDataDB;
        private IDialogueDirectingPrefabResourceDB DialoguePrefabResourceDB;
        private IDialogueViewModel DialogueViewModel;

        private FadeInAndOutService FadeInAndOutService;

        public void InitialSetting(IDialogueDirectingResourceDataDB dialogueResourceDataDB, 
            IDialogueDirectingPrefabResourceDB dialoguePrefabResourceDB, IDialogueViewModel dialogueViewModel)
        {
            this.DialogueResourceDataDB = dialogueResourceDataDB;
            this.DialoguePrefabResourceDB = dialoguePrefabResourceDB;
            this.DialogueViewModel = dialogueViewModel;

            this.FadeInAndOutService = new();
        }

        public void SetDialogueService(string dialogueIndex)
        {
            if(!this.DialogueResourceDataDB.TryGetDialogueDirectingResourceData(dialogueIndex, out var dialogueDirectingResourceData)) 
            {
                Debug.Log($"Key에 대응되는 대화 연출 리소스 정보가 없습니다.");
                return;
            }


        }

        public void ResetDialogueService()
        {

        }

        public void ActorViewGenerate(DialogueDirectingPrefabData dialogueDirectingPrefabData)
        {
            GameObject newActorPrefab = MonoBehaviour.Instantiate(dialogueDirectingPrefabData.Prefab, dialogueDirectingPrefabData.PrefabParent);
            this.DialogueViewModel.RegisterViewObject(dialogueDirectingPrefabData.PrefabKey, newActorPrefab);

            var actorView = newActorPrefab.GetComponent<DialogueActorView>();
            this.DialogueViewModel.Register<IActivation>(dialogueDirectingPrefabData.PrefabKey, actorView);
            this.DialogueViewModel.Register<IFadeInAndOut>(dialogueDirectingPrefabData.PrefabKey, actorView);
            this.DialogueViewModel.Register<ISpriteSetter>(dialogueDirectingPrefabData.PrefabKey, actorView);
            this.DialogueViewModel.Register<IPositioner>(dialogueDirectingPrefabData.PrefabKey, actorView);
        }

        public void SpriteViewGenerate(DialogueDirectingPrefabData dialogueDirectingPrefabData)
        {
            GameObject newSpritePrefab = MonoBehaviour.Instantiate(dialogueDirectingPrefabData.Prefab, dialogueDirectingPrefabData.PrefabParent);
            this.DialogueViewModel.RegisterViewObject(dialogueDirectingPrefabData.PrefabKey, newSpritePrefab);

            var spriteView = newSpritePrefab.GetComponent<DialogueSpriteRendererView>();
            this.DialogueViewModel.Register<IActivation>(dialogueDirectingPrefabData.PrefabKey, spriteView);
            this.DialogueViewModel.Register<IFadeInAndOut>(dialogueDirectingPrefabData.PrefabKey, spriteView);
        }

        public void CanvasUIUXViewGenerate(DialogueDirectingPrefabData dialogueDirectingPrefabData)
        {

        }
    }

    public interface IDialogueDirectingSystemGameFlow
    {
        public void OperateDialogueDirecting(int currentDirectingIndex);

        public void OperateDialogueClickInteraction();
    }

    public class DialogueDirectingSystemGameFlow : IDialogueDirectingSystemGameFlow
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

        // Text 출력 코루틴 제어를 위한 데이터.
        private DirectingCoroutineControlData TextDirectingCoroutineControlData = new();
        // CanvasUIUX, BackGround, Actor의 Action 코루틴 제어를 위한 데이터.
        private List<DirectingCoroutineControlData> ActionDirectingCoroutineControlDatas = new();

        // 연출 Data Table
        private List<DialogueDirectingData> TotalDialogueDirectingDatas;
        //        private int currentDirectingIndex = 0;

        // 자동 재생 기능 코루틴 제어 데이터.
        private AutoPlayDirectingData AutoPlayDirectingData = new(2f);

        // 연속적인 연출 수행을 위해, 마지막으로 수행한 연출 데이터의 내용을 기록해 놓습니다.
        private DialogueDirectingData LastDirectingData = new();

        public void InitialSetting(IDialogueTextDirectingView DialogueTextDirectingView, IDialogueChoiceDirectingViewMediator DialogueChoiceDirectingViewMediator,
            IDialogueImageDirectingFacade DialogueImageDirectingFacade, IDialogueCutsceneDirectingView DialogueCutsceneDirectingView, IDialogueHistoryGenerator DialogueHistoryGenerator,
            ICoroutineRunner CoroutineRunner)
        {
            this.DialogueTextDirectingView = DialogueTextDirectingView;
            this.DialogueChoiceDirectingViewMediator = DialogueChoiceDirectingViewMediator;
            this.DialogueImageDirectingFacade = DialogueImageDirectingFacade;
            this.DialogueCutsceneDirectingView = DialogueCutsceneDirectingView;
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
            // 수행하고자 하는 연출 번호가 잘못된 경우, return.
            if (this.TotalDialogueDirectingDatas.Count <= currentDirectingIndex || this.TotalDialogueDirectingDatas[currentDirectingIndex] == null)
            {
                Debug.Log($"잘못된 연출 번호 : {currentDirectingIndex}");
                return;
            }
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

                        this.DialogueHistoryGenerator.AddDialogueHistory(currentDialogueDirectingData.Index, currentDialogueDirectingData.DirectingContent);
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
                        newDirectingCoroutineControlData.ControlCoroutine = StartCoroutine(this.OperateActionCoroutine(newDirectingCoroutineControlData.DirectingIndex));
                        newDirectingCoroutineControlData.BehaviourToken = imageBehaviourToken;

                        Debug.Log($"DirectingIndex : {newDirectingCoroutineControlData.DirectingIndex}, BehaviourCoroutine : {newDirectingCoroutineControlData.BehaviourCoroutine}," +
                            $" BehaviourCoroutine : {newDirectingCoroutineControlData.BehaviourCoroutine}, BehaviourToken : {newDirectingCoroutineControlData.BehaviourToken.IsRequestEnd}");
                    }
                    break;
                default:
                    break;
            }

            this.LastDirectingData.Index = currentDialogueDirectingData.Index;
            this.LastDirectingData.IsSkipable = currentDialogueDirectingData.IsSkipable;
            this.LastDirectingData.IsAutoable = currentDialogueDirectingData.IsAutoable;
            this.LastDirectingData.NextDirectiveCommand = currentDialogueDirectingData.NextDirectiveCommand;

            if (currentDialogueDirectingData.IsChainWithNext)
            {
                if (this.TryGetNextDirectingIndex(currentDialogueDirectingData.NextDirectiveCommand, out var nextIndex))
                {
                    this.OperateDialogueDirecting(nextIndex);
                    return;
                }
            }
        }


        // Action 코루틴 제어
        private IEnumerator OperateActionCoroutine(int directingIndex)
        {
            DirectingCoroutineControlData thisDirectingCoroutineControlData = this.ActionDirectingCoroutineControlDatas.Find(x => x.DirectingIndex == directingIndex);
           
            yield return thisDirectingCoroutineControlData.BehaviourCoroutine;

            thisDirectingCoroutineControlData.BehaviourCoroutine = null;
            thisDirectingCoroutineControlData.ControlCoroutine = null;
            this.ActionDirectingCoroutineControlDatas.Remove(thisDirectingCoroutineControlData);

            this.RequestNextDirecting();
        }
        // Text 코루틴 제어
        private IEnumerator OperateTextDisplayCoroutine()
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
            if (this.LastDirectingData.IsAutoable && this.TextDirectingCoroutineControlData.IsOperationEnd() && this.ActionDirectingCoroutineControlDatas.Count == 0)
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
            // 해당 연출이 마우스클릭을 통해 생략이 가능한지 여부.
            if (!this.LastDirectingData.IsSkipable) return;

            // 연출 중이 아니라면, 다음 시퀀스 스탭 시작.
            if (this.ActionDirectingCoroutineControlDatas.Count == 0 && this.TextDirectingCoroutineControlData.IsOperationEnd())
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
                this.StartCoroutine(this.OperateDialogueAutoDisplay());
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
                if (this.ActionDirectingCoroutineControlDatas.Count == 0 && this.TextDirectingCoroutineControlData.IsOperationEnd())
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
            if (!this.TextDirectingCoroutineControlData.IsOperationEnd())
            {
                this.TextDirectingCoroutineControlData.BehaviourToken.IsRequestEnd = true;
            }

            foreach (var controlData in this.ActionDirectingCoroutineControlDatas)
            {
                controlData.BehaviourToken.IsRequestEnd = true;
            }
        }
    }

    public class AutoPlayDirectingData
    {
        public Coroutine AutoCoroutine { get; set; }

        public float AutoWaitDuration { get; set; }
        public float CurrentWaitDuration { get; set; }

        public bool IsRequestEnd { get; set; }

        public AutoPlayDirectingData(float duration)
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