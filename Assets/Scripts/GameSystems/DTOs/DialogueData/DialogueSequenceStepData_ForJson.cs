using System.Linq;
using System.Collections.Generic;

namespace GameSystems.DTOs
{
    [System.Serializable]
    public class DialogueDirectingDataList_ForJson
    {
        public List<DialogueDirectingData_ForJson> DialogueDirectingDatas;
    }
    [System.Serializable]
    public class DialogueDirectingData_ForJson
    {
        public int Index;

        public string DirectingType;
        public string DirectingContent;
        public string IsChainWithNext;
        public bool IsSkipable;
        public bool IsAutoAble;
        public string NextDirectiveCommand;
    }


    [System.Serializable]
    public class SpriteAttitudeTexture2D
    {
        public AttitudeType AttitudeType;
        public UnityEngine.Texture2D Texture2D;
    }


    [System.Serializable]
    public class SpriteFaceTexture2D
    {
        public FaceType FaceType;
        public UnityEngine.Texture2D Texture2D;
    }
}