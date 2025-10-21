using System.Collections;
using UnityEngine;
using TMPro;

namespace GameSystems.Entities.MainStageScene
{
    public interface IDialogueCutsceneDirectingView
    {
        public bool TryDirectCutsceneDisplayOperation(string directingContent, out IEnumerator enumerator);
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

        public bool TryDirectCutsceneDisplayOperation(string directingContent, out IEnumerator enumerator)
        {
            enumerator = null;

            if (!this.TryParseDirectingContent(directingContent, out var parsedContent)) return false;

            this.TitleText.text = parsedContent[1];
            this.SubTitleText.text = "- " + parsedContent[2] + " -";

            enumerator = this.OperateCutsceneDisplay(float.Parse(parsedContent[3]), float.Parse(parsedContent[4]), float.Parse(parsedContent[5]));
            return true;
        }
        private IEnumerator OperateCutsceneDisplay(float fadeInDuration, float wait, float fadeOutDuration)
        {
            // 일단 FadeOut 값으로 만들기.
            this.FadeInAndOutService.SetAlphaValue(this.CutsceneBackGroundImage, 0);
            this.FadeInAndOutService.SetAlphaValue(this.TitleText, 0);
            this.FadeInAndOutService.SetAlphaValue(this.SubTitleText, 0);
            // 활성화가 안되어 있으면 활성화.
            this.CutsceneBackGroundImage.gameObject.SetActive(true);
            this.TitleText.gameObject.SetActive(true);
            this.SubTitleText.gameObject.SetActive(true);

            yield return Time.deltaTime;

            yield return this.FadeInAndOutService.FadeOperation(new UnityEngine.UI.Graphic[] { this.CutsceneBackGroundImage, this.TitleText, this.SubTitleText }, 0, 1, fadeInDuration);

            yield return new WaitForSeconds(wait);

            yield return this.FadeInAndOutService.FadeOperation(new UnityEngine.UI.Graphic[] { this.CutsceneBackGroundImage, this.TitleText, this.SubTitleText }, 1, 0, fadeOutDuration);
        }

        // FadeInOut_Title01_Chapter01_1_1_1 이런 형식으로 와야됨.
        private bool TryParseDirectingContent(string directingContent, out string[] parsedContent)
        {
            parsedContent = directingContent.Split('_');

            if (parsedContent.Length == 6) return true;
            else return false;
        }
    }
}