/*using UnityEngine;

using GameSystems.DTOs;
using GameSystems.Entities.MainStageScene;

namespace GameSystems.GameFlows.MainStageScene
{
    public interface IDialogueActorGameFlow
    {
        public void InitialSetting(string[] dialogueActorNames, Texture2D[] dialogueActorTexture2Ds);
    }

    public class DialogueActorGameFlow : MonoBehaviour, IGameFlow, IDialogueActorGameFlow
    {
        [SerializeField] private GameObject DialogueActorPrefab;
        [SerializeField] private Transform DialogueActorParentObject;

        private void Awake()
        {
            this.InitialSetting_Parsing();
        }

        public void InitialSetting(string[] dialogueActorNames, Texture2D[] dialogueActorTexture2Ds)
        {
            for(int i = 0; i < dialogueActorNames.Length; ++i)
            {
                var newDialogueActorView = Instantiate(this.DialogueActorPrefab, DialogueActorParentObject).GetComponent<DialogueSpriteRendererView>();
                newDialogueActorView.SetTexture2D(dialogueActorNames[i], dialogueActorTexture2Ds[i]);

                newDialogueActorView.gameObject.SetActive(false);
            }
        }

        private async void InitialSetting_Parsing()
        {
            var GlobalRepository = Repository.GlobalSceneRepository.Instance;

            PlainServices.IResourcesPathResolver resourcesPathResolver =
                GlobalRepository.PlainServices_LazyReferenceRepository.GetOrCreate<PlainServices.ResourcesPathResolver>();
            PlainServices.IJsonReadAndWriteService jsonReadAndWriteService =
                GlobalRepository.PlainServices_LazyReferenceRepository.GetOrCreate<PlainServices.JsonReadAndWriteService>();
            PlainServices.Textrue2DLoadService textrue2DLoadService =
                GlobalRepository.PlainServices_LazyReferenceRepository.GetOrCreate<PlainServices.Textrue2DLoadService>();

            // ��ȭ ��ũ��Ʈ�� �ʿ��� Actor �̹��� Index�� ��� Json���� �ε�.
            string filePath = resourcesPathResolver.GetDialogueActorTablePath(DialogueStoryType.CookingStoryType);
            // Json ���� �ε� �Ϸ�. ( �뷮�� ũ�� ������, ���� Texture2D �ε带 �񵿱�� �ϴ� �Ͱ� �����Ͽ� �ϰ��� ���� )
            DialogueActorTable_Json dialogueActorTable_Json = await jsonReadAndWriteService.ReadAsync<DialogueActorTable_Json>(filePath);
            // Ư�� ��ũ��Ʈ�� �ʿ��� 'Actor �̹��� �̸���'�� ������.
            if(!this.TryParseTexture2D(dialogueActorTable_Json, 0, DialoguePhaseType.Intro, out var dialogueActorNames))
            {
                Debug.Log($"�̹��� Texture2D �������� ������.");
                return;
            }

            // �� Actor �̹��� ��� ������.
            string[] actorPngFilePath = resourcesPathResolver.GetDialogueActorTextrue2DPath(dialogueActorNames);

            // Texture2D�� ����� .png ������ �ε� ��, Texture2D�� ��ȯ�Ͽ� ��ȯ.
            // ���� ����� �ڵ尡 ��������, byte�� �ް� ���⼭ Texture2D�� Type���� �Ѱ��ִ� ������� �� ��.
            Texture2D[] dialogueActors = await textrue2DLoadService.LoadIllustTexture2D(actorPngFilePath);

            // Texture2D[] ��ŭ Actor �̸� ����� ����.
            this.InitialSetting(dialogueActorNames, dialogueActors);
        }

        private bool TryParseTexture2D(DialogueActorTable_Json dialogueActorTable_Json, int index, DialoguePhaseType dialoguePhaseType, out string[] dialogueActorNames)
        {
            dialogueActorNames = null;

            // �۾� ���� or DialogueActor ���� ������� ��, ���� ��ȯ.
            if (!dialogueActorTable_Json.TryDialogueActorRow_Json(index, dialoguePhaseType, out var dialogueActorRow)
                || dialogueActorRow.Equals(string.Empty)) return false;

            // ���� ���� �� ��ǥ�� �и�
            dialogueActorNames = dialogueActorRow.DialogueActorNames.Replace(" ", "").Split(',');
            return true;
        }
    }
}*/