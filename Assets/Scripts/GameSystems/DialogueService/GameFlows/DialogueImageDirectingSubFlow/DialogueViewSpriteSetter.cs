using GameSystems.DialogueDirectingService.Datas;
using GameSystems.DialogueDirectingService.Views;

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
        private IDialogueViewObjectDataHandler DialogueViewObjectDataHandler;

        public DialogueViewSpriteSetter(IDialogueViewObjectDataHandler dialogueViewObjectDataHandler)
        {
            this.DialogueViewObjectDataHandler = dialogueViewObjectDataHandler;
        }

        // 기능
        public bool TrySetAttitudeTexture2D(string key, string directContent)
        {
            if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<ISpriteSetter>(key, out var viewObject)) return false;

            AttitudeType attitudeType = (AttitudeType)System.Enum.Parse(typeof(AttitudeType), directContent);

            viewObject.SetAttitude(attitudeType);
            return true;
        }
        public bool TrySetFaceTexture2D(string key, string directContent)
        {
            if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<ISpriteSetter>(key, out var viewObject)) return false;

            FaceType faceType = (FaceType)System.Enum.Parse(typeof(FaceType), directContent);

            viewObject.SetFace(faceType);
            return true;
        }
        public bool TrySetSpeakerAndListenerColor(string speakerKey)
        {
            if (!this.DialogueViewObjectDataHandler.TryGetAllPlugIn<ISpriteSetter>(out var viewObjects)) return false;

            if (speakerKey.Equals("Player"))
            {
                foreach (var plugIn in viewObjects)
                {
                    plugIn.SetListenerColor();
                }
            }
            else
            {
                if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<ISpriteSetter>(speakerKey, out var viewObject)) return false;

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