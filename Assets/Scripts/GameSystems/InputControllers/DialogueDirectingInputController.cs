using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

using GameSystems.DialogueDirectingService.GameFlow;
using GameSystems.DialogueDirectingService.Datas;

namespace GameSystems.DialogueDirectingService.InputControllers
{
    public class DialogueDirectingInputController : MonoBehaviour
    {
        private IDialogueDirectingGameFlow DialogueDirectingGameFlow;
        private IDialogueDirectingControlData DialogueDirectingControlData;

        public void InitialSetting(IDialogueDirectingControlData dialogueDirectingControlData, IDialogueDirectingGameFlow dialogueDirectingGameFlow)
        {
            this.DialogueDirectingGameFlow = dialogueDirectingGameFlow;
            this.DialogueDirectingControlData = dialogueDirectingControlData;
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            // UI 최상단 요소를 가져오기.
            GameObject topUI = GetTopmostUIUnderPointer();
            if (topUI == null) return; // UI가 아니면 무시.

            // 최상단 UI에 IAllowMouseClick이 있으면 통과
            var allow = topUI.GetComponent<IUIMouseClickAvailable>();
            if (allow != null)
            {
                if (DialogueDirectingGameFlow != null)
                {
                    DialogueDirectingGameFlow.OperateDialogueClickInteraction();
                }
            }
        }

        private GameObject GetTopmostUIUnderPointer()
        {
            // 이벤트 시스템 없으면 리턴.
            if (EventSystem.current == null)return null;    

            // 
            var pointer = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, results);

            // EventSystem은 우선순위/정렬 순서대로 정렬된 결과를 반환한다.
            // 첫 번째가 화면상 최상단 UI
            return results.Count > 0 ? results[0].gameObject : null;
        }
    }
}
