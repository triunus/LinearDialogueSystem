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
        private IJsonReadAndWriteService JsonReadAndWriteService;

        private string savePath;

        public SaveAndLoadService()
        {
            this.savePath = System.IO.Path.Combine(Application.persistentDataPath, "SaveData", "Continue.json");

            var GlobalRepository = Repository.GlobalSceneRepository.Instance;
            this.JsonReadAndWriteService = GlobalRepository.PlainServices_LazyReferenceRepository.GetOrCreate<JsonReadAndWriteService>();
        }

        public void Save(RuntimeUserDataModel RuntimeUserDataModel)
        {
            SaveAndLoadData saveAndLoadData = new SaveAndLoadData();

            saveAndLoadData.GamePlayData = new GamePlayData(RuntimeUserDataModel.CurrentGamePlayModel);
            saveAndLoadData.CookingStoryScenarioData = new DTOs.CookingStoryScenarioData(RuntimeUserDataModel.CurrentCookingStoryScenarioDataModel);
            saveAndLoadData.CharacterStoryScenarioData = new CharacterStoryScenarioData(RuntimeUserDataModel.CurrentCharacterStoryScenarioDataModel);

            Debug.Log($"Saved Path : {this.savePath}");

            this.JsonReadAndWriteService.Wirte<SaveAndLoadData>(saveAndLoadData, this.savePath);
        }

        public bool TryLoad(out SaveAndLoadData saveAndLoadData)
        {
            saveAndLoadData = this.JsonReadAndWriteService.Read<SaveAndLoadData>(this.savePath);

            if (saveAndLoadData == null)
            {
                Debug.Log($"저장 정보가 없음.");
                return false;
            }

            return true;
        }
    }
}