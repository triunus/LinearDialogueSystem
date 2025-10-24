
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameSystems.InputControllers.MainStageScene
{
    public class UIImageClickView : MonoBehaviour, IPointerClickHandler
    {
        private IDialogueInputController DialogueInputController;

        private void Awake()
        {
            var LocalRepository = Repository.MainStageSceneRepository.Instance;

            this.DialogueInputController = LocalRepository.InputController_LazyReferenceRepository.
                GetOrWaitReference<DialogueInputController>(x => this.DialogueInputController = x);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            this.DialogueInputController.OperateDialogueClickInteraction();
        }
    }
}