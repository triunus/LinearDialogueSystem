/*using System.Collections;

using UnityEngine;

using GameSystems.DialogueDirectingService.Datas;
using GameSystems.DialogueDirectingService.Views;

namespace GameSystems.DialogueDirectingService.GameFlow
{
    public interface IDialogueDirectingViewFacade
    {
        public bool TryAction(string directingContent, out IEnumerator resultEnumerator, out BehaviourToken behaviourToken);
        public bool TrySetSpeakerAndListenerColor(string directingContent);
    }

    public class DialogueDirectingViewFacade : IDialogueDirectingViewFacade
    {
        private IDialogueViewObjectDataHandler DialogueViewObjectDataHandler;

        private IDialogueViewFadeParser DialogueViewFadeParser;
        private IDialogueViewPositioner DialogueViewPositioner;
        private IDialogueViewTextParser DialogueViewTextParser;
        private IDialogueViewChoiceButtonParser DialogueViewChoiceButtonParser;

        public DialogueDirectingViewFacade(IDialogueViewObjectDataHandler dialogueViewObjectDataHandler)
        {
            this.DialogueViewObjectDataHandler = dialogueViewObjectDataHandler;

            this.DialogueViewFadeParser = new DialogueViewFadeParser();
            this.DialogueViewPositioner = new DialogueViewPositionParser();
            this.DialogueViewTextParser = new DialogueViewTextParser();
            this.DialogueViewChoiceButtonParser = new DialogueViewChoiceButtonParser();
        }

        public bool TryAction(string directingContent, out IEnumerator resultEnumerator, out BehaviourToken behaviourToken)
        {
            resultEnumerator = null;
            behaviourToken = null;

            // parsedContent[0] : ActionType
            // parsedContent[1] : TargetName
            // parsedContent[2] : ActionType마다 다름.
            string[] parsedContent = directingContent.Split('_');
            DirectingActionType behaviourType = (DirectingActionType)System.Enum.Parse(typeof(DirectingActionType), parsedContent[0]);

            switch (behaviourType)
            {
                case DirectingActionType.FadeIn:
                    if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<IFadeInAndOut>(parsedContent[1], out var fadeInView)) return false;
                    if (!this.DialogueViewFadeParser.TryParseDuration(parsedContent[2], out var fadeInDuration)) return false;

                    behaviourToken = new BehaviourToken(isRequestEnd: false);
                    resultEnumerator = fadeInView.FadeIn(fadeInDuration, behaviourToken);
                    return true;
                case DirectingActionType.FadeOut:
                    if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<IFadeInAndOut>(parsedContent[1], out var fadeOutView)) return false;
                    if (!this.DialogueViewFadeParser.TryParseDuration(parsedContent[2], out var fadeOutDuration)) return false;

                    behaviourToken = new BehaviourToken(isRequestEnd: false);
                    resultEnumerator = fadeOutView.FadeOut(fadeOutDuration, behaviourToken);
                    return true;
                case DirectingActionType.DirectShow:
                    if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<IActivation>(parsedContent[1], out var showView)) return false;

                    showView.Show();
                    return true;
                case DirectingActionType.DirectHide:
                    if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<IActivation>(parsedContent[1], out var hideView)) return false;

                    hideView.Hide();
                    return true;
                case DirectingActionType.SetAttitudeSprite:
                    if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<ISpriteSetter>(parsedContent[1], out var setAttitudeView)) return false;

                    AttitudeType attitudeType = (AttitudeType)System.Enum.Parse(typeof(AttitudeType), parsedContent[2]);
                    setAttitudeView.SetAttitude(attitudeType);
                    return true;
                case DirectingActionType.SetFaceSprite:
                    if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<ISpriteSetter>(parsedContent[1], out var setFaceView)) return false;

                    FaceType faceType = (FaceType)System.Enum.Parse(typeof(FaceType), parsedContent[2]);
                    setFaceView.SetFace(faceType);
                    return true;
                case DirectingActionType.SetPosition:
                    if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<IPositioner>(parsedContent[1], out var setPositionView)) return false;
                    if (!this.DialogueViewPositioner.TryParsePosition(parsedContent[2], out var pos)) return false;

                    setPositionView.DirectPosition(pos);
                    return true;
                case DirectingActionType.Move:
                    if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<IPositioner>(parsedContent[1], out var setMoveView)) return false;
                    if (!this.DialogueViewPositioner.TryParseMoveValue(parsedContent[2], out var parsedPositions, out var parsedDurations)) return false;

                    behaviourToken = new BehaviourToken(isRequestEnd : false);
                    resultEnumerator = setMoveView.Move(parsedPositions, parsedDurations, behaviourToken);
                    return true;
                case DirectingActionType.SetTextDisplay:
                    if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<ITextDisplayer>(parsedContent[1], out var setTextDisplayView)) return false;
                    if (!this.DialogueViewTextParser.TryParseDirectingContent(parsedContent[2], out var spaker, out var content)) return false;



                    behaviourToken = new BehaviourToken(isRequestEnd: false);
                    resultEnumerator = setTextDisplayView.TextDisplay(spaker, content, behaviourToken);
                    return true;
                case DirectingActionType.SetButton:
                    if (!this.DialogueViewObjectDataHandler.TryGetPlugIn<IChoiceButtonSetter>(parsedContent[1], out var setButtonView)) return false;
                    if (!this.DialogueViewChoiceButtonParser.TryParseChoiceButtonContent(directingContent, out var buttonKey, out var needConditions, out var buttonContent, out int nextBranchPoint)) return false;

                    // 버튼 설정.
                    behaviourToken = new BehaviourToken(isRequestEnd: false);
                    resultEnumerator = setButtonView.OperateChoiceButtonDisplay(buttonContent, nextBranchPoint, behaviourToken);
                    // 조건이 들어간 버튼인 경우, 조건 설정.
                    if(needConditions != null)
                        setButtonView.SetCondition(needConditions);

                    return true;
                case DirectingActionType.None:
                    break;
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

            string speakerKey = parsedContent[0];

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

        public bool TryUpdateChoiceButtonView(string selectedButtonKey)
        {
            if (!this.DialogueViewObjectDataHandler.TryGetAllPlugIn<IChoiceButtonSetter>(out var viewObjects)) return false;

            foreach (var buttonView in viewObjects)
            {
                buttonView.UpdateChoiceButtonView(selectedButtonKey);
            }

            return true;
        }
    }
}*/