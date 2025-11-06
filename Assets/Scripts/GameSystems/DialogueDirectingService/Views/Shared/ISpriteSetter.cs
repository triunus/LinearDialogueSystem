using GameSystems.DialogueDirectingService.Datas;

namespace GameSystems.DialogueDirectingService.Views
{
    public interface ISpriteSetter
    {
        public void SetAttitude(AttitudeType attitudeType);
        public void SetFace(FaceType faceType);

        public void SetSpeakerColor();
        public void SetListenerColor();
    }
}