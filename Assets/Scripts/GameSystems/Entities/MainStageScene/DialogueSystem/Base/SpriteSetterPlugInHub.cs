
using UnityEngine;

using GameSystems.DTOs;

namespace GameSystems.Entities.MainStageScene
{
    public class SpriteSetterPlugInHub : PlugInHub<ISpriteSetter>
    {
        public bool TrySetAttitudeTexture2D(string key, string spriteContent)
        {
            if (!this.PlugIns.ContainsKey(key)) return false;
            if (!this.TryParseAttitudeContent(spriteContent, out var attitudeType)) return false;

            this.PlugIns[key].SetAttitude(attitudeType);
            return true;
        }
        private bool TryParseAttitudeContent(string attitudeContent, out AttitudeType attitudeType)
        {
            string[] parsedContent = attitudeContent.Split('_');

            if (parsedContent.Length > 1)
            {
                Debug.Log($"잘못된 Attitude 요청됨.");
                attitudeType = AttitudeType.None;
                return false;
            }
            else
            {
                attitudeType = (AttitudeType)System.Enum.Parse(typeof(AttitudeType), parsedContent[0]);
                return true;
            }
        }

        public bool TrySetFaceTexture2D(string key, string spriteContent)
        {
            if (!this.PlugIns.ContainsKey(key)) return false;
            if (!this.TryParseFaceContent(spriteContent, out var faceType)) return false;

            this.PlugIns[key].SetFace(faceType);
            return true;
        }
        private bool TryParseFaceContent(string faceContent, out FaceType faceType)
        {
            string[] parsedContent = faceContent.Split('_');

            if (parsedContent.Length > 1)
            {
                Debug.Log($"잘못된 Attitude 요청됨.");
                faceType = FaceType.None;
                return false;
            }
            else
            {
                faceType = (FaceType)System.Enum.Parse(typeof(FaceType), parsedContent[0]);
                return true;
            }
        }
    }
}