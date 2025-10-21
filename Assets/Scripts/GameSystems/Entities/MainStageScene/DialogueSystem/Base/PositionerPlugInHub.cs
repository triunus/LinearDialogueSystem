using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.Entities.MainStageScene
{
    public class PositionerPlugInHub : PlugInHub<IPositioner>
    {
        public bool TryDirectPosition(string key, string directingContent)
        {
            if (!this.PlugIns.ContainsKey(key)) return false;
            if (!this.TryParsePosition(directingContent, out var pos)) return false;

            this.PlugIns[key].DirectPosition(pos);
            return true;
        }
        private bool TryParsePosition(string directingContent, out Vector3 position)
        {
            string[] parsedContent = directingContent.Split('_');

            if (parsedContent.Length > 1)
            {
                Debug.Log($"잘못된 SetPositioner 요청됨.");
                position = default;
                return false;
            }

            // string을 float으로 파싱.
            string onlyNumbers = new string(parsedContent[0].Where(c => char.IsDigit(c)).ToArray());
            int convertedPositionLayer = int.Parse(onlyNumbers);

            if (convertedPositionLayer < -2f) convertedPositionLayer = -2;
            if (12f < convertedPositionLayer) convertedPositionLayer = 12;

            position = this.Get2DObjectPosition(convertedPositionLayer);
            return true;
        }

        public bool TryMove(string key, string directingContent, out IEnumerator enumerator)
        {
            enumerator = null;

            if (!this.PlugIns.ContainsKey(key)) return false;
            if (!this.TryParseMovePosition(directingContent, out var position)) return false;
            if (!this.TryParseDuration(directingContent, out var duration)) return false;

            enumerator = this.PlugIns[key].Move(position, duration);
            return true;
        }
        // 마지막 값을 제외한 값이 Postion 값.
        private bool TryParseMovePosition(string directingContent, out Vector3[] positions)
        {
            positions = default;

            // 문자열에 필요 없는 값 삭제.
            string onlyNumbers = new string(directingContent.Where(c => char.IsDigit(c) || c == '-').ToArray());
            // 문자열 나누기.
            string[] parsedContent = directingContent.Split('-');

            if (parsedContent.Length != 2) return false;

            List<Vector3> convertedPos = new();

            // 컨버트 및 변경.
            // 화면에서 너무 멀어지는 경우 제한.
            for (int i = 0; i < parsedContent.Length-1; ++i)
            {
                int tempPositionLayer = int.Parse(parsedContent[i]);

                if (tempPositionLayer < -2) tempPositionLayer = -2;
                if (12 < tempPositionLayer) tempPositionLayer = 12;

                convertedPos.Add(this.Get2DObjectPosition(tempPositionLayer));
            }

            positions = convertedPos.ToArray();
            return true;
        }
        private bool TryParseDuration(string directingContent, out float duration)
        {
            duration = default;

            // 문자열에 필요 없는 값 삭제.
            string onlyNumbers = new string(directingContent.Where(c => char.IsDigit(c) || c == '-').ToArray());
            // 문자열 나누기.
            string[] parsedContent = directingContent.Split('-');

            if (parsedContent.Length != 1) return false;

            float temp = float.Parse(parsedContent[parsedContent.Length - 1]);

            if (temp <= 0) temp = Time.deltaTime;
            if (10 <= temp) temp = 10f;

            duration = temp;

            return true;
        }

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