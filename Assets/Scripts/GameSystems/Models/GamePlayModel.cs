using System;

using GameSystems.DTOs;

namespace GameSystems.Models
{
    [System.Serializable]
    public class RuntimeUserDataModel : IModel
    {
        private GamePlayModel currentGamePlayModel;
        private CookingStoryScenarioDataModel currentCookingStoryScenarioDataModel;
        private CharacterStoryScenarioDataModel currentCharacterStoryScenarioDataModel;

        public GamePlayModel CurrentGamePlayModel { get => currentGamePlayModel; }
        public CookingStoryScenarioDataModel CurrentCookingStoryScenarioDataModel { get => currentCookingStoryScenarioDataModel; }
        public CharacterStoryScenarioDataModel CurrentCharacterStoryScenarioDataModel { get => currentCharacterStoryScenarioDataModel; }

        public RuntimeUserDataModel() { }

        public void SetNewGame()
        {
            this.currentGamePlayModel = new();
            this.currentCookingStoryScenarioDataModel = new();
            this.currentCharacterStoryScenarioDataModel = new();
        }
        
        public void SetLoadGame(SaveAndLoadData saveAndLoadData)
        {
            this.currentGamePlayModel.SetGamePlayModel(saveAndLoadData.GamePlayData);
            this.currentCookingStoryScenarioDataModel.SetDialogueModel(saveAndLoadData.CookingStoryScenarioData);
            this.currentCharacterStoryScenarioDataModel.SetDialogueModel(saveAndLoadData.CharacterStoryScenarioData);
        }
    }

    [System.Serializable]
    public class GamePlayModel
    {
        public int DayNumber;
        public int RelationshipPoint;
        public DialogueStoryType StoryScenarioType;

        public GamePlayModel()
        {
            this.DayNumber = 0;
            this.RelationshipPoint = 0;
            this.StoryScenarioType = DialogueStoryType.CookingStoryType;
        }

        public void SetGamePlayModel(GamePlayData gamePlayData)
        {
            this.DayNumber = gamePlayData.DayNumber;
            this.RelationshipPoint = gamePlayData.RelationshipPoint;
            this.StoryScenarioType = gamePlayData.StoryScenarioType;
        }
    }

    [System.Serializable]
    public class CookingStoryScenarioDataModel
    {
        public int CurrentCookingStoryScenarioID;

        // ���Ǽҵ带 �ݺ��ϴ� ���, �ش� ���Ǽҵ��� ���� intro ����.
        public bool Intro_HasBeenSeen;
        // ���Ǽҵ带 �ݺ��ϴ� ���, �ش� ���Ǽҵ��� ���� Outtro ����.
        public bool Outtro_HasBeenSeen;

        public CookingStoryScenarioDataModel()
        {
            this.CurrentCookingStoryScenarioID = 0;
            this.Intro_HasBeenSeen = false;
            this.Outtro_HasBeenSeen = false;
        }

        public void SetDialogueModel(DTOs.CookingStoryScenarioData cookingStoryScenarioData)
        {
            CurrentCookingStoryScenarioID = cookingStoryScenarioData.CookingStoryScenarioID;
            Intro_HasBeenSeen = cookingStoryScenarioData.Intro_HasBeenSeen;
            Outtro_HasBeenSeen = cookingStoryScenarioData.Outtro_HasBeenSeen;
        }
    }

    [System.Serializable]
    public class CharacterStoryScenarioDataModel
    {
        public int CurrentCharacterStoryScenarioID;

        // ���Ǽҵ带 �ݺ��ϴ� ���, �ش� ���Ǽҵ��� ���� intro ����.
        public bool Intro_HasBeenSeen;

        public CharacterStoryScenarioDataModel()
        {
            this.CurrentCharacterStoryScenarioID = 0;
            this.Intro_HasBeenSeen = false;
        }

        public void SetDialogueModel(DTOs.CharacterStoryScenarioData characterStoryScenarioData)
        {
            CurrentCharacterStoryScenarioID = characterStoryScenarioData.CharacterStoryScenarioID;
            Intro_HasBeenSeen = characterStoryScenarioData.Intro_HasBeenSeen;
        }
    }
}