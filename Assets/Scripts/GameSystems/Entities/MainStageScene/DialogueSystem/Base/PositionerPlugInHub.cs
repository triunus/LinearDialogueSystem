using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameSystems.PlainServices;

namespace GameSystems.Entities.MainStageScene
{
    public class PositionerPlugInHub : PlugInHub<IPositioner>
    {
        public bool TryDirectPosition(string key, string actionPosition)
        {
            if (!this.PlugIns.ContainsKey(key)) return false;
            if (!this.TryParsePosition(actionPosition, out var pos)) return false;

            this.PlugIns[key].DirectPosition(pos);
            return true;
        }
        private bool TryParsePosition(string actionPosition, out Vector3 position)
        {
            // string을 float으로 파싱.
            string onlyNumbers = new string(actionPosition.Where(c => char.IsDigit(c)).ToArray());
            int convertedPositionLayer = int.Parse(onlyNumbers);

            // 화면에서 너무 멀어지는 경우 제한.
            if (convertedPositionLayer < -2) convertedPositionLayer = -2;
            if (12 < convertedPositionLayer) convertedPositionLayer = 12;

            position = this.Get2DObjectPosition(convertedPositionLayer);
            return true;
        }

        public bool TryMove(string key, string actionPosition, string actionDuration, out IEnumerator enumerator)
        {
            enumerator = null;

            if (this.PlugIns.ContainsKey(key))
            {
                if (!this.TryParseMovePosition(actionPosition, out var positions)) return false;
                if (!this.TryParseDuration(actionDuration, out var durations)) return false;

                enumerator = this.PlugIns[key].Move(positions, durations);
                return true;
            }
            else return false;
        }
        private bool TryParseMovePosition(string actionPosition, out Vector3[] positions)
        {
            // string을 float으로 파싱.
            string onlyNumbers = new string(actionPosition.Where(c => char.IsDigit(c) || c == '_').ToArray());
            string[] positionLayers = onlyNumbers.Split('_');

            List<int> convertedPositionLayer = new();

            // 컨버트 및 변경.
            // 화면에서 너무 멀어지는 경우 제한.
            for (int i = 0; i < positionLayers.Length; ++i)
            {
                int temp = int.Parse(positionLayers[i]);

                if (temp < -2) temp = -2;
                if (12 < temp) temp = 12;

                convertedPositionLayer.Add(temp);
            }

            List<Vector3> convertedPos = new();

            for (int i = 0; i < convertedPositionLayer.Count; ++i)
            {
                convertedPos.Add(this.Get2DObjectPosition(convertedPositionLayer[i]));
            }

            positions = convertedPos.ToArray();
            return true;
        }
        private bool TryParseDuration(string actionDuration, out float[] durations)
        {
            // string을 float으로 파싱.
            string onlyNumbers = new string(actionDuration.Where(c => char.IsDigit(c) || c == '_').ToArray());
            string[] stringDurations = onlyNumbers.Split('_');

            List<float> convertedDuration = new();

            // 컨버트 및 변경.
            // 너무 낮거나 높은 경우 값 제한.
            for (int i = 0; i < stringDurations.Length; ++i)
            {
                float temp = float.Parse(stringDurations[i]);

                if (temp <= 0) temp = Time.deltaTime;
                if (10 <= temp) temp = 10f;

                convertedDuration.Add(temp);
            }

            durations = convertedDuration.ToArray();
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