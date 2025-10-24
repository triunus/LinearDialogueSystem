using System.Collections;

namespace GameSystems.Entities
{
    public interface IFadeInAndOut
    {
        public IEnumerator FadeIn(float duration, DTOs.BehaviourToken behaviourToken);
        public IEnumerator FadeOut(float duration, DTOs.BehaviourToken behaviourToken);
    }
}