using System.Collections;
using UnityEngine;
using TMPro;

namespace GameSystems.Entities.MainStageScene
{
    public interface IDialogueTextDirectingView
    {
        public bool TryDirectTextDisplayOperation(string directingContent, out IEnumerator enumerator);
    }

    // TextDisplay
    public class DialogueTextDirectingView : MonoBehaviour, IEntity, IDialogueTextDirectingView
    {
        [SerializeField] private TextMeshProUGUI SpeakerTextUI;
        [SerializeField] private TextMeshProUGUI DialogueTextUI;

        [SerializeField] private float charsPerSecond = 20;

        private void Awake()
        {
            var LocalRepository = Repository.MainStageSceneRepository.Instance;

            LocalRepository.Entity_LazyReferenceRepository.RegisterReference<DialogueTextDirectingView>(this);
        }

        public bool TryDirectTextDisplayOperation(string directingContent, out IEnumerator enumerator)
        {
            enumerator = null;
            if (this.DialogueTextUI == null || this.SpeakerTextUI == null) return false;
            // Parsing 실패.
            if (!this.TryParseTextContent(directingContent, out var parsedContent)) return false;

            // 초기화.
            this.SpeakerTextUI.text = string.Empty;
            this.DialogueTextUI.text = string.Empty;

            Debug.Log($"speaker : {parsedContent[0]}, Content : {parsedContent[1]}");

            // 화자 이름 등록.
            this.SpeakerTextUI.text = parsedContent[0];
            // 대사 출력 IEnumerator 할당
            enumerator = this.OperateDialogueTextDisplay(parsedContent[1], this.charsPerSecond);

            return true;
        }
        private IEnumerator OperateDialogueTextDisplay(string content, float charPerSecond)
        {
            // 출력 속도 값이 잘못되어 있으면, 20f으로 변경.
            if (charPerSecond <= 0f) charPerSecond = 20f;

            float delay = 1f / charPerSecond;
            WaitForSeconds wait = new WaitForSeconds(delay);

            // 가장 단순한 방식: 글자 하나씩 붙이기
            for (int i = 0; i < content.Length; i++)
            {
                this.DialogueTextUI.text += content[i];
                yield return wait;
            }
        }

        private bool TryParseTextContent(string directingContent, out string[] parsedContent)
        {
            parsedContent = directingContent.Split('_');

            if (parsedContent.Length != 2) return false;
            else return true;
        }
    }
}