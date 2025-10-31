using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.Entities.MainStageScene
{
    public interface IDialogueHistoryGenerator
    {
        public void AddDialogueHistory(int directingIndex, string directingContent);
    }

    // 이번 생각은,
    // 글을 모아 두었다가 한번에 갱신해주는거.
    // Prefab은 미리 생성. 할당은 미리 수행.
    // Text에 넣는걸 

    public class DialogueHistoryGenerator : MonoBehaviour, IEntity, IDialogueHistoryGenerator
    {
        [SerializeField] private GameObject DialogueHistoryObject;
        private bool isActivated = false;

        [SerializeField] private RectTransform HistoryScrollViewContent;

        [SerializeField] private GameObject LeftHistoryViewPrefab;
        [SerializeField] private GameObject RightHistoryViewPrefab;

        private Dictionary<string, Texture2D> ActorHistoryImages;
        [SerializeField] private Texture2D ActorA_Test;
        [SerializeField] private Texture2D ActorB_Test;
        [SerializeField] private Texture2D Player_Test;

        private Dictionary<int, GameObject> HistoryObjects;

        private void Awake()
        {
            var localRepository = Repository.MainStageSceneRepository.Instance;
            localRepository.Entity_LazyReferenceRepository.RegisterReference<DialogueHistoryGenerator>(this);

            this.ActorHistoryImages = new();
            this.HistoryObjects = new();

            this.ActorHistoryImages.Add("ActorA", this.ActorA_Test);
            this.ActorHistoryImages.Add("ActorB", this.ActorB_Test);
            this.ActorHistoryImages.Add("Player", this.Player_Test);

            this.DialogueHistoryObject.SetActive(false);
        }

        public void OnClickedHistoryButton()
        {
            if (this.isActivated)
            {
                this.isActivated = false;
                this.DialogueHistoryObject.SetActive(false);
            }
            else
            {
                this.isActivated = true;
                this.DialogueHistoryObject.SetActive(true);
            }
        }

        public void AddDialogueHistory(int directingIndex, string directingContent)
        {
            if(!this.TryParseDirectingContent(directingContent, out string target, out string actorName, out string content))
            {
                Debug.Log($"Parse가 잘못됨.");
                return;
            }

            // 이미 한번 History로 남겨준 기록인 경우, 넘어감.
            if (this.HistoryObjects.ContainsKey(directingIndex))
            {
                Debug.Log($"이미 한번 기록한 대화 내용입니다.");
                return;
            }

            if (!this.ActorHistoryImages.ContainsKey(target))
            {
                Debug.Log($"ActorName에 해당하는 Texture2D가 존재하지 않습니다.");
                return;
            }

            if (this.LeftHistoryViewPrefab == null || this.RightHistoryViewPrefab == null || this.HistoryScrollViewContent == null)
            {
                Debug.Log($"Prefab 등 필요한 리소스가 등록되어 있지 않습니다.");
                return;
            }

            GameObject newHistoryViewObject = null;

            if (target == "Player")
                newHistoryViewObject = Instantiate(this.RightHistoryViewPrefab, this.HistoryScrollViewContent);
            else
                newHistoryViewObject = Instantiate(this.LeftHistoryViewPrefab, this.HistoryScrollViewContent);

            IDialogueHistoryView newDialogueHistoryView = newHistoryViewObject.GetComponent<IDialogueHistoryView>();

            this.HistoryObjects.Add(directingIndex, newHistoryViewObject);
            newDialogueHistoryView.SetHistory(this.ActorHistoryImages[target], actorName, content);
        }

        private bool TryParseDirectingContent(string directingContent, out string target, out string actorName, out string content)
        {
            target = null;
            actorName = null;
            content = null;

            string[] parsedContent = directingContent.Split('_');
            if (parsedContent.Length != 3) return false;

            target = parsedContent[0];
            actorName = parsedContent[1];
            content = parsedContent[2];
            return true;
        }

        private IEnumerator WaitDeltaTime(GameObject newAddedObject)
        {
            yield return Time.deltaTime;

            newAddedObject.SetActive(false);
        }
    }
}