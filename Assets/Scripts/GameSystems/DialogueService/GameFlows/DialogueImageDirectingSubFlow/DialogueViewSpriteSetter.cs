using Foundations.PlugInHub;

using GameSystems.DialogueDirectingService.Datas;


namespace GameSystems.DialogueDirectingService.GameFlow
{
    public interface IDialogueViewSpriteSetter
    {
        public bool TrySetAttitudeTexture2D(string key, string directContent);
        public bool TrySetFaceTexture2D(string key, string directContent);
        public bool TrySetSpeakerAndListenerColor(string speakerKey);
    }

    public class DialogueViewSpriteSetter : IDialogueViewSpriteSetter
    {
        private IMultiPlugInHub DialogueViewModel;

        public DialogueViewSpriteSetter(IMultiPlugInHub multiPlugInHub)
        {
            this.DialogueViewModel = multiPlugInHub;
        }

        // 기능
        public bool TrySetAttitudeTexture2D(string key, string directContent)
        {
            if (!this.DialogueViewModel.TryGetPlugIn<ISpriteSetter>(key, out var viewObject)) return false;

            AttitudeType attitudeType = (AttitudeType)System.Enum.Parse(typeof(AttitudeType), directContent);

            viewObject.SetAttitude(attitudeType);
            return true;
        }
        public bool TrySetFaceTexture2D(string key, string directContent)
        {
            if (!this.DialogueViewModel.TryGetPlugIn<ISpriteSetter>(key, out var viewObject)) return false;

            FaceType faceType = (FaceType)System.Enum.Parse(typeof(FaceType), directContent);

            viewObject.SetFace(faceType);
            return true;
        }
        public bool TrySetSpeakerAndListenerColor(string speakerKey)
        {
            if (!this.DialogueViewModel.TryGetAllPlugIn<ISpriteSetter>(out var viewObjects)) return false;

            if (speakerKey.Equals("Player"))
            {
                foreach (var plugIn in viewObjects)
                {
                    plugIn.SetListenerColor();
                }
            }
            else
            {
                if (!this.DialogueViewModel.TryGetPlugIn<ISpriteSetter>(speakerKey, out var viewObject)) return false;

                foreach (var plugIn in viewObjects)
                {
                    plugIn.SetListenerColor();
                }

                viewObject.SetSpeakerColor();
            }

            return true;
        }
    }
}