/*using System.Collections;
using UnityEngine;

using GameSystems.DTOs;

namespace GameSystems.Entities.MainStageScene
{
    // Dialogue CanvasUIUX 퍼사드
    public class DialogueCanvasUIUXVisualHub : MonoBehaviour, IEntity
    {
        private DialogueCanvasUIUXActivationPlugInHub activationHub;
        private DialogueCanvasUIUXFaderPlugInHub faderHub;

        private void Awake()
        {
            var LocalEntityRepository = Repository.MainStageSceneRepository.Instance.Entity_LazyReferenceRepository;

            LocalEntityRepository.RegisterReference<DialogueCanvasUIUXVisualHub>(this);

            this.activationHub = LocalEntityRepository.GetOrCreate<DialogueCanvasUIUXActivationPlugInHub>();
            this.faderHub = LocalEntityRepository.GetOrCreate<DialogueCanvasUIUXFaderPlugInHub>();
        }

        public bool TryGetAction(ActionDirectionData ActionDirectionData, out IEnumerator enumerator)
        {
            enumerator = null;

            switch (ActionDirectionData.ActionType)
            {
                case ActionType.DirectShow:
                    return this.activationHub.TryDirectShow(ActionDirectionData.TargetName);
                case ActionType.DirectHide:
                    return this.activationHub.TryDirectHide(ActionDirectionData.TargetName);
                case ActionType.FadeIn:
                    if (this.faderHub.TryFadeIn(ActionDirectionData.TargetName, ActionDirectionData.ActionTime, out enumerator)) return true;
                    else return false;
                case ActionType.FadeOut:
                    if (this.faderHub.TryFadeOut(ActionDirectionData.TargetName, ActionDirectionData.ActionTime, out enumerator)) return true;
                    else return false;
                default:
                    break;
            }

            return false;
        }
    }
}*/