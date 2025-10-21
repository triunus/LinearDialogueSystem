using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.Entities.MainStageScene
{
    public interface IDialogueChoiceDirectingViewMediator
    {
        public bool TryDirectChoiceViewOperation(string directingContent, out IEnumerator enumerator);
        public void OnClicekdChoiceButton(int choiceButtonIndex);
    }

    public class DialogueChoiceDirectingViewMediator : MonoBehaviour, IDialogueChoiceDirectingViewMediator, IEntity
    {
        [SerializeField] private GameObject ChoiceButtonPrefab;
        [SerializeField] private RectTransform ChoiceButtonParent;

        private List<IDialogueChoiceButtonView> ChoiceButtonViews = new();

        private bool isButtonClickBlocked;

        private void Awake()
        {
            var LocalRepository = Repository.MainStageSceneRepository.Instance;

            LocalRepository.Entity_LazyReferenceRepository.RegisterReference<DialogueChoiceDirectingViewMediator>(this);
        }

        public bool TryDirectChoiceViewOperation(string directingContent, out IEnumerator enumerator)
        {
            enumerator = null;
            // Parsing ���� ��, ����
            if (!this.TryParseChoiceContent(directingContent, out var parsedContent)) return false;
            // ��� ������ ButtonView �� ��������, ����.
            if (!this.TryGetUseableDialogueChoiceButtonView(out var dialogueChoiceButtonView)) return false;

            enumerator = this.OperateDialogueChoiceDisplay(dialogueChoiceButtonView, parsedContent[0], int.Parse(parsedContent[1]));
            return true;
        }
        private IEnumerator OperateDialogueChoiceDisplay(IDialogueChoiceButtonView view, string choiceContent, int jumpDirectingIndex)
        {
            // ��ư�� �߸� �Է� ���� �ʵ��� ��� ����.
            this.isButtonClickBlocked = true;

            // �ش� ������ GameObject Ȱ��ȭ.
            view.ActivateObject(true);

            // ������ ���� ����.
            view.SetChoiceContent(choiceContent);
            // ������ ��ư ���� ����
            view.UpdateButtonConnect(() => this.OnClicekdChoiceButton(jumpDirectingIndex));

            // �����ϰ� ���� �ð� ��� ��
            yield return Time.deltaTime * 5;

            // ��ư ���� ����.
            this.isButtonClickBlocked = false;
        }

        private bool TryParseChoiceContent(string directingContent, out string[] parsedContent)
        {
            parsedContent = directingContent.Split('_');

            if (parsedContent.Length != 2) return false;
            else return true;
        }
        private bool TryGetUseableDialogueChoiceButtonView(out IDialogueChoiceButtonView DialogueChoiceButtonView)
        {
            DialogueChoiceButtonView = null;

            foreach (var view in this.ChoiceButtonViews)
            {
                // ���� �� ���� ��밡���� View�� ����.
                if (view.IsUseable)
                {
                    view.IsUseable = false;
                    DialogueChoiceButtonView = view;
                    return true;
                }
            }

            // ���� ���� �ִ� View�� �� ��� ���� ���, ���� �ϳ� ���� ��ȯ
            GameObject newChoiceButton = Instantiate(this.ChoiceButtonPrefab, this.ChoiceButtonParent);

            // GameObject ���� ����
            if (newChoiceButton == null)
            {
                Debug.Log($"��ư Prefab�� ����.");
                return false;
            }

            IDialogueChoiceButtonView newDialogueChoiceButtonView = newChoiceButton.GetComponent<IDialogueChoiceButtonView>();

            // ������Ʈ ���� ����
            if (newDialogueChoiceButtonView == null)
            {
                Debug.Log($"Prefab�� View ������Ʈ �Ҵ� ����.");
                return false;
            }

            this.ChoiceButtonViews.Add(newDialogueChoiceButtonView);
            newDialogueChoiceButtonView.IsUseable = false;

            DialogueChoiceButtonView = newDialogueChoiceButtonView;

            return true;
        }

        public void OnClicekdChoiceButton(int choiceButtonIndex)
        {
            // Block �Ǿ� ������ ����.
            if (this.isButtonClickBlocked) return;

            // ��ư Ŭ�� �Է� ������ Block
            this.isButtonClickBlocked = true;

            // ��ư ��Ȱ��ȭ �� �ʱ�ȭ.
            foreach(var view in this.ChoiceButtonViews)
            {
                view.ActivateObject(false);
                view.IsUseable = true;
            }

            // ���⼭ ��ȭ ���� �ý��� ��� �κ����� ����.
        }
    }
}