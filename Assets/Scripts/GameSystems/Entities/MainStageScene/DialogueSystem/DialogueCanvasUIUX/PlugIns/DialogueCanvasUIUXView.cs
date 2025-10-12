using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using GameSystems.PlainServices;

namespace GameSystems.Entities.MainStageScene
{
    public class DialogueCanvasUIUXView : MonoBehaviour, IActivation, IFadeInAndOut
    {
        [SerializeField] private string Key;

        private DialogueCanvasUIUXActivationPlugInHub DialogueCanvasUIUXActivationPlugInHub;
        private DialogueCanvasUIUXFaderPlugInHub DialogueCanvasUIUXFaderPlugInHub;
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
            this.DialogueCanvasUIUXActivationPlugInHub = LocalEntityRepository.GetOrCreate<DialogueCanvasUIUXActivationPlugInHub>();
            this.DialogueCanvasUIUXFaderPlugInHub = LocalEntityRepository.GetOrCreate<DialogueCanvasUIUXFaderPlugInHub>();

            this.DialogueCanvasUIUXActivationPlugInHub.RegisterPlugIn(this.Key, this);
            this.DialogueCanvasUIUXFaderPlugInHub.RegisterPlugIn(this.Key, this);

            this.Hide();
        }

        private void OnDestroy()
        {
            var LocalEntityRepository = Repository.MainStageSceneRepository.Instance.Entity_LazyReferenceRepository;
            this.DialogueCanvasUIUXActivationPlugInHub = LocalEntityRepository.GetOrCreate<DialogueCanvasUIUXActivationPlugInHub>();
            this.DialogueCanvasUIUXFaderPlugInHub = LocalEntityRepository.GetOrCreate<DialogueCanvasUIUXFaderPlugInHub>();

            this.DialogueCanvasUIUXActivationPlugInHub.RemovePlugIn(this.Key);
            this.DialogueCanvasUIUXFaderPlugInHub.RemovePlugIn(this.Key);
        }

        public IEnumerator FadeIn(float duration)
        {
            this.FadeInAndOutService.SetAlphaValue(this.CanvasImages, this.HidedAlpha);
            this.Show();

            yield return Time.deltaTime;

            yield return this.FadeInAndOutService.FadeOperation(this.CanvasImages, this.HidedAlpha, this.ShowedAlpha, duration);
        }
        public IEnumerator FadeOut(float duration)
        {
            yield return this.FadeInAndOutService.FadeOperation(this.CanvasImages, this.ShowedAlpha, this.HidedAlpha, duration);

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