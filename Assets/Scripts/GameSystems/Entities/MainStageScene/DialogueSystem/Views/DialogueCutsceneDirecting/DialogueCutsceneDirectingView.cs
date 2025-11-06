/*using System.Collections;
using UnityEngine;
using TMPro;

namespace GameSystems.Entities.MainStageScene
{
    public interface IDialogueCutsceneDirectingView
    {
        public bool TryDirectCutsceneDisplayOperation(string directingContent, out IEnumerator enumerator, out DTOs.BehaviourToken behaviourToken);
    }

    public class DialogueCutsceneDirectingView : MonoBehaviour, IEntity, IDialogueCutsceneDirectingView
    {
        private PlainServices.IFadeInAndOutService FadeInAndOutService;

        [SerializeField] private UnityEngine.UI.Image CutsceneBackGroundImage;
        [SerializeField] private TextMeshProUGUI TitleText;
        [SerializeField] private TextMeshProUGUI SubTitleText;

        private void Awake()
        {
            var GlobalRepository = Repository.GlobalSceneRepository.Instance;

            this.FadeInAndOutService = GlobalRepository.PlainServices_LazyReferenceRepository.GetOrCreate<PlainServices.FadeInAndOutService>();

            var LocalRepository = Repository.MainStageSceneRepository.Instance;

            LocalRepository.Entity_LazyReferenceRepository.RegisterReference<DialogueCutsceneDirectingView>(this);
        }

        public bool TryDirectCutsceneDisplayOperation(string directingContent, out IEnumerator enumerator, out DTOs.BehaviourToken behaviourToken)
        {
            enumerator = null;
            behaviourToken = null;
            if (!this.TryParseDirectingContent(directingContent, out var parsedContent)) return false;

            this.TitleText.text = parsedContent[0];
            this.SubTitleText.text = "- " + parsedContent[1] + " -";

            behaviourToken = new DTOs.BehaviourToken(isRequestEnd: false);
            enumerator = this.OperateCutsceneDisplay(float.Parse(parsedContent[2]), float.Parse(parsedContent[3]), float.Parse(parsedContent[4]), behaviourToken);
            return true;
        }
        private IEnumerator OperateCutsceneDisplay(float fadeInDuration, float requestWaitDuration, float fadeOutDuration, DTOs.BehaviourToken behaviourToken)
        {
            // 일단 FadeOut 값으로 만들기.
            this.StartState();

            yield return Time.deltaTime;

            yield return this.FadeInAndOutService.FadeOperation(new UnityEngine.UI.Graphic[] { this.CutsceneBackGroundImage, this.TitleText, this.SubTitleText }, 0, 1, fadeInDuration, behaviourToken);
            if (behaviourToken.IsRequestEnd) this.FinalState();

            float currentWaitedTime = 0f;
            while (currentWaitedTime < requestWaitDuration)
            {
                if (behaviourToken.IsRequestEnd) this.FinalState();

                currentWaitedTime += Time.deltaTime;
                yield return Time.deltaTime;
            }

            yield return this.FadeInAndOutService.FadeOperation(new UnityEngine.UI.Graphic[] { this.CutsceneBackGroundImage, this.TitleText, this.SubTitleText }, 1, 0, fadeOutDuration, behaviourToken);
            if (behaviourToken.IsRequestEnd) this.FinalState();
        }
        private void StartState()
        {
            // 일단 FadeOut 값으로 만들기.
            this.FadeInAndOutService.SetAlphaValue(this.CutsceneBackGroundImage, 0);
            this.FadeInAndOutService.SetAlphaValue(this.TitleText, 0);
            this.FadeInAndOutService.SetAlphaValue(this.SubTitleText, 0);

            // 활성화가 안되어 있으면 활성화.
            this.CutsceneBackGroundImage.gameObject.SetActive(true);
            this.TitleText.gameObject.SetActive(true);
            this.SubTitleText.gameObject.SetActive(true);
        }
        private void FinalState()
        {
            this.FadeInAndOutService.SetAlphaValue(this.CutsceneBackGroundImage, 0);
            this.FadeInAndOutService.SetAlphaValue(this.TitleText, 0);
            this.FadeInAndOutService.SetAlphaValue(this.SubTitleText, 0);

            this.CutsceneBackGroundImage.gameObject.SetActive(false);
            this.TitleText.gameObject.SetActive(false);
            this.SubTitleText.gameObject.SetActive(false);
        }

        // FadeInOut_Title01_Chapter01_1_1_1 이런 형식으로 와야됨.
        private bool TryParseDirectingContent(string directingContent, out string[] parsedContent)
        {
            parsedContent = directingContent.Split('_');

            if (parsedContent.Length == 5) return true;
            else return false;
        }
    }
}*/