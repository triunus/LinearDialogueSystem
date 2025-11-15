using System;
using System.Collections;
using UnityEngine;

namespace GameSystems.OutputLayer.DialogueDirectingSystem
{
    public interface ITransformPositioner
    {
        public IEnumerator SetPosition(Vector3 position, Action onCompleted = null);
        public IEnumerator OperateMoveWithFilpX(Vector3[] positions, float[] durations, Action onCompleted = null);
        public IEnumerator OperateMove(Vector3[] positions, float[] durations, Action onCompleted = null);
        public bool IsRequestToStop { get; set; }
    }

    public class TransformPositioner : MonoBehaviour, ITransformPositioner
    {
        [SerializeField] private Transform ActorRootTransform;
        [SerializeField] private SpriteRenderer ActorSpriteRenderer;

        // 기본적으로 바라보는 방향 ( True : 좌 -> 우, False : 우 -> 좌 )
        [SerializeField] private bool defaultFacingRight;

        private bool isRequestToStop = false;
        public bool IsRequestToStop { get => isRequestToStop; set => isRequestToStop = value; }

        public IEnumerator SetPosition(Vector3 position, Action onCompleted = null)
        {
            this.ActorRootTransform.position = position;
            this.FlipX();

            yield return Time.deltaTime;
            this.isRequestToStop = false;
            if (onCompleted != null) onCompleted.Invoke();
        }

        public IEnumerator OperateMoveWithFilpX(Vector3[] positions, float[] durations, Action onCompleted = null)
        {
            for (int i = 0; i < durations.Length; ++i)
            {
                if (this.isRequestToStop) break;
                // 초기값.
                Vector3 start = positions[i];
                Vector3 end = positions[i + 1];
                float elapsed = 0f;

                // 움직이는 방향을 바라보도록 FilpX 
                float directionX = end.x - start.x;
                if (directionX >= 0)
                    this.ActorSpriteRenderer.flipX = !this.defaultFacingRight;
                else
                    this.ActorSpriteRenderer.flipX = this.defaultFacingRight;

                while (elapsed < durations[i])
                {
                    if (this.isRequestToStop) break;

                    float t = elapsed / durations[i];
                    this.ActorRootTransform.position = Vector3.Lerp(start, end, t);

                    elapsed += Time.deltaTime;
                    yield return Time.deltaTime;
                }

                // 마지막 위치 지정
                this.ActorRootTransform.position = end;
            }

            this.ActorRootTransform.position = positions[positions.Length - 1]; ;
            this.FlipX();

            yield return Time.deltaTime;
            this.isRequestToStop = false;
            if (onCompleted != null) onCompleted.Invoke();
        }

        private void FlipX()
        {
            float cameraCenterX = Camera.main.transform.position.x;
            float currentX = this.ActorRootTransform.position.x;

            // 오른쪽에 있으면 왼쪽을 보게
            if (currentX > cameraCenterX)
            {
                this.ActorSpriteRenderer.flipX = defaultFacingRight;
            }
            // 왼쪽에 있으면 오른쪽을 보게
            else
            {
                this.ActorSpriteRenderer.flipX = !defaultFacingRight;
            }
        }

        public IEnumerator OperateMove(Vector3[] positions, float[] durations, Action onCompleted = null)
        {
            for (int i = 0; i < durations.Length; ++i)
            {
                if (this.isRequestToStop) break;
                // 초기값.
                Vector3 start = positions[i];
                Vector3 end = positions[i + 1];
                float elapsed = 0f;

                while (elapsed < durations[i])
                {
                    if (this.isRequestToStop) break;

                    float t = elapsed / durations[i];
                    this.ActorRootTransform.position = Vector3.Lerp(start, end, t);

                    elapsed += Time.deltaTime;
                    yield return Time.deltaTime;
                }

                // 마지막 위치 지정
                this.ActorRootTransform.position = end;
            }

            this.ActorRootTransform.position = positions[positions.Length - 1]; ;

            yield return Time.deltaTime;
            this.isRequestToStop = false;
            if (onCompleted != null) onCompleted.Invoke();
        }
    }
}