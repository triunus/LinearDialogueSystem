using System;
using System.Collections;
using UnityEngine;

namespace GameSystems.OutputLayer.DialogueDirectingSystem
{
    public interface ISetAlpha
    {
        public IEnumerator ShowAlpha(Action onCompleted);
        public IEnumerator HideAlpha(Action onCompleted);
        public IEnumerator FadeIn(float duration, Action onCompleted);
        public IEnumerator FadeOut(float duration, Action onCompleted);
        public bool IsRequestToStop { get; set; }
    }

    public class SpriteRendererAlphaSetter : MonoBehaviour, ISetAlpha
    {
        [SerializeField] private SpriteRenderer[] ActorSprites;

        [SerializeField] private float _ShowAlpha;
        [SerializeField] private float _HideAlpha;

        private bool isRequestToStop = false;
        public bool IsRequestToStop { get => isRequestToStop; set => isRequestToStop = value; }

        public IEnumerator ShowAlpha(Action onCompleted)
        {
            foreach (var sprite in this.ActorSprites)
            {
                Color temp = sprite.color;
                temp.a = _ShowAlpha;
                sprite.color = temp;
            }

            yield return Time.deltaTime;
            if (onCompleted != null) onCompleted.Invoke();
        }
        public IEnumerator HideAlpha(Action onCompleted)
        {
            foreach (var sprite in this.ActorSprites)
            {
                Color temp = sprite.color;
                temp.a = _HideAlpha;
                sprite.color = temp;
            }

            yield return Time.deltaTime;
            if (onCompleted != null) onCompleted.Invoke();
        }

        public IEnumerator FadeIn(float duration, Action onCompleted = null)
        {
            this.isRequestToStop = false;
            yield return Time.deltaTime;

            Color[] clolors = new Color[this.ActorSprites.Length];

            for (int i = 0; i < ActorSprites.Length; ++i)
            {
                if (ActorSprites[i] == null) continue;

                clolors[i] = ActorSprites[i].color;
                clolors[i].a = _HideAlpha;
                ActorSprites[i].color = clolors[i];
            }

            float elapsed = 0f;
            while (elapsed < duration)
            {
                if (this.isRequestToStop) break;

                float t = Mathf.Clamp01(elapsed / duration);

                for (int i = 0; i < this.ActorSprites.Length; ++i)
                {
                    if (this.ActorSprites[i] == null) continue;

                    clolors[i].a = Mathf.Lerp(this._HideAlpha, this._ShowAlpha, t);
                    this.ActorSprites[i].color = clolors[i];
                }

                var add = Time.deltaTime;
                elapsed += add;
                yield return add;
            }

            // 마지막 보정
            foreach (var target in this.ActorSprites)
            {
                if (target == null) continue;

                Color c = target.color;
                c.a = this._ShowAlpha;
                target.color = c;
            }

            yield return Time.deltaTime;
            this.isRequestToStop = false;

            if (onCompleted != null) onCompleted.Invoke();
        }
        public IEnumerator FadeOut(float duration, Action onCompleted = null)
        {
            this.isRequestToStop = false;
            yield return Time.deltaTime;

            Color[] clolors = new Color[this.ActorSprites.Length];

            for (int i = 0; i < ActorSprites.Length; ++i)
            {
                if (ActorSprites[i] == null) continue;

                clolors[i] = ActorSprites[i].color;
                clolors[i].a = _ShowAlpha;
                ActorSprites[i].color = clolors[i];
            }

            float elapsed = 0f;
            while (elapsed < duration)
            {
                if (this.isRequestToStop) break;

                float t = Mathf.Clamp01(elapsed / duration);

                for (int i = 0; i < this.ActorSprites.Length; ++i)
                {
                    if (this.ActorSprites[i] == null) continue;

                    clolors[i].a = Mathf.Lerp(this._ShowAlpha, this._HideAlpha, t);
                    this.ActorSprites[i].color = clolors[i];
                }

                var add = Time.deltaTime;
                elapsed += add;
                yield return add;
            }

            // 마지막 보정
            foreach (var target in this.ActorSprites)
            {
                if (target == null) continue;

                Color c = target.color;
                c.a = this._HideAlpha;
                target.color = c;
            }

            yield return Time.deltaTime;
            this.isRequestToStop = false;

            if (onCompleted != null) onCompleted.Invoke();
        }
    }
}