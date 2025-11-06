namespace GameSystems.DialogueDirectingService.GameFlow
{
    public interface IDialogueViewChoiceButtonParser
    {
        public bool TryParseChoiceButtonContent(string directingContent, out string buttonKey, out string[] conditionButtonKeys, out string content, out int nextBranchPoint);
    }

    public class DialogueViewChoiceButtonParser : IDialogueViewChoiceButtonParser
    {
        public bool TryParseChoiceButtonContent(string directingContent, out string buttonKey, out string[] conditionButtonKeys, out string content, out int nextBranchPoint)
        {
            buttonKey = default;
            conditionButtonKeys = default;
            content = default;
            nextBranchPoint = default;

            string[] parsedContent = directingContent.Split('_');

            if (parsedContent.Length != 5) return false;

            buttonKey = parsedContent[1];
            if (!conditionButtonKeys.Equals("None"))
            {
                conditionButtonKeys = parsedContent[2].Split('-');

            }

            content = parsedContent[3];
            nextBranchPoint = int.Parse(parsedContent[4]);
            return true;
        }
    }
}