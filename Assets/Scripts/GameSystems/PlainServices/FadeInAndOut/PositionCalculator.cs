using UnityEngine;

namespace GameSystems.PlainServices
{
    public class PositionCalculator : IPlainService
    {
        public Vector3 Get2DObjectPosition(int horizontalIndex)
        {
            Camera Camera = Camera.main;

            // 카메라 화면의 절반 크기 (월드 단위)
            float halfH = Camera.orthographicSize;
            float halfW = halfH * Camera.aspect;

            // 전체 영역을 10등분
            float stepX = (halfW * 2f) / 10f;

            // 좌하단 기준 위치 계산
            Vector3 bottomLeft = new Vector3(Camera.transform.position.x, Camera.transform.position.y, 0) - new Vector3(halfW, halfH, 0f);

            // 인덱스 위치에 배치
            Vector3 targetPos = bottomLeft + new Vector3(stepX * horizontalIndex, halfH, 0f);
            return targetPos;
        }
    }
}