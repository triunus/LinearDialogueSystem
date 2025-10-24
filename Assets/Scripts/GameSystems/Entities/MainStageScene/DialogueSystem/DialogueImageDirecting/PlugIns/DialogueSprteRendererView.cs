using System.Collections;
using UnityEngine;

using GameSystems.PlainServices;

namespace GameSystems.Entities.MainStageScene
{
    public class DialogueSprteRendererView : MonoBehaviour, IActivation, IFadeInAndOut
    {
        [SerializeField] private string Key;

        private IFadeInAndOutService FadeInAndOutService;

        [SerializeField] private GameObject BackGroundObject;
        [SerializeField] private SpriteRenderer[] SpriteRenderers;

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


        public void Show()
        {
            this.BackGroundObject.SetActive(true);
        }
        public void Hide()
        {
            this.BackGroundObject.SetActive(false);
        }


        public IEnumerator FadeIn(float duration, DTOs.BehaviourToken behaviourToken)
        {
            this.FadeInAndOutService.SetAlphaValue(this.SpriteRenderers, this.HidedAlpha);
            this.Show();

            yield return Time.deltaTime;

            yield return this.FadeInAndOutService.FadeOperation(this.SpriteRenderers, this.HidedAlpha, this.ShowedAlpha, duration, behaviourToken);
        }
        public IEnumerator FadeOut(float duration, DTOs.BehaviourToken behaviourToken)
        {
            yield return this.FadeInAndOutService.FadeOperation(this.SpriteRenderers, this.ShowedAlpha, this.HidedAlpha, duration, behaviourToken);

            this.Hide();
        }
    }
}