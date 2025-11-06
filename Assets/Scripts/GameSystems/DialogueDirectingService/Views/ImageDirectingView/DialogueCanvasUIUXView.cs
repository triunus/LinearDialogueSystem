using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using GameSystems.DialogueDirectingService.Datas;
using GameSystems.DialogueDirectingService.PlainServices;
using Foundations.PlugInHub;

namespace GameSystems.DialogueDirectingService.Views
{
    public class DialogueCanvasUIUXView : MonoBehaviour, IDialogueViewBinding, IActivation, IFadeInAndOut
    {
        private IFadeInAndOutService FadeInAndOutService;

        [SerializeField] private GameObject CanvaseUIUXObject;
        [SerializeField] private Graphic[] CanvasImages;

        [SerializeField] private float ShowedAlpha;
        [SerializeField] private float HidedAlpha;

        private void Awake()
        {
            this.Hide();
        }
        public void InitialBinding(string key, IMultiPlugInHub multiPlugInHub, IFadeInAndOutService fadeInAndOutService = null)
        {
            multiPlugInHub.RegisterPlugIn(key, this);
            multiPlugInHub.RegisterPlugIn(key, this);

            this.FadeInAndOutService = fadeInAndOutService;
        }


        public IEnumerator FadeIn(float duration, BehaviourToken behaviourToken)
        {
            this.FadeInAndOutService.SetAlphaValue(this.CanvasImages, this.HidedAlpha);
            this.Show();

            yield return Time.deltaTime;

            yield return this.FadeInAndOutService.FadeOperation(this.CanvasImages, this.HidedAlpha, this.ShowedAlpha, duration, behaviourToken);
        }
        public IEnumerator FadeOut(float duration, BehaviourToken behaviourToken)
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