using UnityEngine;

using GameSystems.UnityServices;
using GameSystems.DialogueDirectingService.Datas;
using GameSystems.DialogueDirectingService.Generator;
using GameSystems.DialogueDirectingService.GameFlow;
using GameSystems.DialogueDirectingService.InputControllers;

namespace GameSystems.DialogueDirectingService
{
    public class DialogueDirectingService : MonoBehaviour
    {
        // 연출 시스템에서 사용되는 ViewModel
        private DialogueViewObjectData DialogueViewObjectData;
        private DialogueDirectingControlData DialogueDirectingControlData;

        // DialogueViewModel의 생명주기를 담당하는 객체
        private DialogueViewObjectGenerator DialogueViewObjectGenerator;
        private DialogueDirectingControlDataGenerator DialogueDirectingControlDataGenerator;

        // 연출에 사용되는 상위 GameFlow
        private DialogueDirectingMainFlow DialogueDirectingMainFlow;
        [SerializeField] private DialogueDirectingInputController DialogueDirectingInputController;
        // 상위 GameFlow에 사용되는 서비스
        [SerializeField] private CoroutineRunner CoroutineRunner;


/*        // 아직 안 건듬.
        [SerializeField] private DialogueTextDirectingView DialogueTextDirectingView;
        // 아직 안 건듬.
        [SerializeField] private DialogueChoiceDirectingViewMediator DialogueChoiceDirectingViewMediator;
        // 아직 안 건듬.
        [SerializeField] private DialogueCutsceneDirectingView DialogueCutsceneDirectingView;
        // 아직 안 건듬.
        [SerializeField] private DialogueHistoryGenerator DialogueHistoryGenerator;*/



        // 반복적인 테스트를 위한 임시값.
        private bool IsDialogueServiceSetting = false;
        private bool IsActivated = false;

        public DialogueDirectingService()
        {
            this.DialogueViewObjectData = new();
            this.DialogueDirectingControlData = new();

            this.DialogueViewObjectGenerator = new(this.DialogueViewObjectData);
            this.DialogueDirectingControlDataGenerator = new(this.DialogueDirectingControlData);

            this.DialogueDirectingMainFlow = new(this.DialogueViewObjectData, this.DialogueDirectingControlData, this.CoroutineRunner);
            this.DialogueDirectingInputController.InitialSetting(this.DialogueDirectingControlData, this.DialogueDirectingMainFlow);
        }
       
        public void OperateDialogueServiceSetting_Test()
        {
            // 반복적인 재생 테스트를 위한 임시값.
            if (this.IsDialogueServiceSetting) return;
            else this.IsDialogueServiceSetting = true;

            this.DialogueViewObjectGenerator.SetDialogueResource(0);
            this.DialogueDirectingControlDataGenerator.SetDialogueResoruce(0);
        }

        public void OperateDialogueService_Test()
        {
            // 반복적인 재생 테스트를 위한 임시값.
            if (this.IsActivated) return;
            else this.IsActivated = true;

            this.DialogueDirectingMainFlow.OperateDialogueDirecting(0);
        }
    }
}