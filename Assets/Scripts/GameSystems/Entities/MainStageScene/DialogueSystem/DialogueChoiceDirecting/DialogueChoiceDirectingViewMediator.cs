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
            // Parsing 실패 시, 종료
            if (!this.TryParseChoiceContent(directingContent, out var parsedContent)) return false;
            // 사용 가능한 ButtonView 못 가져오면, 종료.
            if (!this.TryGetUseableDialogueChoiceButtonView(out var dialogueChoiceButtonView)) return false;

            enumerator = this.OperateDialogueChoiceDisplay(dialogueChoiceButtonView, parsedContent[0], int.Parse(parsedContent[1]));
            return true;
        }
        private IEnumerator OperateDialogueChoiceDisplay(IDialogueChoiceButtonView view, string choiceContent, int jumpDirectingIndex)
        {
            // 버튼이 잘못 입력 되지 않도록 잠시 차단.
            this.isButtonClickBlocked = true;

            // 해당 선택지 GameObject 활성화.
            view.ActivateObject(true);

            // 선택지 내용 갱신.
            view.SetChoiceContent(choiceContent);
            // 선택지 버튼 연결 갱신
            view.UpdateButtonConnect(() => this.OnClicekdChoiceButton(jumpDirectingIndex));

            // 안전하게 일정 시각 경과 후
            yield return Time.deltaTime * 5;

            // 버튼 차단 해제.
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
                // 가장 맨 앞의 사용가능한 View를 리턴.
                if (view.IsUseable)
                {
                    view.IsUseable = false;
                    DialogueChoiceButtonView = view;
                    return true;
                }
            }

            // 현재 갖고 있는 View를 다 사용 중인 경우, 새로 하나 만들어서 반환
            GameObject newChoiceButton = Instantiate(this.ChoiceButtonPrefab, this.ChoiceButtonParent);

            // GameObject 생성 오류
            if (newChoiceButton == null)
            {
                Debug.Log($"버튼 Prefab이 없음.");
                return false;
            }

            IDialogueChoiceButtonView newDialogueChoiceButtonView = newChoiceButton.GetComponent<IDialogueChoiceButtonView>();

            // 컴포넌트 참조 오류
            if (newDialogueChoiceButtonView == null)
            {
                Debug.Log($"Prefab에 View 컴포넌트 할당 안함.");
                return false;
            }

            this.ChoiceButtonViews.Add(newDialogueChoiceButtonView);
            newDialogueChoiceButtonView.IsUseable = false;

            DialogueChoiceButtonView = newDialogueChoiceButtonView;

            return true;
        }

        public void OnClicekdChoiceButton(int choiceButtonIndex)
        {
            // Block 되어 있으면 차단.
            if (this.isButtonClickBlocked) return;

            // 버튼 클릭 입력 들어오면 Block
            this.isButtonClickBlocked = true;

            // 버튼 비활성화 및 초기화.
            foreach(var view in this.ChoiceButtonViews)
            {
                view.ActivateObject(false);
                view.IsUseable = true;
            }

            // 여기서 대화 연출 시스템 기능 부분으로 연결.
        }
    }
}