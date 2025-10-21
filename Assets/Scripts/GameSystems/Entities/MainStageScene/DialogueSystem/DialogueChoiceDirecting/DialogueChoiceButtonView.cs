using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameSystems.Entities.MainStageScene
{
    public interface IDialogueChoiceButtonView
    {
        public void ActivateObject(bool value);
        public void UpdateButtonConnect(UnityEngine.Events.UnityAction action);
        public void SetChoiceContent(string content);
        public bool IsUseable { get; set; }
    }

    public class DialogueChoiceButtonView : MonoBehaviour, IDialogueChoiceButtonView
    {
        [SerializeField] private GameObject ChoiceButtonViewObject;
        [SerializeField] private Button ChoiceButton;
        [SerializeField] private TextMeshProUGUI ChoiceContent;

        public void ActivateObject(bool value)
        {
            this.ChoiceButtonViewObject.SetActive(value);
        }

        public void UpdateButtonConnect(UnityEngine.Events.UnityAction action)
        {
            // 기존 버튼 연결 제거.
            this.ChoiceButton.onClick.RemoveAllListeners();
            this.ChoiceButton.onClick.AddListener(action);
        }
        public void SetChoiceContent(string content)
        {
            this.ChoiceContent.text = content;
        }

        public bool IsUseable { get; set; }
    }
}