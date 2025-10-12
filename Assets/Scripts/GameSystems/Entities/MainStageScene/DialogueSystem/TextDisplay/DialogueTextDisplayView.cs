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

            // �ʱ�ȭ.
            this.SpeakerUI.text = string.Empty;
            this.dialogueTextUI.text = string.Empty;

            // ȭ�� �̸� ���.
            this.SpeakerUI.text = sayDirectionTextData.SpeakerName;

            // �ӵ� ���� �̻��ϸ�, �ٷ� ���.
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

            // ���� �ܼ��� ���: ���� �ϳ��� ���̱�
            for (int i = 0; i < content.Length; i++)
            {
                this.dialogueTextUI.text += content[i];
                yield return wait;
            }
        }
    }
}