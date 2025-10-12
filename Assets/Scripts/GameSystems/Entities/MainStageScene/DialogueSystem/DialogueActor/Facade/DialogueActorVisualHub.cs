using System.Collections;
using UnityEngine;

using GameSystems.DTOs;

namespace GameSystems.Entities.MainStageScene
{
    // Dialogue Actor 퍼사드
    public class DialogueActorVisualHub : MonoBehaviour, IEntity
    {
        [SerializeField] private DialogueActorActivationPlugInHub activationHub;
        [SerializeField] private DialogueActorFaderPlugInHub faderHub;
        [SerializeField] private DialogueActorPositionerPlugInHub positionerHub;

        [SerializeField] private DialogueActorFaceSetterPlugInHub faceSetterHub;

        private void Awake()
        {
            var LocalRepository = Repository.MainStageSceneRepository.Instance;

            LocalRepository.Entity_LazyReferenceRepository.RegisterReference<DialogueActorVisualHub>(this);

            var LocalEntityRepository = Repository.MainStageSceneRepository.Instance.Entity_LazyReferenceRepository;
            this.activationHub = LocalEntityRepository.GetOrCreate<DialogueActorActivationPlugInHub>();
            this.faderHub = LocalEntityRepository.GetOrCreate<DialogueActorFaderPlugInHub>();
            this.positionerHub = LocalEntityRepository.GetOrCreate<DialogueActorPositionerPlugInHub>();

            this.faceSetterHub = LocalEntityRepository.GetOrCreate<DialogueActorFaceSetterPlugInHub>();
        }

        public bool TryGetAction(ActionDirectionData actionDirectionData, out IEnumerator enumerator)
        {
            enumerator = null;

            switch (actionDirectionData.ActionType)
            {
                case ActionType.DirectShow:
                    if (this.positionerHub.TryDirectPosition(actionDirectionData.TargetName, actionDirectionData.ActionPosition) &&
                        this.activationHub.TryDirectShow(actionDirectionData.TargetName)) return true;
                    else return false;
                case ActionType.DirectHide:
                    return this.activationHub.TryDirectHide(actionDirectionData.TargetName);

                case ActionType.FadeIn:
                    if (this.positionerHub.TryDirectPosition(actionDirectionData.TargetName, actionDirectionData.ActionPosition) &&
                        this.faderHub.TryFadeIn(actionDirectionData.TargetName, actionDirectionData.ActionTime, out enumerator)) return true;
                    else return false;
                case ActionType.FadeOut:
                    if (this.faderHub.TryFadeOut(actionDirectionData.TargetName, actionDirectionData.ActionTime, out enumerator)) return true;
                    else return false;
                case ActionType.Move:
                    if (this.positionerHub.TryMove(actionDirectionData.TargetName, actionDirectionData.ActionPosition, actionDirectionData.ActionTime, out enumerator)) return true;
                    else return false;
                case ActionType.None:
                    return true;
                default:
                    break;
            }

            return false;
        }

        public bool TrySetFace(FaceDirectionData faceDirectionData)
            => this.faceSetterHub.TrySetFace(faceDirectionData.TargetName, faceDirectionData.FaceType);
    }
}