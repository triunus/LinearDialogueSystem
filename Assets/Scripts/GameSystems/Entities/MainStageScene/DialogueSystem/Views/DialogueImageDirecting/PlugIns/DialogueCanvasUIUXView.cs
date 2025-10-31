using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using GameSystems.PlainServices;
using GameSystems.GameFlows.MainStageScene;

namespace GameSystems.Entities.MainStageScene
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
        public void InitialBinding(string key, IDialogueViewObjectModel dialogueViewModel, IFadeInAndOutService fadeInAndOutService = null)
        {
            dialogueViewModel.RegisterPlugIn(key, this);
            dialogueViewModel.RegisterPlugIn(key, this);

            this.FadeInAndOutService = fadeInAndOutService;
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