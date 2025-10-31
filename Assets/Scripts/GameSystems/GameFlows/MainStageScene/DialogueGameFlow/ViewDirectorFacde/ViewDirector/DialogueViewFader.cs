using System.Collections;

using UnityEngine;

using Foundations.PlugInHub;

using GameSystems.DTOs;

using GameSystems.Entities;

namespace GameSystems.GameFlows.MainStageScene
{
    public interface IDialogueViewFader
    {
        public bool TryFadeIn(string key, string faderContent, out IEnumerator enumerator, out BehaviourToken behaviourToken);
        public bool TryFadeOut(string key, string faderContent, out IEnumerator enumerator, out BehaviourToken behaviourToken);
    }

    public class DialogueViewFader : IDialogueViewFader
    {
        private IMultiPlugInHub DialogueViewModel;

        public DialogueViewFader(IMultiPlugInHub multiPlugInHub)
        {
            this.DialogueViewModel = multiPlugInHub;
        }

        // 기능.
        public bool TryFadeIn(string key, string faderContent, out IEnumerator enumerator, out BehaviourToken behaviourToken)
        {
            enumerator = null;
            behaviourToken = null;

            if (this.DialogueViewModel.TryGetPlugIn<IFadeInAndOut>(key, out var viewObject))
            {
                if (!this.TryParseDuration(faderContent, out var duration)) return false;

                behaviourToken = new BehaviourToken(isRequestEnd: false);
                enumerator = viewObject.FadeIn(duration, behaviourToken);
                return true;
            }
            else return false;
        }
        public bool TryFadeOut(string key, string faderContent, out IEnumerator enumerator, out BehaviourToken behaviourToken)
        {
            enumerator = null;
            behaviourToken = null;

            if (this.DialogueViewModel.TryGetPlugIn<IFadeInAndOut>(key, out var viewObject))
            {
                if (!this.TryParseDuration(faderContent, out var duration)) return false;

                behaviourToken = new BehaviourToken(isRequestEnd: false);
                enumerator = viewObject.FadeOut(duration, behaviourToken);
                return true;
            }
            else return false;
        }

        private bool TryParseDuration(string faderContent, out float duration)
        {
            string[] parsedData = faderContent.Split('_');

            // 오류 값일 경우, 1f 값을 할당.
            if (parsedData.Length > 1)
            {
                duration = 1f;
                return false;
            }

            // string을 float으로 파싱.
            duration = float.Parse(parsedData[0]);

            // 너무 낮거나 높은 경우 값 제한.
            if (duration <= 0) duration = Time.deltaTime;
            if (10 <= duration) duration = 10f;

            return true;
        }
    }
}