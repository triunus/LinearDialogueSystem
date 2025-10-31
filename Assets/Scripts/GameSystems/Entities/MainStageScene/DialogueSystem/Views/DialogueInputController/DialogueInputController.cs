using System.Collections;

using UnityEngine;

using GameSystems.GameFlows.MainStageScene;

namespace GameSystems.InputControllers.MainStageScene
{
    public interface IDialogueInputController
    {
        public void OperateDialogueClickInteraction();
    }

    public class DialogueInputController : MonoBehaviour, IInputController, IDialogueInputController
    {
        private IDialogueDirectingGameFlow DialogueDirectingSystemGameFlow;

        private bool duplicatedInputBlock;
        private float duplicatedInputBlockDuration;

        private void Awake()
        {
            var LocalRepository = Repository.MainStageSceneRepository.Instance;

            LocalRepository.InputController_LazyReferenceRepository.RegisterReference<DialogueInputController>(this);

            this.DialogueDirectingSystemGameFlow = LocalRepository.GameFlow_LazyReferenceRepository.
                GetOrWaitReference<DialogueDirectingGameFlow>(x => this.DialogueDirectingSystemGameFlow = x);

            this.duplicatedInputBlock = false;
        }

        public void OperateDialogueClickInteraction()
        {
            // 현재 Block 중이면 리턴.
            if (duplicatedInputBlock) return;
    
            // 아니면 Block 시작
            this.duplicatedInputBlock = true;
            StartCoroutine(this.BlockWait());

            this.DialogueDirectingSystemGameFlow.OperateDialogueClickInteraction();
        }

        private IEnumerator BlockWait()
        {
            while (this.duplicatedInputBlockDuration < 0.5f)
            {
                this.duplicatedInputBlockDuration += Time.deltaTime;
                yield return null;
            }

            this.duplicatedInputBlockDuration = 0;
            this.duplicatedInputBlock = false;
        }
    }
}