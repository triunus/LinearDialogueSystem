/*using System.Collections;
using UnityEngine;

using GameSystems.DTOs;

namespace GameSystems.Entities.MainStageScene
{
    // Dialogue BackGround 퍼사드
    public class DialogueBackGroundVisualHub : MonoBehaviour, IEntity
    {
        [SerializeField] private DialogueBackGroundActivationPlugInHub activationHub;
        [SerializeField] private DialogueBackGroundFaderPlugInHub faderHub;

        private void Awake()
        {
            var LocalRepository = Repository.MainStageSceneRepository.Instance;

            LocalRepository.Entity_LazyReferenceRepository.RegisterReference<DialogueBackGroundVisualHub>(this);

            var LocalEntityRepository = Repository.MainStageSceneRepository.Instance.Entity_LazyReferenceRepository;
            this.activationHub = LocalEntityRepository.GetOrCreate<DialogueBackGroundActivationPlugInHub>();
            this.faderHub = LocalEntityRepository.GetOrCreate<DialogueBackGroundFaderPlugInHub>();
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