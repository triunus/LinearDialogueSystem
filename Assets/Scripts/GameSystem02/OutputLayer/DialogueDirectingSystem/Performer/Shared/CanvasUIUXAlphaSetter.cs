using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.OutputLayer.DialogueDirectingSystem
{
    public class CanvasUIUXAlphaSetter : MonoBehaviour, ISetAlpha
    {
        [SerializeField] private Graphic[] CanvasUIUXs;

        [SerializeField] private float _ShowAlpha;
        [SerializeField] private float _HideAlpha;

        private bool isRequestToStop = false;
        public bool IsRequestToStop { get => isRequestToStop; set => isRequestToStop = value; }

        public IEnumerator ShowAlpha(Action onCompleted)
        {
            foreach (var sprite in this.CanvasUIUXs)
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
            foreach (var sprite in this.CanvasUIUXs)
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

            Color[] clolors = new Color[this.CanvasUIUXs.Length];

            for (int i = 0; i < CanvasUIUXs.Length; ++i)
            {
                if (CanvasUIUXs[i] == null) continue;

                clolors[i] = CanvasUIUXs[i].color;
                clolors[i].a = _HideAlpha;
                CanvasUIUXs[i].color = clolors[i];
            }

            float elapsed = 0f;
            while (elapsed < duration)
            {
                if (this.isRequestToStop) break;

                float t = Mathf.Clamp01(elapsed / duration);

                for (int i = 0; i < this.CanvasUIUXs.Length; ++i)
                {
                    if (this.CanvasUIUXs[i] == null) continue;

                    clolors[i].a = Mathf.Lerp(this._HideAlpha, this._ShowAlpha, t);
                    this.CanvasUIUXs[i].color = clolors[i];
                }

                var add = Time.deltaTime;
                elapsed += add;
                yield return add;
            }

            // 마지막 보정
            foreach (var target in this.CanvasUIUXs)
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

            Color[] clolors = new Color[this.CanvasUIUXs.Length];

            for (int i = 0; i < CanvasUIUXs.Length; ++i)
            {
                if (CanvasUIUXs[i] == null) continue;

                clolors[i] = CanvasUIUXs[i].color;
                clolors[i].a = _ShowAlpha;
                CanvasUIUXs[i].color = clolors[i];
            }

            float elapsed = 0f;
            while (elapsed < duration)
            {
                if (this.isRequestToStop) break;

                float t = Mathf.Clamp01(elapsed / duration);

                for (int i = 0; i < this.CanvasUIUXs.Length; ++i)
                {
                    if (this.CanvasUIUXs[i] == null) continue;

                    clolors[i].a = Mathf.Lerp(this._ShowAlpha, this._HideAlpha, t);
                    this.CanvasUIUXs[i].color = clolors[i];
                }

                var add = Time.deltaTime;
                elapsed += add;
                yield return add;
            }

            // 마지막 보정
            foreach (var target in this.CanvasUIUXs)
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