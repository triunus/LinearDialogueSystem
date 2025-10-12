namespace GameSystems.DTOs
{
    public class LobbyScenePayload : PlainServices.IPayload
    {
        // ?? 뭐 필요함?
    }

    public class EmptyScenePayload : PlainServices.IPayload
    {
        public EmptyScenePayloadState EmptyScenePayloadState;

        public EmptyScenePayload(EmptyScenePayloadState emptyScenePayloadState)
        {
            EmptyScenePayloadState = emptyScenePayloadState;
        }
    }

    public enum EmptyScenePayloadState
    {

        None, ToLobbyScene, ToMainStageScene
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