using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using GameSystems.PlainServices;

namespace GameSystems.Entities.MainStageScene
{
    public class DialogueCanvasUIUXView : MonoBehaviour, IActivation, IFadeInAndOut
    {
        [SerializeField] private string Key;

        private IFadeInAndOutService FadeInAndOutService;

        [SerializeField] private GameObject CanvaseUIUXObject;
        [SerializeField] private Graphic[] CanvasImages;

        [SerializeField] private float ShowedAlpha;
        [SerializeField] private float HidedAlpha;

        private void Awake()
        {
            var GlobalRepository = Repository.GlobalSceneRepository.Instance;

            this.FadeInAndOutService = GlobalRepository.PlainServices_LazyReferenceRepository.GetOrCreate<FadeInAndOutService>();

            var LocalEntityRepository = Repository.MainStageSceneRepository.Instance.Entity_LazyReferenceRepository;

            var DialogueActivationPlugInHub = LocalEntityRepository.GetOrCreate<DialogueActivationPlugInHub>();
            var DialogueFaderPlugInHub = LocalEntityRepository.GetOrCreate<DialogueFaderPlugInHub>();

            DialogueActivationPlugInHub.RegisterPlugIn(this.Key, this);
            DialogueFaderPlugInHub.RegisterPlugIn(this.Key, this);

            this.Hide();
        }

        private void OnDestroy()
        {
            var LocalEntityRepository = Repository.MainStageSceneRepository.Instance.Entity_LazyReferenceRepository;

            var DialogueActivationPlugInHub = LocalEntityRepository.GetOrCreate<DialogueActivationPlugInHub>();
            var DialogueFaderPlugInHub = LocalEntityRepository.GetOrCreate<DialogueFaderPlugInHub>();

            DialogueActivationPlugInHub.RemovePlugIn(this.Key);
            DialogueFaderPlugInHub.RemovePlugIn(this.Key);
        }

        public IEnumerator FadeIn(float duration, DTOs.BehaviourToken behaviourToken)
        {
            this.FadeInAndOutService.SetAlphaValue(this.CanvasImages, this.HidedAlpha);
            this.Show();

            yield return Time.deltaTime;

            yield return this.FadeInAndOutService.FadeOperation(this.CanvasImages, this.HidedAlpha, this.ShowedAlpha, duration, behaviourToken);
        }
        public IEnumerator FadeOut(float duration, DTOs.BehaviourToken behaviourToken)
        {
            yield return this.FadeInAndOutService.FadeOperation(this.CanvasImages, this.ShowedAlpha, this.HidedAlpha, duration, behaviourToken);

            this.Hide();
        }

        public void Show()
        {
            this.CanvaseUIUXObject.SetActive(true);
        }
        public void Hide()
        {
            this.CanvaseUIUXObject.SetActive(false);
        }
    }
}