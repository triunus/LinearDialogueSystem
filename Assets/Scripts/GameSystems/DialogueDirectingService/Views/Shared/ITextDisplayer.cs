using System.Collections;

using GameSystems.DialogueDirectingService.Datas;

namespace GameSystems.DialogueDirectingService.Views
{
    public interface ITextDisplayer
    {
        public IEnumerator TextDisplay(string speaker, string content, BehaviourToken behaviourToken);
    }
}