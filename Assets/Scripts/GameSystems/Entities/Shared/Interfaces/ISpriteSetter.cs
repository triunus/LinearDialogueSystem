namespace GameSystems.Entities
{
    public interface ISpriteSetter
    {
        public void SetAttitude(DTOs.AttitudeType attitudeType);
        public void SetFace(DTOs.FaceType faceType);
    }
}