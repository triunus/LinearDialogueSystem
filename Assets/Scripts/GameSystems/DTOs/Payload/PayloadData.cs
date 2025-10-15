namespace GameSystems.DTOs
{
    public class LobbyScenePayload : PlainServices.IPayload
    {
        // ?? 뭐 필요함?
    }

    public class LoadingScenePayload : PlainServices.IPayload
    {
        public LoadingScenePayloadState EmptyScenePayloadState;

        public LoadingScenePayload(LoadingScenePayloadState emptyScenePayloadState)
        {
            EmptyScenePayloadState = emptyScenePayloadState;
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
        public DialogueTable DialogueDataTable;

        public MainStageScenePayload(DialogueTable dialogueDataTable)
        {
            DialogueDataTable = dialogueDataTable;
        }
    }
}