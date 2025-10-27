
using UnityEngine;

using GameSystems.DTOs;

namespace GameSystems.Entities.MainStageScene
{
    public class SpriteSetterPlugInHub : PlugInHub<ISpriteSetter>
    {
        public bool TrySetAttitudeTexture2D(string key, string directContent)
        {
            if (!this.PlugIns.ContainsKey(key)) return false;

            AttitudeType attitudeType = (AttitudeType)System.Enum.Parse(typeof(AttitudeType), directContent);

            this.PlugIns[key].SetAttitude(attitudeType);
            return true;
        }

        public bool TrySetFaceTexture2D(string key, string directContent)
        {
            if (!this.PlugIns.ContainsKey(key)) return false;

            FaceType faceType = (FaceType)System.Enum.Parse(typeof(FaceType), directContent);

            this.PlugIns[key].SetFace(faceType);
            return true;
        }

        public bool TrySetSpeakerAndListenerColor(string speakerKey)
        {
            if (speakerKey.Equals("Player"))
            {
                foreach (var plugIn in this.PlugIns.Values)
                {
                    plugIn.SetListenerColor();
                }
            }
            else
            {
                if (!this.PlugIns.ContainsKey(speakerKey)) return false;

                foreach (var plugIn in this.PlugIns.Values)
                {
                    plugIn.SetListenerColor();
                }

                this.PlugIns[speakerKey].SetSpeakerColor();
            }

            return true;
        }
    }
}