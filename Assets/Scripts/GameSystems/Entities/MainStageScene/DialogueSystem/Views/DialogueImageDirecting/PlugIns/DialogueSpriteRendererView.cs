using System.Collections;
using UnityEngine;

using GameSystems.PlainServices;
using GameSystems.GameFlows.MainStageScene;

namespace GameSystems.Entities.MainStageScene
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
        public void InitialBinding(string key, IDialogueViewObjectModel dialogueViewModel, IFadeInAndOutService fadeInAndOutService = null)
        {
            dialogueViewModel.RegisterPlugIn<IActivation>(key, this);
            dialogueViewModel.RegisterPlugIn<IFadeInAndOut>(key, this);

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