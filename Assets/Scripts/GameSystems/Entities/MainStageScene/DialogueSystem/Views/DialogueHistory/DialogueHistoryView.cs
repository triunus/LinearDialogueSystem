using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameSystems.Entities.MainStageScene
{
    public interface IDialogueHistoryView
    {
        public void SetHistory(Texture2D actorImage, string name, string content);
        public void RefreshTextGUI();
    }

    public class DialogueHistoryView : MonoBehaviour, IDialogueHistoryView
    {
        [SerializeField] private Image ActorImage;
        [SerializeField] private TextMeshProUGUI ActorName;
        [SerializeField] private TextMeshProUGUI Content;

        [SerializeField] private bool IsRightHistoryPrefab;

        private void OnEnable()
        {
            this.RefreshTextGUI();
        }

        public void SetHistory(Texture2D actorImage, string name, string content)
        {
            this.ActorImage.sprite = Sprite.Create( actorImage,
            new Rect(0, 0, actorImage.width, actorImage.height),
            new Vector2(0.5f, 0.5f)
            );

            this.ActorName.text = name;
            this.Content.text = content;
        }

        public void RefreshTextGUI()
        {
            this.Content.ForceMeshUpdate();

            int currentLine = this.Content.textInfo.lineCount;

            if (this.IsRightHistoryPrefab && currentLine <= 1)
            {
                this.Content.alignment = TextAlignmentOptions.Right;
            }
            else
            {
                this.Content.alignment = TextAlignmentOptions.Left;
            }
        }
    }
}