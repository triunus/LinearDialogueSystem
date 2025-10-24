
using UnityEngine;

using GameSystems.DTOs;

namespace GameSystems.Entities.MainStageScene
{
    public class SpriteSetterPlugInHub : PlugInHub<ISpriteSetter>
    {
        public bool TrySetAttitudeTexture2D(string key, string spriteContent)
        {
            if (!this.PlugIns.ContainsKey(key)) return false;

            AttitudeType attitudeType = (AttitudeType)System.Enum.Parse(typeof(AttitudeType), spriteContent);

            this.PlugIns[key].SetAttitude(attitudeType);
            return true;
        }

        public bool TrySetFaceTexture2D(string key, string spriteContent)
        {
            if (!this.PlugIns.ContainsKey(key)) return false;

            FaceType faceType = (FaceType)System.Enum.Parse(typeof(FaceType), spriteContent);

            this.PlugIns[key].SetFace(faceType);
            return true;
        }
    }
}