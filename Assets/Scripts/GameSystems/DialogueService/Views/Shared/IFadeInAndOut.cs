using System.Collections;

using GameSystems.DialogueDirectingService.Datas;

namespace GameSystems.DialogueDirectingService.Views
{
    public interface IFadeInAndOut
    {
        public IEnumerator FadeIn(float duration, BehaviourToken behaviourToken);
        public IEnumerator FadeOut(float duration, BehaviourToken behaviourToken);
    }
}