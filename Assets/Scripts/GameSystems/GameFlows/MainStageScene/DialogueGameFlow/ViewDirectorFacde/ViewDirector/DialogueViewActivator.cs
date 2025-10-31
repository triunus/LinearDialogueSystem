using Foundations.PlugInHub;

using GameSystems.Entities;

namespace GameSystems.GameFlows.MainStageScene
{
    public interface IDialogueViewActivator
    {
        public bool TryDirectShow(string key);
        public bool TryDirectHide(string key);
    }

    public class DialogueViewActivator : IDialogueViewActivator
    {
        private IMultiPlugInHub DialogueViewModel;

        public DialogueViewActivator(IMultiPlugInHub multiPlugInHub)
        {
            this.DialogueViewModel = multiPlugInHub;
        }

        // 기능.
        public bool TryDirectShow(string key)
        {
            if (this.DialogueViewModel.TryGetPlugIn<IActivation>(key, out var viewObject)) return false;

            viewObject.Show();
            return true;
        }
        public bool TryDirectHide(string key)
        {
            if (this.DialogueViewModel.TryGetPlugIn<IActivation>(key, out var viewObject)) return false;

            viewObject.Hide();
            return true;
        }
    }
}