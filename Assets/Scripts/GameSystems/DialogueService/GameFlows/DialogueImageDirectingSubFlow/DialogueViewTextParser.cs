namespace GameSystems.DialogueDirectingService.GameFlow
{
    public interface IDialogueViewTextParser
    {
        public bool TryParseDirectingContent(string directingContent, out string speakerName, out string textContent);
    }

    public class DialogueViewTextParser : IDialogueViewTextParser
    {
        public bool TryParseDirectingContent(string directingContent, out string speakerName, out string textContent)
        {
            speakerName = default;
            textContent = default;

            string[] parsedContent = directingContent.Split('_');

            if (parsedContent.Length != 3) return false;

            speakerName = parsedContent[1];
            textContent = parsedContent[2];
            return true;
        }
    }
}