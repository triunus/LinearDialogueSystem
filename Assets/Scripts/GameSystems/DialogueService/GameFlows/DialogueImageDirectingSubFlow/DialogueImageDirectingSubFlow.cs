using System.Collections;

using UnityEngine;

using GameSystems.DialogueDirectingService.Datas;

namespace GameSystems.DialogueDirectingService.GameFlow
{
    public interface IDialogueImageDirectingSubFlow
    {
        public bool TryAction(string directingContent, out IEnumerator resultEnumerator, out BehaviourToken behaviourToken);
        public bool TrySetSpeakerAndListenerColor(string directingContent);
    }

    public class DialogueImageDirectingSubFlow : IDialogueImageDirectingSubFlow
    {
        private IDialogueViewActivator DialogueViewActivator;
        private IDialogueViewFader DialogueViewFader;
        private IDialogueViewSpriteSetter DialogueViewSpriteSetter;
        private IDialogueViewPositioner DialogueViewPositioner;

        public DialogueImageDirectingSubFlow(IDialogueViewObjectDataHandler dialogueViewObjectDataHandler)
        {
            this.DialogueViewActivator = new DialogueViewActivator(dialogueViewObjectDataHandler);
            this.DialogueViewFader = new DialogueViewFader(dialogueViewObjectDataHandler);
            this.DialogueViewSpriteSetter = new DialogueViewSpriteSetter(dialogueViewObjectDataHandler);
            this.DialogueViewPositioner = new DialogueViewPositioner(dialogueViewObjectDataHandler);
        }

        public bool TryAction(string directingContent, out IEnumerator resultEnumerator, out BehaviourToken behaviourToken)
        {
            resultEnumerator = null;
            behaviourToken = null;

            // parsedContent[0] : ActionType
            // parsedContent[1] : TargetName
            // parsedContent[2] : ActionType마다 다름.
            string[] parsedContent = directingContent.Split('_');
            ActionType behaviourType = (ActionType)System.Enum.Parse(typeof(ActionType), parsedContent[0]);

            switch (behaviourType)
            {
                case ActionType.FadeIn:
                    if (this.DialogueViewFader.TryFadeIn(parsedContent[1], parsedContent[2], out resultEnumerator, out behaviourToken)) return true;
                    else return false;
                case ActionType.FadeOut:
                    if (this.DialogueViewFader.TryFadeOut(parsedContent[1], parsedContent[2], out resultEnumerator, out behaviourToken)) return true;
                    else return false;
                case ActionType.Move:
                    if (this.DialogueViewPositioner.TryMove(parsedContent[1], parsedContent[2], out resultEnumerator, out behaviourToken)) return true;
                    else return false;
                case ActionType.DirectShow:
                    if (this.DialogueViewActivator.TryDirectShow(parsedContent[1])) return true;
                    else return false;
                case ActionType.DirectHide:
                    if (this.DialogueViewActivator.TryDirectHide(parsedContent[1])) return true;
                    else return false;
                case ActionType.SetAttitudeSprite:
                    if (this.DialogueViewSpriteSetter.TrySetAttitudeTexture2D(parsedContent[1], parsedContent[2])) return true;
                    else return false;
                case ActionType.SetFaceSprite:
                    if (this.DialogueViewSpriteSetter.TrySetFaceTexture2D(parsedContent[1], parsedContent[2])) return true;
                    else return false;
                case ActionType.SetPosition:
                    if (this.DialogueViewPositioner.TryDirectPosition(parsedContent[1], parsedContent[2])) return true;
                    else return false;
                case ActionType.None:
                default:
                    Debug.Log($"ActionType의 값이 잘못되었던가, enum으로 Parsing이 제대로 되지 않았음.");
                    break;
            }

            return false;
        }

        public bool TrySetSpeakerAndListenerColor(string directingContent)
        {
            string[] parsedContent = directingContent.Split('_');
            if (parsedContent.Length != 3) return false;

            this.DialogueViewSpriteSetter.TrySetSpeakerAndListenerColor(parsedContent[0]);

            return true;
        }
    }
}