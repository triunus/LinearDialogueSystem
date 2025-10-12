using UnityEngine;

using System.IO;
using GameSystems.Models;
using GameSystems.DTOs;
using GameSystems.PlainServices;

namespace GameSystems.GameFlows.EmptyScene
{
    public interface IScenarioDataLoadService
    {
        public bool TryLoadDialogueData(RuntimeUserDataModel runtimeUserDataModel, out DialogueTable dialogueTable);
    }

    // 이거 SO 읽어오는 것으로 변경.
    public class ScenarioDialogueDataLoadService : IPlainService, IScenarioDataLoadService
    {
        private IJsonReadAndWriteService JsonReadAndWriteService;

        public ScenarioDialogueDataLoadService()
        {
            var GlobalRepository = Repository.GlobalSceneRepository.Instance;
            this.JsonReadAndWriteService = GlobalRepository.PlainServices_LazyReferenceRepository.GetOrCreate<JsonReadAndWriteService>();
        }

        public bool TryLoadDialogueData(RuntimeUserDataModel runtimeUserDataModel, out DialogueTable dialogueTable)
        {
            // 필요한 데이터를 가져오는 경로 값 추출.
            string path = this.GetDetailPath(runtimeUserDataModel);

            dialogueTable = this.JsonReadAndWriteService.Read<DialogueTable>(path);

            Debug.Log($"path : {path}");

            if (dialogueTable == null)
            {
                Debug.Log($"저장 정보가 없음.");
                return false;
            }

            return true;
        }

        // 필요한 데이터를 가져오는 경로 값 추출.
        private string GetDetailPath(RuntimeUserDataModel runtimeUserDataModel)
        {
            if (runtimeUserDataModel.CurrentGamePlayModel.StoryScenarioType == DTOs.StoryScenarioType.CookingStoryType)
            {
                return Path.Combine(Application.streamingAssetsPath, "CookingStoryScenarios", "CookingStory_" + runtimeUserDataModel.CurrentCookingStoryScenarioDataModel.CurrentCookingStoryScenarioID.ToString() + ".json");
            }
            else if (runtimeUserDataModel.CurrentGamePlayModel.StoryScenarioType == DTOs.StoryScenarioType.CharacterStoryType)
            {
                return Path.Combine(Application.streamingAssetsPath, "CharacterStoryScenarios", "CharacterStory_" + runtimeUserDataModel.CurrentCharacterStoryScenarioDataModel.CurrentCharacterStoryScenarioID.ToString() + ".json");
            }
            else
            {
                UnityEngine.Debug.Log($"로컬 데이터 Read 오류남");
                return null;
            }
        }
    }

}