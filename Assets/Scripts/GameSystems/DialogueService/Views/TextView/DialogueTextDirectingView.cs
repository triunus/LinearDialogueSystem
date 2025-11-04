using System.Collections;
using UnityEngine;
using TMPro;

using GameSystems.DialogueDirectingService.Datas;

namespace GameSystems.DialogueDirectingService.Views
{
    // TextDisplay
    public class DialogueTextDirectingView : MonoBehaviour, ITextDisplayer
    {
        [SerializeField] private TextMeshProUGUI SpeakerTextUI;
        [SerializeField] private TextMeshProUGUI DialogueTextUI;

        [SerializeField] private float charsPerSecond = 20;

        public IEnumerator TextDisplay(string speaker, string content, BehaviourToken behaviourToken)
        {
            if (this.DialogueTextUI == null || this.SpeakerTextUI == null)
            {
                Debug.Log($"Text UIUX 관련 SerializeField 연결 오류");
                return default;
            }

            // 초기화.
            this.SpeakerTextUI.text = string.Empty;
            this.DialogueTextUI.text = string.Empty;

            // 화자 이름 등록.
            this.SpeakerTextUI.text = speaker;

            // 대사 출력.
            return this.OperateDialogueTextDisplay(content, this.charsPerSecond, behaviourToken);
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
    }
}