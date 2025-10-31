
using UnityEngine;

using GameSystems.DTOs;
using GameSystems.UnityServices;
using GameSystems.Entities.MainStageScene;

namespace GameSystems.GameFlows.MainStageScene
{
    public class DialogueDirectingService : MonoBehaviour
    {
        // 연출 시스템에서 사용되는 유니티 리소스 DB
        [SerializeField] private DialogueDirectingResourceDataDB  DialogueDirectingResourceDataDB;
        [SerializeField] private DialogueDirectingPrefabResourceDB DialogueDirectingPrefabResourceDB;

        // 연출 시스템에서 사용되는 ViewModel
        private DialogueViewModel DialogueViewModel;
        // DialogueViewModel의 생명주기를 담당하는 객체
        private DialogueViewObjectGenerator DialogueGenerator;

        // 연출에 사용되는 상위 GameFlow
        private DialogueDirectingGameFlow DialogueDirectingSystemGameFlow;

        // Image 연출에 사용되는 하위 GameFlow
        private DialogueImageDirectingFacade DialogueImageDirectingFacade;

        // 상위 GameFlow에 사용되는 서비스
        [SerializeField] private CoroutineRunner CoroutineRunner;


        // 아직 안 건듬.
        [SerializeField] private DialogueTextDirectingView DialogueTextDirectingView;
        // 아직 안 건듬.
        [SerializeField] private DialogueChoiceDirectingViewMediator DialogueChoiceDirectingViewMediator;
        // 아직 안 건듬.
        [SerializeField] private DialogueCutsceneDirectingView DialogueCutsceneDirectingView;
        // 아직 안 건듬.
        [SerializeField] private DialogueHistoryGenerator DialogueHistoryGenerator;



        // 반복적인 테스트를 위한 임시값.
        private bool IsDialogueServiceSetting = false;
        private bool IsActivated = false;

        public DialogueDirectingService()
        {
            this.DialogueViewModel = new();

            this.DialogueGenerator = new(this.DialogueDirectingResourceDataDB, this.DialogueDirectingPrefabResourceDB, this.DialogueViewModel);

            this.DialogueImageDirectingFacade = new(this.DialogueViewModel);

            this.DialogueDirectingSystemGameFlow = new(this.DialogueTextDirectingView, this.DialogueChoiceDirectingViewMediator, this.DialogueCutsceneDirectingView, this.DialogueHistoryGenerator, this.DialogueImageDirectingFacade, this.CoroutineRunner);
        }
       
        public void OperateDialogueServiceSetting_Test()
        {
            // 반복적인 재생 테스트를 위한 임시값.
            if (this.IsDialogueServiceSetting) return;
            else this.IsDialogueServiceSetting = true;

            this.DialogueGenerator.SetDialogueResource("0");
        }

        public void OperateDialogueService_Test()
        {
            // 반복적인 재생 테스트를 위한 임시값.
            if (this.IsActivated) return;
            else this.IsActivated = true;

            this.DialogueDirectingSystemGameFlow.OperateDialogueDirecting(0);
        }
    }
}