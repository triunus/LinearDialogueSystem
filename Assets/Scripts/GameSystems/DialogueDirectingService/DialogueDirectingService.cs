using System.Collections.Generic;

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
        // 연출 시스템에서 사용되는 ViewModel의 생성 및 삭제 / 관리 역할을 갖습니다.
        private DialogueViewObjectData DialogueViewObjectData;
        private DialogueViewObjectGenerator DialogueViewObjectGenerator;
        private DialogueDefaultObjectGenerator DialogueDefaultObjectGenerator;

        private DialogueViewObjectDataHandler DialogueViewObjectDataHandler;

        private DialogueDirectingControlDataGenerator DialogueDirectingControlDataGenerator;
        private DialogueDirectingControlData DialogueDirectingControlData;


        // 연출에 사용되는 상위 GameFlow
        private DialogueDirectingMainFlow DialogueDirectingMainFlow;
        [SerializeField] private DialogueDirectingInputController DialogueDirectingInputController;
        // 상위 GameFlow에 사용되는 서비스
        [SerializeField] private CoroutineRunner CoroutineRunner;

        // 반복적인 테스트를 위한 임시값.
        private bool IsDialogueServiceSetting = false;
        private bool IsActivated = false;

        private void Awake()
        {
            this.DialogueViewObjectData = new();
            this.DialogueViewObjectGenerator = new(this.DialogueViewObjectData, this.DialogueViewObjectData, this.DialogueViewObjectData);
            this.DialogueDefaultObjectGenerator = new(this.DialogueViewObjectData, this.DialogueViewObjectData, this.DialogueViewObjectData);

            this.DialogueViewObjectDataHandler = new(this.DialogueViewObjectData, this.DialogueViewObjectGenerator);

            this.DialogueDirectingControlData = new();
            this.DialogueDirectingControlDataGenerator = new(this.DialogueDirectingControlData);

            this.DialogueDirectingMainFlow = new(
                dialogueViewObjectDataHandler: this.DialogueViewObjectDataHandler,
                dialogueDirectingControlData: this.DialogueDirectingControlData,
                coroutineRunner: this.CoroutineRunner);

            this.DialogueDirectingInputController.InitialSetting(
                dialogueDirectingGameFlow: this.DialogueDirectingMainFlow,
                dialogueDirectingControlData: this.DialogueDirectingControlData);
        }

        // 임시 테스트
        public void OperateDialogueInitialSetting()
        {
            // Default 객체 생성.
            this.DialogueDefaultObjectGenerator.GenerateAllDefaultGameObject();

            // 현재 DialogueDirecting에 필요한 객체 정보를 '매개변수로 받아옴'
            DialogueResoruceData tempRialogueResoruceData = new()
            {
                DialogueIndex = 1,
                PrefabKeys = new List<string>() { "DialogueButtomTextDirectingView" }
            };
            // 현재 DialogueDirecting에 필요한 객체 생성.
            this.DialogueViewObjectGenerator.GenerateAllNeedViewObject(tempRialogueResoruceData);
        }
       
        public void OperateDialogueServiceSetting_Test()
        {
            // 반복적인 재생 테스트를 위한 임시값.
            if (this.IsDialogueServiceSetting) return;
            else this.IsDialogueServiceSetting = true;
  
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

    public class DialogueResoruceData
    {
        public int DialogueIndex;
        public List<string> PrefabKeys;
    }
}