using System.Collections;
using UnityEngine;

using Foundations.PlugInHub;
using GameSystems.DialogueDirectingService.PlainServices;
using GameSystems.DialogueDirectingService.Datas;

namespace GameSystems.DialogueDirectingService.Views
{
    public class DialogueSpriteRendererView : MonoBehaviour, IDialogueViewBinding, IActivation, IFadeInAndOut
    {
        private IFadeInAndOutService FadeInAndOutService;

        [SerializeField] private GameObject BackGroundObject;
        [SerializeField] private SpriteRenderer[] SpriteRenderers;

        [SerializeField] private float ShowedAlpha;
        [SerializeField] private float HidedAlpha;

        private void Awake()
        {
            this.Hide();
        }
        public void InitialBinding(string key, IMultiPlugInHub multiPlugInHub, IFadeInAndOutService fadeInAndOutService = null)
        {
            multiPlugInHub.RegisterPlugIn<IActivation>(key, this);
            multiPlugInHub.RegisterPlugIn<IFadeInAndOut>(key, this);

            this.FadeInAndOutService = fadeInAndOutService;
        }

        public void Show()
        {
            this.BackGroundObject.SetActive(true);
        }
        public void Hide()
        {
            this.BackGroundObject.SetActive(false);
        }

        public IEnumerator FadeIn(float duration, BehaviourToken behaviourToken)
        {
            this.FadeInAndOutService.SetAlphaValue(this.SpriteRenderers, this.HidedAlpha);
            this.Show();

            yield return Time.deltaTime;

            yield return this.FadeInAndOutService.FadeOperation(this.SpriteRenderers, this.HidedAlpha, this.ShowedAlpha, duration, behaviourToken);
        }
        public IEnumerator FadeOut(float duration, BehaviourToken behaviourToken)
        {
            yield return this.FadeInAndOutService.FadeOperation(this.SpriteRenderers, this.ShowedAlpha, this.HidedAlpha, duration, behaviourToken);

            this.Hide();
        }
    }
}