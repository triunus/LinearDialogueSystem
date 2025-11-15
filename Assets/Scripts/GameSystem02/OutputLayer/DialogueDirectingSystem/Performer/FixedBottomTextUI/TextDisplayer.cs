using System;
using System.Collections;
using UnityEngine;
using TMPro;

namespace GameSystems.OutputLayer.DialogueDirectingSystem
{
    public interface ITextDisplayer
    {
        public IEnumerator TextDisplay(string speaker, string content, Action onCompleted = null);

        public bool IsRequestToStop { get; set; }
    }

    public class TextDisplayer : MonoBehaviour, ITextDisplayer
    {
        [SerializeField] private TextMeshProUGUI SpeakerTextUI;
        [SerializeField] private TextMeshProUGUI ContentTextUI;

        private float charsPerSecond = 20;

        private bool isRequestToStop = false;
        public bool IsRequestToStop { get => isRequestToStop; set => isRequestToStop = value; }

        public IEnumerator TextDisplay(string speaker, string content, Action onCompleted = null)
        {
            if (this.ContentTextUI == null || this.SpeakerTextUI == null)
            {
                Debug.Log($"Text UIUX 관련 SerializeField 연결 오류");
            }
            else
            {
                // 초기화.
                this.SpeakerTextUI.text = string.Empty;
                this.ContentTextUI.text = string.Empty;

                // 화자 이름 등록.
                this.SpeakerTextUI.text = speaker;


                // 출력 속도 값이 잘못되어 있으면, 20f으로 변경.
                if (this.charsPerSecond <= 0f) this.charsPerSecond = 20f;

                // 1프레임 당 출력하고 싶은 문자 수.
                float calculatedDelay = 1f / this.charsPerSecond;
                float currentWaitDuration = 0;

                // 대사 출력.
                for (int i = 0; i < content.Length; i++)
                {
                    if (this.isRequestToStop) break;

                    if (currentWaitDuration < calculatedDelay)
                    {
                        currentWaitDuration += Time.deltaTime;
                    }
                    else
                    {
                        this.ContentTextUI.text += content[i];
                        currentWaitDuration = 0;
                    }

                    yield return Time.deltaTime;
                }

                this.ContentTextUI.text = content;
            }

            this.isRequestToStop = false;
            yield return Time.deltaTime;
            if (onCompleted != null) onCompleted.Invoke();
        }
    }
}