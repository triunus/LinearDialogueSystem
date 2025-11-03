using System.Collections;

namespace GameSystems.DialogueDirectingService.Views
{
    public interface IFadeInAndOut
    {
        public IEnumerator FadeIn(float duration, Datas.BehaviourToken behaviourToken);
        public IEnumerator FadeOut(float duration, Datas.BehaviourToken behaviourToken);
    }
}