namespace GameSystems.DialogueDirectingService.Views
{
    public interface ISpriteSetter
    {
        public void SetAttitude(Datas.AttitudeType attitudeType);
        public void SetFace(Datas.FaceType faceType);

        public void SetSpeakerColor();
        public void SetListenerColor();
    }
}