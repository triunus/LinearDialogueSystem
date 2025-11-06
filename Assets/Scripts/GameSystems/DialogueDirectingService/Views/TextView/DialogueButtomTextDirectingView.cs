using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using GameSystems.DialogueDirectingService.Datas;
using Foundations.PlugInHub;
using GameSystems.DialogueDirectingService.PlainServices;

namespace GameSystems.DialogueDirectingService.Views
{
    // TextDisplay
    public class DialogueButtomTextDirectingView : MonoBehaviour, IDialogueViewBinding, IActivation, IFadeInAndOut, ITextDisplayer
    {
        private IFadeInAndOutService FadeInAndOutService;

        [SerializeField] private GameObject PrefabRootGameObject;
        [SerializeField] private List<Graphic> TextDirectingObjects;

        [SerializeField] private TextMeshProUGUI SpeakerTextUI;
        [SerializeField] private TextMeshProUGUI DialogueTextUI;

        [SerializeField] private float charsPerSecond = 20;

        [SerializeField] private float ShowedAlpha;
        [SerializeField] private float HidedAlpha;

        public void InitialBinding(string key, IMultiPlugInHub multiPlugInHub, IFadeInAndOutService fadeInAndOutService = null)
        {
            multiPlugInHub.RegisterPlugIn<IActivation>(key, this);
            multiPlugInHub.RegisterPlugIn<IFadeInAndOut>(key, this);
            multiPlugInHub.RegisterPlugIn<ITextDisplayer>(key, this);

            this.FadeInAndOutService = fadeInAndOutService;
        }

        public IEnumerator FadeIn(float duration, BehaviourToken behaviourToken)
        {
            // 일단 FadeOut 값으로 만들기.
            this.FadeInAndOutService.SetAlphaValue(this.TextDirectingObjects.ToArray(), this.HidedAlpha);
            // 활성화가 안되어 있으면 활성화.
            this.Show();

            yield return Time.deltaTime;

            yield return this.FadeInAndOutService.FadeOperation(this.TextDirectingObjects.ToArray(), this.HidedAlpha, this.ShowedAlpha, duration, behaviourToken);

        }

        public IEnumerator FadeOut(float duration, BehaviourToken behaviourToken)
        {
            yield return this.FadeInAndOutService.FadeOperation(this.TextDirectingObjects.ToArray(), this.ShowedAlpha, this.HidedAlpha, duration, behaviourToken);

            this.Hide();
        }

        public void Hide()
        {
            this.PrefabRootGameObject.SetActive(false);
        }

        public void Show()
        {
            this.PrefabRootGameObject.SetActive(true);
        }

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