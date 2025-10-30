using System.Collections;
using System.Linq;

using UnityEngine;
using GameSystems.DTOs;

namespace GameSystems.Entities.MainStageScene
{
    public interface IDialogueViewActivator
    {
        public bool TryDirectShow(string key);
        public bool TryDirectHide(string key);
    }
    public class DialogueViewActivator : IPlugInHub<IActivation>, IDialogueViewActivator
    {
        private PlugInHub<IActivation> PlugInHub;

        public DialogueViewActivator()
        {
            this.PlugInHub = new();
        }

        // 위임된 등록 기능.
        public void RegisterPlugIn(string key, IActivation plugIn) => this.PlugInHub.RegisterPlugIn(key, plugIn);
        public void RemovePlugIn(string key) => this.PlugInHub.RemovePlugIn(key);
        public bool TryGetPlugIn(string key, out IActivation plugIn) => this.PlugInHub.TryGetPlugIn(key, out plugIn);
        public bool TryGetPlugIns(out IActivation[] plugIns) => this.PlugInHub.TryGetPlugIns(out plugIns);

        // 기능.
        public bool TryDirectShow(string key)
        {
            if (!this.PlugInHub.TryGetPlugIn(key, out var viewObject)) return false;

            viewObject.Show();
            return true;
        }
        public bool TryDirectHide(string key)
        {
            if (!this.PlugInHub.TryGetPlugIn(key, out var viewObject)) return false;

            viewObject.Hide();
            return true;
        }
    }

    public interface IDialogueViewFader
    {
        public bool TryFadeIn(string key, string faderContent, out IEnumerator enumerator, out BehaviourToken behaviourToken);
        public bool TryFadeOut(string key, string faderContent, out IEnumerator enumerator, out BehaviourToken behaviourToken);
    }
    public class DialogueViewFader : IPlugInHub<IFadeInAndOut>, IDialogueViewFader
    {
        private PlugInHub<IFadeInAndOut> PlugInHub;

        public DialogueViewFader()
        {
            this.PlugInHub = new();
        }

        // 위임된 등록 기능.
        public void RegisterPlugIn(string key, IFadeInAndOut plugIn) => this.PlugInHub.RegisterPlugIn(key, plugIn);
        public void RemovePlugIn(string key) => this.PlugInHub.RemovePlugIn(key);
        public bool TryGetPlugIn(string key, out IFadeInAndOut plugIn) => this.PlugInHub.TryGetPlugIn(key, out plugIn);
        public bool TryGetPlugIns(out IFadeInAndOut[] plugIns) => this.PlugInHub.TryGetPlugIns(out plugIns);

        // 기능.
        public bool TryFadeIn(string key, string faderContent, out IEnumerator enumerator, out BehaviourToken behaviourToken)
        {
            enumerator = null;
            behaviourToken = null;

            if (this.PlugInHub.TryGetPlugIn(key, out var viewObject))
            {
                if (!this.TryParseDuration(faderContent, out var duration)) return false;

                behaviourToken = new BehaviourToken(isRequestEnd: false);
                enumerator = viewObject.FadeIn(duration, behaviourToken);
                return true;
            }
            else return false;
        }
        public bool TryFadeOut(string key, string faderContent, out IEnumerator enumerator, out BehaviourToken behaviourToken)
        {
            enumerator = null;
            behaviourToken = null;

            if (this.PlugInHub.TryGetPlugIn(key, out var viewObject))
            {
                if (!this.TryParseDuration(faderContent, out var duration)) return false;

                behaviourToken = new BehaviourToken(isRequestEnd: false);
                enumerator = viewObject.FadeOut(duration, behaviourToken);
                return true;
            }
            else return false;
        }

        private bool TryParseDuration(string faderContent, out float duration)
        {
            string[] parsedData = faderContent.Split('_');

            // 오류 값일 경우, 1f 값을 할당.
            if (parsedData.Length > 1)
            {
                duration = 1f;
                return false;
            }

            // string을 float으로 파싱.
            duration = float.Parse(parsedData[0]);

            // 너무 낮거나 높은 경우 값 제한.
            if (duration <= 0) duration = Time.deltaTime;
            if (10 <= duration) duration = 10f;

            return true;
        }
    }

    public interface IDialogueViewSpriteSetter
    {
        public bool TrySetAttitudeTexture2D(string key, string directContent);
        public bool TrySetFaceTexture2D(string key, string directContent);
        public bool TrySetSpeakerAndListenerColor(string speakerKey);
    }
    public class DialogueViewSpriteSetter : IPlugInHub<ISpriteSetter>, IDialogueViewSpriteSetter
    {
        private PlugInHub<ISpriteSetter> PlugInHub;

        public DialogueViewSpriteSetter()
        {
            this.PlugInHub = new();
        }

        // 위임된 등록 기능.
        public void RegisterPlugIn(string key, ISpriteSetter plugIn) => this.PlugInHub.RegisterPlugIn(key, plugIn);
        public void RemovePlugIn(string key) => this.PlugInHub.RemovePlugIn(key);
        public bool TryGetPlugIn(string key, out ISpriteSetter plugIn) => this.PlugInHub.TryGetPlugIn(key, out plugIn);
        public bool TryGetPlugIns(out ISpriteSetter[] plugIns) => this.PlugInHub.TryGetPlugIns(out plugIns);

