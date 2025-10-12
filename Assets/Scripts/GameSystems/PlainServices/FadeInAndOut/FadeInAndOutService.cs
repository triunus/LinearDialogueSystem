using System.Collections;

using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.PlainServices
{
    public interface IFadeInAndOutService
    {
        public void SetAlphaValue(Graphic target, float targetAlpha);
        public void SetAlphaValue(Graphic[] targetObjects, float targetAlpha);
        public void SetAlphaValue(SpriteRenderer target, float targetAlpha);
        public void SetAlphaValue(SpriteRenderer[] targetObjects, float targetAlpha);

        public IEnumerator FadeOperation(Graphic target, float startAlpha, float targetAlpha, float duration);
        public IEnumerator FadeOperation(Graphic[] targetObjects, float startAlpha, float targetAlpha, float duration);
        public IEnumerator FadeOperation(SpriteRenderer target, float startAlpha, float targetAlpha, float duration);
        public IEnumerator FadeOperation(SpriteRenderer[] targetObjects, float startAlpha, float targetAlpha, float duration);   
    }

    public class FadeInAndOutService : IPlainService, IFadeInAndOutService
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

        public IEnumerator FadeOperation(Graphic target, float startAlpha, float targetAlpha, float duration)
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
                float t = Mathf.Clamp01(elapsed / duration);
                tempCol.a = Mathf.Lerp(startAlpha, targetAlpha, t);
                target.color = tempCol;

                elapsed += Time.deltaTime;
                yield return null; // Time.deltaTime ���� null
            }

            tempCol.a = targetAlpha;
            target.color = tempCol;
        }
        public IEnumerator FadeOperation(Graphic[] targetObjects, float startAlpha, float targetAlpha, float duration)
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

            // ������ ����
            foreach (var target in targetObjects)
            {
                if (target == null) continue;

                Color c = target.color;
                c.a = targetAlpha;
                target.color = c;
            }
        }
        public IEnumerator FadeOperation(SpriteRenderer target, float startAlpha, float targetAlpha, float duration)
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
                float t = Mathf.Clamp01(elapsed / duration);
                tempCol.a = Mathf.Lerp(startAlpha, targetAlpha, t);
                target.color = tempCol;

                elapsed += Time.deltaTime;
                yield return null; // Time.deltaTime ���� null
            }

            tempCol.a = targetAlpha;
            target.color = tempCol;
        }
        public IEnumerator FadeOperation(SpriteRenderer[] targetObjects, float startAlpha, float targetAlpha, float duration)
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

            // ������ ����
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