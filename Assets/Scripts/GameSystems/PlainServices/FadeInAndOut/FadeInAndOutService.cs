using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using GameSystems.DialogueDirectingService.Datas;

namespace GameSystems.DialogueDirectingService.PlainServices
{
    public interface IFadeInAndOutService
    {
        public void SetAlphaValue(Graphic target, float targetAlpha);
        public void SetAlphaValue(Graphic[] targetObjects, float targetAlpha);
        public void SetAlphaValue(SpriteRenderer target, float targetAlpha);
        public void SetAlphaValue(SpriteRenderer[] targetObjects, float targetAlpha);

        public IEnumerator FadeOperation(Graphic target, float startAlpha, float targetAlpha, float duration, BehaviourToken behaviourToken = null);
        public IEnumerator FadeOperation(Graphic[] targetObjects, float startAlpha, float targetAlpha, float duration, BehaviourToken behaviourToken = null);
        public IEnumerator FadeOperation(SpriteRenderer target, float startAlpha, float targetAlpha, float duration, BehaviourToken behaviourToken = null);
        public IEnumerator FadeOperation(SpriteRenderer[] targetObjects, float startAlpha, float targetAlpha, float duration, BehaviourToken behaviourToken = null);   
    }

    public class FadeInAndOutService : IFadeInAndOutService
    {
        public void SetAlphaValue(Graphic target, float targetAlpha)
        {
            Color col = target.color;
            col.a = targetAlpha;
            target.color = col;
        }
        public void SetAlphaValue(Graphic[] targetObjects, float targetAlpha)
        {
            foreach (var target in targetObjects)
            {
                Color col = target.color;
                col.a = targetAlpha;
                target.color = col;
            }
        }
        public void SetAlphaValue(SpriteRenderer target, float targetAlpha)
        {
            Color col = target.color;
            col.a = targetAlpha;
            target.color = col;
        }
        public void SetAlphaValue(SpriteRenderer[] targetObjects, float targetAlpha)
        {
            foreach (var target in targetObjects)
            {
                Color col = target.color;
                col.a = targetAlpha;
                target.color = col;
            }
        }

        public IEnumerator FadeOperation(Graphic target, float startAlpha, float targetAlpha, float duration, BehaviourToken behaviourToken = null)
        {
            Color tempCol = target.color;
            tempCol.a = startAlpha;
            target.color = tempCol;

            yield return null;

            if (duration == 0)
                duration = Time.deltaTime;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                if (behaviourToken != null && behaviourToken.IsRequestEnd) break;

                float t = Mathf.Clamp01(elapsed / duration);
                tempCol.a = Mathf.Lerp(startAlpha, targetAlpha, t);
                target.color = tempCol;

                elapsed += Time.deltaTime;
                yield return null; // Time.deltaTime 말고 null
            }

            tempCol.a = targetAlpha;
            target.color = tempCol;
        }
        public IEnumerator FadeOperation(Graphic[] targetObjects, float startAlpha, float targetAlpha, float duration, BehaviourToken behaviourToken = null)
        {
            Color[] clolors = new Color[targetObjects.Length];

            for (int i = 0; i < targetObjects.Length; ++i)
            {
                if (targetObjects[i] == null) continue;

                clolors[i] = targetObjects[i].color;
                clolors[i].a = startAlpha;
                targetObjects[i].color = clolors[i];
            }

            yield return null;

            if (duration == 0)
                duration = Time.deltaTime;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                if (behaviourToken != null && behaviourToken.IsRequestEnd) break;

                float t = Mathf.Clamp01(elapsed / duration);

                for (int i = 0; i < targetObjects.Length; ++i)
                {
                    if (targetObjects[i] == null) continue;

                    clolors[i].a = Mathf.Lerp(startAlpha, targetAlpha, t);
                    targetObjects[i].color = clolors[i];
                }

                elapsed += Time.deltaTime;
                yield return Time.deltaTime;
            }

            // 마지막 보정
            foreach (var target in targetObjects)
            {
                if (target == null) continue;

                Color c = target.color;
                c.a = targetAlpha;
                target.color = c;
            }
        }
        public IEnumerator FadeOperation(SpriteRenderer target, float startAlpha, float targetAlpha, float duration, BehaviourToken behaviourToken = null)
        {
            Color tempCol = target.color;
            tempCol.a = startAlpha;

            target.color = tempCol;
            yield return null;

            if (duration == 0)
                duration = Time.deltaTime;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                if (behaviourToken != null && behaviourToken.IsRequestEnd) break;

                float t = Mathf.Clamp01(elapsed / duration);
                tempCol.a = Mathf.Lerp(startAlpha, targetAlpha, t);
                target.color = tempCol;

                elapsed += Time.deltaTime;
                yield return null; // Time.deltaTime 말고 null
            }

            tempCol.a = targetAlpha;
            target.color = tempCol;
        }
        public IEnumerator FadeOperation(SpriteRenderer[] targetObjects, float startAlpha, float targetAlpha, float duration, BehaviourToken behaviourToken = null)
        {
            Color[] clolors = new Color[targetObjects.Length];

            for (int i = 0; i < targetObjects.Length; ++i)
            {
                if (targetObjects[i] == null) continue;

                clolors[i] = targetObjects[i].color;
                clolors[i].a = startAlpha;
                targetObjects[i].color = clolors[i];
            }

            yield return null;

            if (duration == 0)
                duration = Time.deltaTime;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                if (behaviourToken != null && behaviourToken.IsRequestEnd) break;

                float t = Mathf.Clamp01(elapsed / duration);

                for (int i = 0; i < targetObjects.Length; ++i)
                {
                    if (targetObjects[i] == null) continue;

                    clolors[i].a = Mathf.Lerp(startAlpha, targetAlpha, t);
                    targetObjects[i].color = clolors[i];
                }

                elapsed += Time.deltaTime;
                yield return Time.deltaTime;
            }

            // 마지막 보정
            foreach (var target in targetObjects)
            {
                if (target == null) continue;

                Color c = target.color;
                c.a = targetAlpha;
                target.color = c;
            }
        }
    }
}