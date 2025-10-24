using System.Collections;
using UnityEngine;
using TMPro;

using GameSystems.DTOs;

namespace GameSystems.Entities.MainStageScene
{
    public interface IDialogueTextDirectingView
    {
        public bool TryDirectTextDisplayOperation(string directingContent, out IEnumerator enumerator, out BehaviourToken behaviourToken);
        public void OperateToFinalState_TextDisplay(string directingContent);
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

        public bool TryDirectTextDisplayOperation(string directingContent, out IEnumerator enumerator, out BehaviourToken behaviourToken)
        {
            enumerator = null;
            behaviourToken = null;
            if (this.DialogueTextUI == null || this.SpeakerTextUI == null) return false;
            // Parsing 실패.
            if (!this.TryParseTextContent(directingContent, out var parsedContent)) return false;

            // 초기화.
            this.SpeakerTextUI.text = string.Empty;
            this.DialogueTextUI.text = string.Empty;

//            Debug.Log($"speaker : {parsedContent[0]}, Content : {parsedContent[1]}");

            // 화자 이름 등록.
            this.SpeakerTextUI.text = parsedContent[0];

            behaviourToken = new BehaviourToken(isRequestEnd : false);
            // 대사 출력 IEnumerator 할당
            enumerator = this.OperateDialogueTextDisplay(parsedContent[1], this.charsPerSecond, behaviourToken);

            return true;
        }
        private IEnumerator OperateDialogueTextDisplay(string content, float charPerSecond, BehaviourToken behaviourToken)
        {
            // 출력 속도 값이 잘못되어 있으면, 20f으로 변경.
            if (charPerSecond <= 0f) charPerSecond = 20f;

            float calculatedDelay = 1f / charPerSecond;
            float currentWaitDuration = 0;

            // 가장 단순한 방식: 글자 하나씩 붙이기
            for (int i = 0; i < content.Length; i++)
            {
                if (behaviourToken.IsRequestEnd) break;

                if (currentWaitDuration < calculatedDelay)
                {
                    currentWaitDuration += Time.deltaTime;
                }
                else
                {
                    this.DialogueTextUI.text += content[i];
                    currentWaitDuration = 0;
                }

                yield return Time.deltaTime;
            }

            this.DialogueTextUI.text = content;
        }
        public void OperateToFinalState_TextDisplay(string directingContent)
        {
            if (this.DialogueTextUI == null || this.SpeakerTextUI == null) return;
            Debug.Log($"OperateToFinalState_TextDisplay - 0");
            // Parsing 실패.
            if (!this.TryParseTextContent(directingContent, out var parsedContent)) return;

            Debug.Log($"OperateToFinalState_TextDisplay - 1");
            this.DialogueTextUI.text = parsedContent[1];            
        }


        private bool TryParseTextContent(string directingContent, out string[] parsedContent)
        {
            parsedContent = directingContent.Split('_');

            if (parsedContent.Length != 2) return false;
            else return true;
        }
    }
}