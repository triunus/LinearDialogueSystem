using GameSystems.DialogueDirectingService.Datas;
using GameSystems.DialogueDirectingService.Views;

namespace GameSystems.DialogueDirectingService.GameFlow
{
    public interface IDialogueViewActivator
    {
        public bool TryDirectShow(string key);
        public bool TryDirectHide(string key);
    }

    public class DialogueViewActivator : IDialogueViewActivator
    {
        private IDialogueViewObjectDataHandler DialogueViewObjectDataHandler;

        public DialogueViewActivator(IDialogueViewObjectDataHandler dialogueViewObjectDataHandler)
        {
            this.DialogueViewObjectDataHandler = dialogueViewObjectDataHandler;
        }

        // 기능.
        public bool TryDirectShow(string key)
        {
            if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<IActivation>(key, out var viewObject)) return false;

            viewObject.Show();
            return true;
        }
        public bool TryDirectHide(string key)
        {
            if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<IActivation>(key, out var viewObject)) return false;

            viewObject.Hide();
            return true;
        }
    }
}