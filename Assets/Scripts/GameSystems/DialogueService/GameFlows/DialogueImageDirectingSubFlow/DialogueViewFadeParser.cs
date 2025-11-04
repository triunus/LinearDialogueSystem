using UnityEngine;

namespace GameSystems.DialogueDirectingService.GameFlow
{
    public interface IDialogueViewFadeParser
    {
        public bool TryParseDuration(string faderContent, out float duration);
    }

    public class DialogueViewFadeParser : IDialogueViewFadeParser
    {
        public bool TryParseDuration(string faderContent, out float duration)
        {
            string[] parsedData = faderContent.Split('_');

            // 오류 값일 경우, 1f 값을 할당.
            if (parsedData.Length > 1)
            {
                duration = 1f;
                return false;
            }

            // string을 float으로 파싱.
            duration = float.Parse(parsedData[0]);

            // 너무 낮거나 높은 경우 값 제한.
            if (duration <= 0) duration = Time.deltaTime;
            if (10 <= duration) duration = 10f;

            return true;
        }
    }
}