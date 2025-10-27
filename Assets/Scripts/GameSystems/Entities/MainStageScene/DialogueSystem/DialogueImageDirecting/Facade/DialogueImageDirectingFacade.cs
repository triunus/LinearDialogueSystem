using System.Collections;

using UnityEngine;

using GameSystems.DTOs;

namespace GameSystems.Entities.MainStageScene
{
    public interface IDialogueImageDirectingFacade
    {
        public bool TryAction(string directingContent, out IEnumerator resultEnumerator, out DTOs.BehaviourToken behaviourToken);
        public bool TrySetSpeakerAndListenerColor(string directingContent);
    }

    public class DialogueImageDirectingFacade : MonoBehaviour, IEntity, IDialogueImageDirectingFacade
    {
        private DialogueActivationPlugInHub ActivationPlugInHub;
        private DialogueFaderPlugInHub FaderPlugInHub;
        private DialogueSpriteSetterPlugInHub SpriteSetterPlugInHub;
        private DialoguePositionerPlugInHub PositionerPlugInHub;

        private void Awake()
        {
            var LocalEntityRepository = Repository.MainStageSceneRepository.Instance.Entity_LazyReferenceRepository;

            LocalEntityRepository.RegisterReference<DialogueImageDirectingFacade>(this);

            this.ActivationPlugInHub = LocalEntityRepository.GetOrCreate<DialogueActivationPlugInHub>();
            this.FaderPlugInHub = LocalEntityRepository.GetOrCreate<DialogueFaderPlugInHub>();
            this.SpriteSetterPlugInHub = LocalEntityRepository.GetOrCreate<DialogueSpriteSetterPlugInHub>();
            this.PositionerPlugInHub = LocalEntityRepository.GetOrCreate<DialoguePositionerPlugInHub>();
        }

        public bool TryAction(string directingContent, out IEnumerator resultEnumerator, out DTOs.BehaviourToken behaviourToken)
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
                    if (this.FaderPlugInHub.TryFadeIn(parsedContent[1], parsedContent[2], out resultEnumerator, out behaviourToken)) return true;
                    else return false;
                case ActionType.FadeOut:
                    if (this.FaderPlugInHub.TryFadeOut(parsedContent[1], parsedContent[2], out resultEnumerator, out behaviourToken)) return true;
                    else return false;
                case ActionType.Move:
                    if (this.PositionerPlugInHub.TryMove(parsedContent[1], parsedContent[2], out resultEnumerator, out behaviourToken)) return true;
                    else return false;
                case ActionType.DirectShow:
                    if (this.ActivationPlugInHub.TryDirectShow(parsedContent[1])) return true;
                    else return false;
                case ActionType.DirectHide:
                    if (this.ActivationPlugInHub.TryDirectHide(parsedContent[1])) return true;
                    else return false;
                case ActionType.SetAttitudeSprite:
                    if (this.SpriteSetterPlugInHub.TrySetAttitudeTexture2D(parsedContent[1], parsedContent[2])) return true;
                    else return false;
                case ActionType.SetFaceSprite:
                    if (this.SpriteSetterPlugInHub.TrySetFaceTexture2D(parsedContent[1], parsedContent[2])) return true;
                    else return false;
                case ActionType.SetPosition:
                    if (this.PositionerPlugInHub.TryDirectPosition(parsedContent[1], parsedContent[2])) return true;
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

            this.SpriteSetterPlugInHub.TrySetSpeakerAndListenerColor(parsedContent[0]);

            return true;
        }
    }
}