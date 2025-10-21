namespace GameSystems.DTOs
{
    public class LobbyScenePayload : PlainServices.IPayload
    {
        // ?? 뭐 필요함?
    }

    public class LoadingScenePayload : PlainServices.IPayload
    {
        public LoadingScenePayloadState LoadingScenePayloadState;

        public LoadingScenePayload(LoadingScenePayloadState emptyScenePayloadState)
        {
            LoadingScenePayloadState = emptyScenePayloadState;
        }
    }

    public enum LoadingScenePayloadState
    {

        None, ToLobbyScene, ToMainStageScene, ToCookingScene, ToCharacterScene
    }

    public class CookingScenePayload : PlainServices.IPayload
    {

    }

    public class CharacterScenePayload : PlainServices.IPayload
    {

    }

    public class MainStageScenePayload : PlainServices.IPayload
    {

    }
}