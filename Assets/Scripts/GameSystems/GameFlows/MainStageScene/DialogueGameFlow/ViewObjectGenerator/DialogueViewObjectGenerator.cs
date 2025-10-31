
using UnityEngine;

using GameSystems.DTOs;
using GameSystems.Entities.MainStageScene;
using GameSystems.PlainServices;

namespace GameSystems.GameFlows.MainStageScene
{
    public interface IDialogueDirectingDataGenerator
    {

    }

    public class DialogueDirectingDataGenerator : IDialogueDirectingDataGenerator
    {
        private IDialogueDirectingResourceDataDB DialogueDirectingResourceDataDB;

        public DialogueDirectingDataGenerator()
        {
        }
    }

    public interface IDialogueViewObjectGenerator
    {
        public void SetDialogueResource(string dialogueIndex);
        public void ResetDialogueService();
        public void GenerateViewObject(DialogueDirectingPrefabData dialogueDirectingPrefabData);
    }

    // 런타임 내, 생성되는 객체의 경우, 생성과 동시에, Bind가 이루어져야 됨.
    // Generator 객체가 해당 역할을 담당함. 즉, 생명주기를 담당한다는 거지.
    // 또한, Generator의 생성자로, Bind에 사용될 참조가 전달되면서, 일종의 DI의 역할도 수행해.
    // 일종의 작은 Factory 인거지.
    public class DialogueViewObjectGenerator
    {
        private IDialogueDirectingResourceDataDB DialogueDirectingResourceDataDB;
        private IDialogueDirectingPrefabResourceDB DialogueDirectingPrefabResourceDB1;
        private IDialogueViewObjectModel DialogueViewModel;

        private FadeInAndOutService FadeInAndOutService;

        public DialogueViewObjectGenerator(IDialogueDirectingResourceDataDB dialogueDirectingResourceDataDB, IDialogueDirectingPrefabResourceDB dialogueDirectingPrefabResourceDB, IDialogueViewObjectModel dialogueViewModel)
        {
            this.DialogueDirectingResourceDataDB = dialogueDirectingResourceDataDB;
            this.DialogueDirectingPrefabResourceDB1 = dialogueDirectingPrefabResourceDB;
            this.DialogueViewModel = dialogueViewModel;

            this.FadeInAndOutService = new();
        }

        public void SetDialogueResource(string dialogueIndex)
        {
            if(!this.DialogueDirectingResourceDataDB.TryGetDialogueDirectingResourceData(dialogueIndex, out var dialogueDirectingResourceData)) 
            {
                Debug.Log($"Key에 대응되는 대화 연출 리소스 정보가 없습니다.");
                return;
            }


        }

        public void ResetDialogueService()
        {

        }

        public void GenerateViewObject(DialogueDirectingPrefabData dialogueDirectingPrefabData)
        {
            GameObject newActorPrefab = MonoBehaviour.Instantiate(dialogueDirectingPrefabData.Prefab, dialogueDirectingPrefabData.PrefabParent);
            this.DialogueViewModel.RegisterViewObject(dialogueDirectingPrefabData.PrefabKey, newActorPrefab);

            var actorView = newActorPrefab.GetComponent<IDialogueViewBinding>();
            actorView.InitialBinding(dialogueDirectingPrefabData.PrefabKey, this.DialogueViewModel, this.FadeInAndOutService);
        }
    }

    public interface IDialogueViewBinding
    {
        public void InitialBinding(string key, IDialogueViewObjectModel dialogueViewModel, IFadeInAndOutService fadeInAndOutService = null);
    }
}