        // 기능
        public bool TrySetAttitudeTexture2D(string key, string directContent)
        {
            if (!this.PlugInHub.TryGetPlugIn(key, out var viewObject)) return false;

            AttitudeType attitudeType = (AttitudeType)System.Enum.Parse(typeof(AttitudeType), directContent);

            viewObject.SetAttitude(attitudeType);
            return true;
        }
        public bool TrySetFaceTexture2D(string key, string directContent)
        {
            if (!this.PlugInHub.TryGetPlugIn(key, out var viewObject)) return false;

            FaceType faceType = (FaceType)System.Enum.Parse(typeof(FaceType), directContent);

            viewObject.SetFace(faceType);
            return true;
        }
        public bool TrySetSpeakerAndListenerColor(string speakerKey)
        {
            if (!this.PlugInHub.TryGetPlugIns(out var viewObjects)) return false;

            if (speakerKey.Equals("Player"))
            {
                foreach (var plugIn in viewObjects)
                {
                    plugIn.SetListenerColor();
                }
            }
            else
            {
                if (!this.PlugInHub.TryGetPlugIn(speakerKey, out var viewObject)) return false;

                foreach (var plugIn in viewObjects)
                {
                    plugIn.SetListenerColor();
                }

                viewObject.SetSpeakerColor();
            }

            return true;
        }
    }

    public interface IDialogueViewPositioner
    {
        public bool TryDirectPosition(string key, string directingContent);
        public bool TryMove(string key, string directingContent, out IEnumerator enumerator, out BehaviourToken behaviourToken);
    }
    public class DialogueViewPositioner : IPlugInHub<IPositioner>, IDialogueViewPositioner
    {
        private PlugInHub<IPositioner> PlugInHub;

        public DialogueViewPositioner()
        {
            this.PlugInHub = new();
        }

        // 위임된 등록 기능.
        public void RegisterPlugIn(string key, IPositioner plugIn) => this.PlugInHub.RegisterPlugIn(key, plugIn);
        public void RemovePlugIn(string key) => this.PlugInHub.RemovePlugIn(key);
        public bool TryGetPlugIn(string key, out IPositioner plugIn) => this.PlugInHub.TryGetPlugIn(key, out plugIn);
        public bool TryGetPlugIns(out IPositioner[] plugIns) => this.PlugInHub.TryGetPlugIns(out plugIns);

        // 기능
        public bool TryDirectPosition(string key, string directingContent)
        {
            if (!this.PlugInHub.TryGetPlugIn(key, out var viewObject)) return false;
            if (!this.TryParsePosition(directingContent, out var pos)) return false;

            viewObject.DirectPosition(pos);
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

        public bool TryMove(string key, string directingContent, out IEnumerator enumerator, out BehaviourToken behaviourToken)
        {
            enumerator = null;
            behaviourToken = null;

            if (!this.PlugInHub.TryGetPlugIn(key, out var viewObject)) return false;
            if (!this.TryParseMoveValue(directingContent, out var parsedPositions, out var parsedDurations)) return false;

            behaviourToken = new DTOs.BehaviourToken(false);
            enumerator = viewObject.Move(parsedPositions, parsedDurations, behaviourToken);
            return true;
        }
        private bool TryParseMoveValue(string directingContent, out Vector3[] positions, out float[] durations)
        {
            positions = default;
            durations = default;

            // 문자열 나누기.
            // 문자열에 필요 없는 값 삭제.
            string onlyNumbers = new string(directingContent.Where(c => char.IsDigit(c) || c == '-' || c == '/' || c == '.').ToArray());
            string[] parsedContent = directingContent.Split('/');

            // 문자열 나누기.
            string[] parsedPositions = parsedContent[0].Split('-');
            // 문자열 나누기.
            string[] parsedDurations = parsedContent[1].Split('-');

            if (parsedPositions.Length < 2 || parsedDurations.Length < 1) return false;
            if (parsedPositions.Length - 1 != parsedDurations.Length) return false;

            positions = this.ParseMovePosition(parsedPositions);
            durations = this.ParseDuration(parsedDurations);

            return true;
        }
        // 마지막 값을 제외한 값이 Postion 값.
        private Vector3[] ParseMovePosition(string[] parsedPositions)
        {
            Vector3[] positions = new Vector3[parsedPositions.Length];

            for (int i = 0; i < positions.Length; ++i)
            {
                // 컨버트 및 변경.
                int tempPositionLayer = int.Parse(parsedPositions[i]);

                // 화면에서 너무 멀어지는 경우 제한.
                if (tempPositionLayer < -2) tempPositionLayer = -2;
                if (12 < tempPositionLayer) tempPositionLayer = 12;

                positions[i] = this.Get2DObjectPosition(tempPositionLayer);
            }

            return positions;
        }
        private float[] ParseDuration(string[] parsedDurations)
        {
            float[] durations = new float[parsedDurations.Length];

            for (int i = 0; i < durations.Length; ++i)
            {
                // 컨버트 및 변경.
                float tempDuration = float.Parse(parsedDurations[i]);

                // 화면에서 너무 멀어지는 경우 제한.
                if (tempDuration <= 0) tempDuration = Time.deltaTime;
                if (10 <= tempDuration) tempDuration = 10f;

                durations[i] = tempDuration; ;
            }

            return durations;
        }

        private Vector3 Get2DObjectPosition(int horizontalIndex)
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