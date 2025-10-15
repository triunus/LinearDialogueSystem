using UnityEngine;

using GameSystems.Models;
using GameSystems.DTOs;

namespace GameSystems.PlainServices
{
    public interface ISaveAndLoadService
    {
        public void Save(RuntimeUserDataModel RuntimeUserDataModel);
        public bool TryLoad(out SaveAndLoadData saveAndLoadData);
    }

    public class SaveAndLoadService : IPlainService, ISaveAndLoadService
    {
        private IResourcesPathResolver ResourcesPathResolver;
        private IJsonReadAndWriteService JsonReadAndWriteService;

        public SaveAndLoadService()
        {
            var GlobalRepository = Repository.GlobalSceneRepository.Instance;
            this.ResourcesPathResolver = GlobalRepository.PlainServices_LazyReferenceRepository.GetOrCreate<ResourcesPathResolver>();
            this.JsonReadAndWriteService = GlobalRepository.PlainServices_LazyReferenceRepository.GetOrCreate<JsonReadAndWriteService>();
        }

        public void Save(RuntimeUserDataModel RuntimeUserDataModel)
        {
            string savePath = this.ResourcesPathResolver.GetSaveAndLoadCombinePath();

            SaveAndLoadData saveAndLoadData = new SaveAndLoadData();

            saveAndLoadData.GamePlayData = new GamePlayData(RuntimeUserDataModel.CurrentGamePlayModel);
            saveAndLoadData.CookingStoryScenarioData = new DTOs.CookingStoryScenarioData(RuntimeUserDataModel.CurrentCookingStoryScenarioDataModel);
            saveAndLoadData.CharacterStoryScenarioData = new CharacterStoryScenarioData(RuntimeUserDataModel.CurrentCharacterStoryScenarioDataModel);

            this.JsonReadAndWriteService.Wirte<SaveAndLoadData>(saveAndLoadData, savePath);
        }

        public bool TryLoad(out SaveAndLoadData saveAndLoadData)
        {
            string savePath = this.ResourcesPathResolver.GetSaveAndLoadCombinePath();

            saveAndLoadData = this.JsonReadAndWriteService.Read<SaveAndLoadData>(savePath);

            if (saveAndLoadData == null)
            {
                Debug.Log($"저장 정보가 없음.");
                return false;
            }

            return true;
        }
    }
}