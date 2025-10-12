using System.Collections;
using UnityEngine;
using TMPro;

using GameSystems.DTOs;

namespace GameSystems.Entities.MainStageScene
{

    // TextDisplay
    public class DialogueTextDisplayView : MonoBehaviour, IEntity
    {
        [SerializeField] private TextMeshProUGUI SpeakerUI;
        [SerializeField] private TextMeshProUGUI dialogueTextUI;

        [SerializeField] private float charsPerSecond = 20;

        private void Awake()
        {
            var LocalRepository = Repository.MainStageSceneRepository.Instance;

            LocalRepository.Entity_LazyReferenceRepository.RegisterReference<DialogueTextDisplayView>(this);
        }

        public bool TryGetTextDisplayOperation(SayDirectionTextData sayDirectionTextData, out IEnumerator enumerator)
        {
            enumerator = null;
            if (this.dialogueTextUI == null || this.SpeakerUI == null) return false;

            // 초기화.
            this.SpeakerUI.text = string.Empty;
            this.dialogueTextUI.text = string.Empty;

            // 화자 이름 등록.
            this.SpeakerUI.text = sayDirectionTextData.SpeakerName;

            // 속도 값이 이상하면, 바로 출력.
            if (charsPerSecond <= 0f)
            {
                this.dialogueTextUI.text = sayDirectionTextData.Content;
                return false;
            }

            Debug.Log($"speaker : {sayDirectionTextData.SpeakerName}, Content : {sayDirectionTextData.Content}");

            enumerator = this.OperateDialogueTextDisplay(sayDirectionTextData.Content);
            return true;
        }

        public IEnumerator OperateDialogueTextDisplay(string content)
        {
            float delay = 1f / charsPerSecond;
            WaitForSeconds wait = new WaitForSeconds(delay);

            // 가장 단순한 방식: 글자 하나씩 붙이기
            for (int i = 0; i < content.Length; i++)
            {
                this.dialogueTextUI.text += content[i];
                yield return wait;
            }
        }
    }
}