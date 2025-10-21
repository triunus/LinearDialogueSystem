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
            // Parsing ����.
            if (!this.TryParseTextContent(directingContent, out var parsedContent)) return false;

            // �ʱ�ȭ.
            this.SpeakerTextUI.text = string.Empty;
            this.DialogueTextUI.text = string.Empty;

            Debug.Log($"speaker : {parsedContent[0]}, Content : {parsedContent[1]}");

            // ȭ�� �̸� ���.
            this.SpeakerTextUI.text = parsedContent[0];
            // ��� ��� IEnumerator �Ҵ�
            enumerator = this.OperateDialogueTextDisplay(parsedContent[1], this.charsPerSecond);

            return true;
        }
        private IEnumerator OperateDialogueTextDisplay(string content, float charPerSecond)
        {
            // ��� �ӵ� ���� �߸��Ǿ� ������, 20f���� ����.
            if (charPerSecond <= 0f) charPerSecond = 20f;

            float delay = 1f / charPerSecond;
            WaitForSeconds wait = new WaitForSeconds(delay);

            // ���� �ܼ��� ���: ���� �ϳ��� ���̱�
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