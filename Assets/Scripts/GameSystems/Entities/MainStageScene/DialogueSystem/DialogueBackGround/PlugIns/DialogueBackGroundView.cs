using System.Collections;
using UnityEngine;

using GameSystems.PlainServices;

namespace GameSystems.Entities.MainStageScene
{
    public class DialogueBackGroundView : MonoBehaviour, IActivation, IFadeInAndOut
    {
        [SerializeField] private string Key;

        private DialogueBackGroundActivationPlugInHub DialogueBackGroundActivationPlugInHub;
        private DialogueBackGroundFaderPlugInHub DialogueBackGroundFaderPlugInHub;
        private IFadeInAndOutService FadeInAndOutService;

        [SerializeField] private GameObject BackGroundObject;
        [SerializeField] private SpriteRenderer SpriteRenderer;

        [SerializeField] private float ShowedAlpha;
        [SerializeField] private float HidedAlpha;

        private void Awake()
        {
            var GlobalRepository = Repository.GlobalSceneRepository.Instance;

            this.FadeInAndOutService = GlobalRepository.PlainServices_LazyReferenceRepository.GetOrCreate<FadeInAndOutService>();

            var LocalEntityRepository = Repository.MainStageSceneRepository.Instance.Entity_LazyReferenceRepository;
            this.DialogueBackGroundActivationPlugInHub = LocalEntityRepository.GetOrCreate<DialogueBackGroundActivationPlugInHub>();
            this.DialogueBackGroundFaderPlugInHub = LocalEntityRepository.GetOrCreate<DialogueBackGroundFaderPlugInHub>();

            this.DialogueBackGroundActivationPlugInHub.RegisterPlugIn(this.Key, this);
            this.DialogueBackGroundFaderPlugInHub.RegisterPlugIn(this.Key, this);

            this.Hide();
        }

        private void OnDestroy()
        {
            var LocalEntityRepository = Repository.MainStageSceneRepository.Instance.Entity_LazyReferenceRepository;
            this.DialogueBackGroundActivationPlugInHub = LocalEntityRepository.GetOrCreate<DialogueBackGroundActivationPlugInHub>();
            this.DialogueBackGroundFaderPlugInHub = LocalEntityRepository.GetOrCreate<DialogueBackGroundFaderPlugInHub>();

            this.DialogueBackGroundActivationPlugInHub.RemovePlugIn(this.Key);
            this.DialogueBackGroundFaderPlugInHub.RemovePlugIn(this.Key);
        }

        public void Show()
        {
            this.BackGroundObject.SetActive(true);
        }
        public void Hide()
        {
            this.BackGroundObject.SetActive(false);
        }


        public IEnumerator FadeIn(float duration)
        {
            this.FadeInAndOutService.SetAlphaValue(this.SpriteRenderer, this.HidedAlpha);
            this.Show();

            yield return Time.deltaTime;

            yield return this.FadeInAndOutService.FadeOperation(this.SpriteRenderer, this.HidedAlpha, this.ShowedAlpha, duration);
        }
        public IEnumerator FadeOut(float duration)
        {
            yield return this.FadeInAndOutService.FadeOperation(this.SpriteRenderer, this.ShowedAlpha, this.HidedAlpha, duration);

            this.Hide();
        }
    }
}