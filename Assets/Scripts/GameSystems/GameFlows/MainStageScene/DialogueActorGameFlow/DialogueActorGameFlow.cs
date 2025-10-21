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

            // 대화 스크립트에 필요한 Actor 이미지 Index가 담긴 Json파일 로드.
            string filePath = resourcesPathResolver.GetDialogueActorTablePath(DialogueStoryType.CookingStoryType);
            // Json 파일 로드 완료. ( 용량이 크지 않으나, 이후 Texture2D 로드를 비동기로 하는 것과 관련하여 일관성 유지 )
            DialogueActorTable_Json dialogueActorTable_Json = await jsonReadAndWriteService.ReadAsync<DialogueActorTable_Json>(filePath);
            // 특정 스크립트에 필요한 'Actor 이미지 이름들'을 가져옴.
            if(!this.TryParseTexture2D(dialogueActorTable_Json, 0, DialoguePhaseType.Intro, out var dialogueActorNames))
            {
                Debug.Log($"이미지 Texture2D 가져오기 실패함.");
                return;
            }

            // 각 Actor 이미지 경로 가져옴.
            string[] actorPngFilePath = resourcesPathResolver.GetDialogueActorTextrue2DPath(dialogueActorNames);

            // Texture2D에 사용할 .png 파일을 로드 후, Texture2D로 변환하여 반환.
            // 이후 비슷한 코드가 많아지면, byte를 받고 여기서 Texture2D를 Type으로 넘겨주는 방식으로 할 듯.
            Texture2D[] dialogueActors = await textrue2DLoadService.LoadIllustTexture2D(actorPngFilePath);

            // Texture2D[] 만큼 Actor 미리 만들어 놓기.
            this.InitialSetting(dialogueActorNames, dialogueActors);
        }

        private bool TryParseTexture2D(DialogueActorTable_Json dialogueActorTable_Json, int index, DialoguePhaseType dialoguePhaseType, out string[] dialogueActorNames)
        {
            dialogueActorNames = null;

            // 작업 실패 or DialogueActor 값이 비어있을 때, 실패 반환.
            if (!dialogueActorTable_Json.TryDialogueActorRow_Json(index, dialoguePhaseType, out var dialogueActorRow)
                || dialogueActorRow.Equals(string.Empty)) return false;

            // 공백 제거 후 쉼표로 분리
            dialogueActorNames = dialogueActorRow.DialogueActorNames.Replace(" ", "").Split(',');
            return true;
        }
    }
}*/