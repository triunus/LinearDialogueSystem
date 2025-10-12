using System.Collections;

namespace GameSystems.Entities
{
    public interface IFadeInAndOut
    {
        public IEnumerator FadeIn(float duration);
        public IEnumerator FadeOut(float duration);
    }
}