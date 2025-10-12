using System;

namespace GameSystems.DTOs
{
    [Serializable]
    public class SaveAndLoadData
    {
        public GamePlayData GamePlayData;
        public CookingStoryScenarioData CookingStoryScenarioData;
        public CharacterStoryScenarioData CharacterStoryScenarioData;
    }

    [Serializable]
    public struct GamePlayData
    {
        public int DayNumber;
        public int RelationshipPoint;
        public StoryScenarioType StoryScenarioType;

        public GamePlayData(Models.GamePlayModel gamePlayModel)
        {
            this.DayNumber = gamePlayModel.DayNumber;
            this.RelationshipPoint = gamePlayModel.RelationshipPoint;
            this.StoryScenarioType = gamePlayModel.StoryScenarioType;
        }
    }

    [Serializable]
    public enum StoryScenarioType
    {
        CookingStoryType = 0,
        CharacterStoryType = 1
    }

    [Serializable]
    public struct CookingStoryScenarioData
    {
        public int CookingStoryScenarioID;

        // 애피소드를 반복하는 경우, 해당 애피소드의 게임 intro 생략.
        public bool Intro_HasBeenSeen;
        // 애피소드를 반복하는 경우, 해당 애피소드의 게임 Outtro 생략.
        public bool Outtro_HasBeenSeen;

        public CookingStoryScenarioData(Models.CookingStoryScenarioDataModel cookingStoryScenarioDataModel)
        {
            this.CookingStoryScenarioID = cookingStoryScenarioDataModel.CurrentCookingStoryScenarioID;
            this.Intro_HasBeenSeen = cookingStoryScenarioDataModel.Intro_HasBeenSeen;
            this.Outtro_HasBeenSeen = cookingStoryScenarioDataModel.Outtro_HasBeenSeen;
        }
    }

    public struct CharacterStoryScenarioData
    {
        public int CharacterStoryScenarioID;

        // 애피소드를 반복하는 경우, 해당 애피소드의 게임 intro 생략.
        public bool Intro_HasBeenSeen;

        public CharacterStoryScenarioData(Models.CharacterStoryScenarioDataModel characterStoryScenarioDataModel)
        {
            this.CharacterStoryScenarioID = characterStoryScenarioDataModel.CurrentCharacterStoryScenarioID;
            this.Intro_HasBeenSeen = characterStoryScenarioDataModel.Intro_HasBeenSeen;
        }
    }
}