using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameSystems.GameFlows.MainStageScene;

namespace GameSystems.Entities.MainStageScene
{
    public interface IDialogueChoiceDirectingViewMediator
    {
        public bool TryDirectChoiceViewOperation(string directingContent, out IEnumerator enumerator, out DTOs.BehaviourToken behaviourToken);
        public void OnClicekdChoiceButton(int choiceButtonIndex);
    }

    public class DialogueChoiceDirectingViewMediator : MonoBehaviour, IDialogueChoiceDirectingViewMediator, IEntity
    {
        private IDialogueDirectingGameFlow DialogueDirectingSystemGameFlow;

        [SerializeField] private GameObject ChoiceButtonPrefab;
        [SerializeField] private RectTransform ChoiceButtonParent;

        private List<IDialogueChoiceButtonView> ChoiceButtonViews = new();

        private bool isButtonClickBlocked;
        private float safeWaitDuration = 0.3f;
        private float currentWaitDuration = 0f;

        private void Awake()
        {
            var LocalRepository = Repository.MainStageSceneRepository.Instance;

            LocalRepository.Entity_LazyReferenceRepository.RegisterReference<DialogueChoiceDirectingViewMediator>(this);

            this.DialogueDirectingSystemGameFlow = LocalRepository.GameFlow_LazyReferenceRepository.
                GetOrWaitReference<DialogueDirectingGameFlow>(x => this.DialogueDirectingSystemGameFlow = x);
        }

        public void InitialSetting()
        {

        }

        public bool TryDirectChoiceViewOperation(string directingContent, out IEnumerator enumerator, out DTOs.BehaviourToken behaviourToken)
        {
            enumerator = null;
            behaviourToken = null;

            // Parsing ���� ��, ����
            if (!this.TryParseChoiceContent(directingContent, out var parsedContent)) return false;
            // ��� ������ ButtonView �� ��������, ����.
            if (!this.TryGetUseableDialogueChoiceButtonView(out var dialogueChoiceButtonView)) return false;

            behaviourToken = new DTOs.BehaviourToken(isRequestEnd: false);
            enumerator = this.OperateDialogueChoiceDisplay(dialogueChoiceButtonView, parsedContent[0], int.Parse(parsedContent[1]), behaviourToken);
            return true;
        }
        private IEnumerator OperateDialogueChoiceDisplay(IDialogueChoiceButtonView view, string choiceContent, int jumpDirectingIndex, DTOs.BehaviourToken behaviourToken)
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
            while (this.currentWaitDuration < this.safeWaitDuration)
            {
                if (behaviourToken.IsRequestEnd) break;

                this.currentWaitDuration += Time.deltaTime;
                yield return Time.deltaTime;
            }

            // ��ư ���� ����.
            this.isButtonClickBlocked = false;
            this.currentWaitDuration = 0;
        }

        private bool TryParseChoiceContent(string directingContent, out string[] parsedContent)
        {
            parsedContent = directingContent.Split('_');

            if (parsedContent.Length == 2) return true;
            else return false;
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
            this.DialogueDirectingSystemGameFlow.OperateDialogueDirecting(choiceButtonIndex);
        }
    }
}