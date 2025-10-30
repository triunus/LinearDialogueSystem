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
    public interface IDialoguePrefabResourceDB
    {
        public bool TryGetPrefabData(string prefabKey, out DialoguePrefabData prefabData);
    }

    public class DialoguePrefabResourceDB : MonoBehaviour
    {
        [SerializeField] private List<DialoguePrefabData> DialoguePrefabDatas;

        public bool TryGetPrefabData(string prefabKey, out DialoguePrefabData prefabData)
        {
            prefabData = null;

            foreach (var data in this.DialoguePrefabDatas)
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
    public class DialoguePrefabData
    {
        [SerializeField] private string prefabKey;
        [SerializeField] private DialogueViewType dialogueViewType;

        [SerializeField] private Transform prefabParent;
        [SerializeField] private GameObject prefab;

        public string PrefabKey { get => prefabKey; }
        public DialogueViewType DialogueViewType { get => this.dialogueViewType; }
        public Transform PrefabParent { get => prefabParent; }
        public GameObject Prefab { get => prefab; }
    }

    [System.Serializable]
    public enum DialogueViewType
    {
        SpriteView,
        CanvasUIUXView,
        ActorView,
    }

    public class DialogueDirectingService
    {
        private IDialoguePrefabResourceDB DialoguePrefabResourceDB;
        private ICoroutineRunner CoroutineRunner;

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

        // �ݺ����� �׽�Ʈ�� ���� �ӽð�.
        private bool IsActivated_Temp = false;

        public DialogueDirectingService(IDialoguePrefabResourceDB dialoguePrefabResourceDB, ICoroutineRunner coroutineRunner)
        {
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


        // �׽�Ʈ�� ���� �ӽ÷� �����ϱ� ���� ���
        public void TestButtonOperation()
        {
            // �ݺ����� ��� �׽�Ʈ�� ���� �ӽð�.
            if (this.IsActivated_Temp) return;
            else this.IsActivated_Temp = true;

            this.DialogueDirectingSystemGameFlow.OperateDialogueDirecting(0);
        }
    }

    // ��Ÿ�� ��, �����Ǵ� ��ü�� ���, ������ ���ÿ�, Bind�� �̷������ ��.
    // Generator ��ü�� �ش� ������ �����. ��, �����ֱ⸦ ����Ѵٴ� ����.
    // ����, Generator�� �����ڷ�, Bind�� ���� ������ ���޵Ǹ鼭, ������ DI�� ���ҵ� ������.
    // ������ ���� Factory �ΰ���.
    public class DialogueGenerator
    {
        private IDialoguePrefabResourceDB DialoguePrefabResourceDB;

        private PlainServices.FadeInAndOutService FadeInAndOutService;

        private IPlugInHub<IActivation> ActivationPlugInHub;
        private IPlugInHub<IFadeInAndOut> FaderPlugInHub;
        private IPlugInHub<ISpriteSetter> SpriteSetterPlugInHub;
        private IPlugInHub<IPositioner> PositonerPlugInHub;

        private Dictionary<string, GameObject> ActorObjects;

        public void InitialSetting(IPlugInHub<IActivation> activationPlugInHub, IPlugInHub<IFadeInAndOut> faderPlugInHub,
            IPlugInHub<ISpriteSetter> spriteSetterPlugInHub, IPlugInHub<IPositioner> positonerPlugInHub)
        {
            this.ActivationPlugInHub = activationPlugInHub;
            this.FaderPlugInHub = faderPlugInHub;
            this.SpriteSetterPlugInHub = spriteSetterPlugInHub;
            this.PositonerPlugInHub = positonerPlugInHub;

            this.ActorObjects = new();
        }

        public void Generate(string key, SpriteAttitudeTexture2D[] attitudeTexture2Ds, SpriteFaceTexture2D[] faceTexture2Ds)
        {
            GameObject newActorPrefab = Instantiate(this.ActorViewObject.ViewObjectPrefab, this.ActorViewObject.ObjectParent);
            this.ActorObjects.Add(key, newActorPrefab);

            var actorView = newActorPrefab.GetComponent<DialogueActorView>();
            actorView.IntialSetTexture2D(key, attitudeTexture2Ds, faceTexture2Ds);

            // Bind
            this.ActivationPlugInHub.RegisterPlugIn(key, actorView);
            this.FaderPlugInHub.RegisterPlugIn(key, actorView);
            this.SpriteSetterPlugInHub.RegisterPlugIn(key, actorView);
            this.PositonerPlugInHub.RegisterPlugIn(key, actorView);
        }

        // Bind ���� �� ����.
        public void Remove(string key)
        {
            this.ActivationPlugInHub.RemovePlugIn(key);
            this.FaderPlugInHub.RemovePlugIn(key);
            this.SpriteSetterPlugInHub.RemovePlugIn(key);
            this.PositonerPlugInHub.RemovePlugIn(key);

            Destroy(this.ActorObjects[key]);
            this.ActorObjects.Remove(key);
        }
    }

    public class ActorGenerator : MonoBehaviour
    {
        [SerializeField] private UnityViewObjectData ActorViewObject;

        private IPlugInHub<IActivation> ActivationPlugInHub;
        private IPlugInHub<IFadeInAndOut> FaderPlugInHub;
        private IPlugInHub<ISpriteSetter> SpriteSetterPlugInHub;
        private IPlugInHub<IPositioner> PositonerPlugInHub;

        private Dictionary<string, GameObject> ActorObjects;

        public void InitialSetting(IPlugInHub<IActivation> activationPlugInHub, IPlugInHub<IFadeInAndOut> faderPlugInHub,
            IPlugInHub<ISpriteSetter> spriteSetterPlugInHub, IPlugInHub<IPositioner> positonerPlugInHub)
        {
            this.ActivationPlugInHub = activationPlugInHub;
            this.FaderPlugInHub = faderPlugInHub;
            this.SpriteSetterPlugInHub = spriteSetterPlugInHub;
            this.PositonerPlugInHub = positonerPlugInHub;

            this.ActorObjects = new();
        }

        // ���� �� Bind
        public void Generate(string key, SpriteAttitudeTexture2D[] attitudeTexture2Ds, SpriteFaceTexture2D[] faceTexture2Ds)
        {
            GameObject newActorPrefab = Instantiate(this.ActorViewObject.ViewObjectPrefab, this.ActorViewObject.ObjectParent);
            this.ActorObjects.Add(key, newActorPrefab);

            var actorView = newActorPrefab.GetComponent<DialogueActorView>();
            actorView.IntialSetTexture2D(key, attitudeTexture2Ds, faceTexture2Ds);

            // Bind
            this.ActivationPlugInHub.RegisterPlugIn(key, actorView);
            this.FaderPlugInHub.RegisterPlugIn(key, actorView);
            this.SpriteSetterPlugInHub.RegisterPlugIn(key, actorView);
            this.PositonerPlugInHub.RegisterPlugIn(key, actorView);
        }

        // Bind ���� �� ����.
        public void Remove(string key)
        {
            this.ActivationPlugInHub.RemovePlugIn(key);
            this.FaderPlugInHub.RemovePlugIn(key);
            this.SpriteSetterPlugInHub.RemovePlugIn(key);
            this.PositonerPlugInHub.RemovePlugIn(key);

            Destroy(this.ActorObjects[key]);
            this.ActorObjects.Remove(key);
        }
    }

    public class SpriteGenerator : MonoBehaviour
    {
        [SerializeField] private UnityViewObjectData SpriteViewObject;

        private IPlugInHub<IActivation> ActivationPlugInHub;
        private IPlugInHub<IFadeInAndOut> FaderPlugInHub;

        private Dictionary<string, GameObject> SpriteObjects;

        public void InitialSetting(IPlugInHub<IActivation> activationPlugInHub, IPlugInHub<IFadeInAndOut> faderPlugInHub)
        {
            this.ActivationPlugInHub = activationPlugInHub;
            this.FaderPlugInHub = faderPlugInHub;

            this.SpriteObjects = new();
        }

        // ���� �� Bind
        public void Generate(string key, Texture2D spriteTexture2D)
        {
            GameObject newActorPrefab = Instantiate(this.SpriteViewObject.ViewObjectPrefab, this.SpriteViewObject.ObjectParent);
            this.SpriteObjects.Add(key, newActorPrefab);

            var spriteView = newActorPrefab.GetComponent<DialogueSpriteRendererView>();

            // Bind
            this.ActivationPlugInHub.RegisterPlugIn(key, spriteView);
            this.FaderPlugInHub.RegisterPlugIn(key, spriteView);
        }

        // Bind ���� �� ����.
        public void Remove(string key)
        {
            this.ActivationPlugInHub.RemovePlugIn(key);
            this.FaderPlugInHub.RemovePlugIn(key);

            Destroy(this.SpriteObjects[key]);
            this.SpriteObjects.Remove(key);
        }
    }

    public interface IDialogueDirectingSystemGameFlow
    {
        public void OperateDialogueDirecting(int currentDirectingIndex);

        public void OperateDialogueClickInteraction();
    }

    public class DialogueDirectingSystemGameFlow : IDialogueDirectingSystemGameFlow
    {
        // Text ��� View
        private IDialogueTextDirectingView DialogueTextDirectingView;
        // ������ ��� View
        private IDialogueChoiceDirectingViewMediator DialogueChoiceDirectingViewMediator;
        // Image ���� Hub �ۻ��
        private IDialogueImageDirectingFacade DialogueImageDirectingFacade;

        // ���Ǹ� ���� View
        // Cutscene ��� View
        private IDialogueCutsceneDirectingView DialogueCutsceneDirectingView;

        // ����� �ٸ�, ��ȭ History ���� View
        private IDialogueHistoryGenerator DialogueHistoryGenerator;

        private ICoroutineRunner CoroutineRunner;

        // Text ��� �ڷ�ƾ ��� ���� ������.
        private DirectingCoroutineControlData TextDirectingCoroutineControlData = new();
        // CanvasUIUX, BackGround, Actor�� Action �ڷ�ƾ ��� ���� ������.
        private List<DirectingCoroutineControlData> ActionDirectingCoroutineControlDatas = new();

        // ���� Data Table
        private List<DialogueDirectingData> TotalDialogueDirectingDatas;
        //        private int currentDirectingIndex = 0;

        // �ڵ� ��� ��� �ڷ�ƾ ���� ������.
        private AutoPlayDirectingData AutoPlayDirectingData = new(2f);

        // �������� ���� ������ ����, ���������� ������ ���� �������� ������ ����� �����ϴ�.
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
                new DialogueDirectingData(32, "DialogueTextDirectingType", "ActorA_Actor A_�ϴ� ���⼭ �������Ұ�", false, true, false, "Next"),
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
            // �����ϰ��� �ϴ� ���� ��ȣ�� �߸��� ���, return.
            if (this.TotalDialogueDirectingDatas.Count <= currentDirectingIndex || this.TotalDialogueDirectingDatas[currentDirectingIndex] == null)
            {
                Debug.Log($"�߸��� ���� ��ȣ : {currentDirectingIndex}");
                return;
            }
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

                        this.DialogueHistoryGenerator.AddDialogueHistory(currentDialogueDirectingData.Index, currentDialogueDirectingData.DirectingContent);
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


        // Action �ڷ�ƾ ����
        private IEnumerator OperateActionCoroutine(int directingIndex)
        {
            DirectingCoroutineControlData thisDirectingCoroutineControlData = this.ActionDirectingCoroutineControlDatas.Find(x => x.DirectingIndex == directingIndex);
           
            yield return thisDirectingCoroutineControlData.BehaviourCoroutine;

            thisDirectingCoroutineControlData.BehaviourCoroutine = null;
            thisDirectingCoroutineControlData.ControlCoroutine = null;
            this.ActionDirectingCoroutineControlDatas.Remove(thisDirectingCoroutineControlData);

            this.RequestNextDirecting();
        }
        // Text �ڷ�ƾ ����
        private IEnumerator OperateTextDisplayCoroutine()
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
            if (this.LastDirectingData.IsAutoable && this.TextDirectingCoroutineControlData.IsOperationEnd() && this.ActionDirectingCoroutineControlDatas.Count == 0)
            {
                if (this.TryGetNextDirectingIndex(this.LastDirectingData.NextDirectiveCommand, out var index))
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
                    nextDirectingIndex = this.LastDirectingData.Index + 1;
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
            // �ش� ������ ���콺Ŭ���� ���� ������ �������� ����.
            if (!this.LastDirectingData.IsSkipable) return;

            // ���� ���� �ƴ϶��, ���� ������ ���� ����.
            if (this.ActionDirectingCoroutineControlDatas.Count == 0 && this.TextDirectingCoroutineControlData.IsOperationEnd())
            {
                // ������ ������ DirectingIndex ���� Ȯ��.
                if (this.TryGetNextDirectingIndex(this.LastDirectingData.NextDirectiveCommand, out var index))
                {
                    // �����Ͽ��� ��, �ش� ���� ����.
                    this.OperateDialogueDirecting(index);
                    this.AutoPlayDirectingData.CurrentWaitDuration = 0;
                }
            }
            // ���� ���̶��, ���� �� ���� ���.
            else
            {
                this.StopDialogueDirection();
            }
        }


        // Auto ��� ���
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

                //  ���� ��� ���� �ڷ�ƾ�� ������ �׳� �Ѿ.
                if (this.ActionDirectingCoroutineControlDatas.Count == 0 && this.TextDirectingCoroutineControlData.IsOperationEnd())
                {
                    // ����� ��ٷ�����, ���� ��ȭ ���� ���� ��û.
                    if (this.AutoPlayDirectingData.CurrentWaitDuration >= this.AutoPlayDirectingData.AutoWaitDuration)
                    {
                        if (this.TryGetNextDirectingIndex(this.LastDirectingData.NextDirectiveCommand, out var index))
                        {
                            this.OperateDialogueDirecting(index);
                        }

                        this.AutoPlayDirectingData.CurrentWaitDuration = 0;
                    }
                    // ���� ��� ���� �ڷ�ƾ�� ������ �ð� ����.
                    else
                    {
                        this.AutoPlayDirectingData.CurrentWaitDuration += Time.deltaTime;
                    }
                }

                yield return Time.deltaTime;
            }

            // Auto �ڷ�ƾ �ʱ�ȭ.
            this.AutoPlayDirectingData.Reset();
        }


        // Skip ���
        public void OnClickedSkipButton()
        {
            // ��ŵ ��� ���ҽ� �Ѿ.
            if (!this.LastDirectingData.IsSkipable) return;

            this.StopDialogueDirection();
        }


        // ���� ����, �ڷ�ƾ ������ �ִٸ�, ��ū�� ���Ͽ� �ڿ������� �������� �ǵ��� �����Ѵ�.
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