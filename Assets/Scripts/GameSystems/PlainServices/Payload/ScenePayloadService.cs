namespace GameSystems.PlainServices
{
    public interface IPayload { }
    public interface IScenePayloadService
    {
        public bool TryGetPayload<T>(out T resultPayload) where T : class, IPayload;
        public void SetPayload<T>(T data) where T : class, IPayload;
        public void ClearPayload();
    }

    public class ScenePayloadService : IPlainService, IScenePayloadService
    {
        private IPayload SceneConvertPayload;

        public ScenePayloadService()
        {
            this.SceneConvertPayload = null;
        }

        public bool TryGetPayload<T>(out T resultPayload) where T : class, IPayload
        {
            resultPayload = null;
            if (this.SceneConvertPayload == null) return false;

            resultPayload = (T)this.SceneConvertPayload;
            return true;
        }

        public void SetPayload<T>(T data) where T : class, IPayload
        {
            this.SceneConvertPayload = data;
        }

        public void ClearPayload()
        {
            this.SceneConvertPayload = null;
        }
    }
}
