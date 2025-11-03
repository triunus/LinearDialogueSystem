using System.Collections;

using UnityEngine;

using GameSystems.DialogueDirectingService.Datas;
using GameSystems.DialogueDirectingService.Views;

namespace GameSystems.DialogueDirectingService.GameFlow
{
    public interface IDialogueViewFader
    {
        public bool TryFadeIn(string key, string faderContent, out IEnumerator enumerator, out BehaviourToken behaviourToken);
        public bool TryFadeOut(string key, string faderContent, out IEnumerator enumerator, out BehaviourToken behaviourToken);
    }

    public class DialogueViewFader : IDialogueViewFader
    {
        private IDialogueViewObjectDataHandler DialogueViewObjectDataHandler;

        public DialogueViewFader(IDialogueViewObjectDataHandler dialogueViewObjectDataHandler)
        {
            this.DialogueViewObjectDataHandler = dialogueViewObjectDataHandler;
        }

        // 기능.
        public bool TryFadeIn(string key, string faderContent, out IEnumerator enumerator, out BehaviourToken behaviourToken)
        {

            enumerator = null;
            behaviourToken = null;

            if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<IFadeInAndOut>(key, out var viewObject)) return false;
            if (!this.TryParseDuration(faderContent, out var duration)) return false;

            behaviourToken = new BehaviourToken(isRequestEnd: false);
            enumerator = viewObject.FadeIn(duration, behaviourToken);
            return true;
        }
        public bool TryFadeOut(string key, string faderContent, out IEnumerator enumerator, out BehaviourToken behaviourToken)
        {
            enumerator = null;
            behaviourToken = null;

            if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<IFadeInAndOut>(key, out var viewObject)) return false;
            if (!this.TryParseDuration(faderContent, out var duration)) return false;

            behaviourToken = new BehaviourToken(isRequestEnd: false);
            enumerator = viewObject.FadeOut(duration, behaviourToken);
            return true;
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