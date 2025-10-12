using System.Collections;
using UnityEngine;

namespace GameSystems.PlainServices
{
    public interface IRectTransformMovementService
    {
        public IEnumerator MoveEaseLinear(RectTransformAnimationData rectTransformAnimationData);
    }

    public class RectTransformMovementService : IPlainService, IRectTransformMovementService
    {
        public RectTransformMovementService() { }

        public IEnumerator MoveEaseLinear(RectTransformAnimationData rectTransformAnimationData)
        {
            float elapsed = 0f;

            while (elapsed < rectTransformAnimationData.AnimationDuration)
            {
                elapsed += Time.deltaTime;

                // 0 → 1 까지 등속 진행률
                float t = Mathf.Clamp01(elapsed / rectTransformAnimationData.AnimationDuration);

                // 등속 이동 (Lerp를 진행률 그대로 사용)
                rectTransformAnimationData.Target.anchoredPosition = Vector3.Lerp(rectTransformAnimationData.From, rectTransformAnimationData.To, t);

                yield return null;
            }

            // 마지막에 정확히 목표 지점으로 스냅
            rectTransformAnimationData.Target.anchoredPosition = rectTransformAnimationData.To;
        }
    }

    [System.Serializable]
    public struct RectTransformAnimationData
    {
        public RectTransformAnimationData(RectTransform Target, Vector3 From, Vector3 To, float animationDuration)
        {
            this.Target = Target;
            this.From = From;
            this.To = To;
            this.AnimationDuration = animationDuration;
        }

        public RectTransform Target { get; }
        public Vector3 From { get; }
        public Vector3 To { get; }
        public float AnimationDuration { get; }
    }
}