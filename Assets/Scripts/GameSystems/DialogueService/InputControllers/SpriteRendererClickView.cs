using UnityEngine;

using GameSystems.GameFlows.MainStageScene;

namespace GameSystems.DialogueDirectingService.InputControllers
{
    public class SpriteRendererClickView : MonoBehaviour
    {
        private IDialogueInputController DialogueInputController;

        private void Awake()
        {
            var LocalRepository = Repository.MainStageSceneRepository.Instance;

            this.DialogueInputController = LocalRepository.InputController_LazyReferenceRepository.
                GetOrWaitReference<DialogueInputController>(x => this.DialogueInputController = x);
        }

        void OnMouseDown()
        {
            this.DialogueInputController.OperateDialogueClickInteraction();
        }

    }
}