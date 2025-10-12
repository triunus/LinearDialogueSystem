
using GameSystems.DTOs;

namespace GameSystems.Entities.MainStageScene
{
    // Face Set
    public class DialogueActorFaceSetterPlugInHub : PlugInHub<IFaceSetter>, IEntity
    {
        public bool TrySetFace(string key, FaceType faceType)
        {
            if (!this.PlugIns.ContainsKey(key)) return false;

            this.PlugIns[key].SetFace(faceType);
            return true;
        }
    }
}