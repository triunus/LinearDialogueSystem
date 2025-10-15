using System.IO;

using UnityEngine;
using GameSystems.DTOs;

namespace GameSystems.PlainServices
{
    public interface IResourcesPathResolver
    {
        public string GetDialogueStoryPath(DialogueStoryType storyScenarioType, int dialogueIndex, DialoguePhaseType dialoguePhaseType);
        public string GetNextDialogueTable(DialogueStoryType dialogueStoryType);
        public string GetStroyBranchTable(DialogueStoryType dialogueStoryType);

        public string GetDialogueActorTablePath(DialogueStoryType dialogueStoryType);
        public string[] GetDialogueActorTextrue2DPath(string[] detailPath);

        public string GetSaveAndLoadCombinePath();
    }

    public class ResourcesPathResolver : IPlainService, IResourcesPathResolver
    {
        // 스토리 스크립트 경로 반환.
        public string GetDialogueStoryPath(DialogueStoryType dialogueStoryType, int dialogueIndex, DialoguePhaseType dialoguePhaseType)
        {
            if (dialogueStoryType == DialogueStoryType.CookingStoryType)
            {
                return Path.Combine(Application.streamingAssetsPath, "Dialogues", "CookingStorys", "CookingStory_" + dialogueIndex.ToString(), "CookingStory_" + dialogueIndex.ToString() + "_" + dialoguePhaseType.ToString() + ".json");
            }
            else if (dialogueStoryType == DialogueStoryType.CharacterStoryType)
            {
                return Path.Combine(Application.streamingAssetsPath, "Dialogues", "CharacterStorys", "CharacterStory_" + dialogueIndex.ToString(), "CharacterStory_" + dialogueIndex.ToString() + "_" + dialoguePhaseType.ToString() + ".json");
            }
            else
            {
                UnityEngine.Debug.Log($"DialogueStoryPath 메소드 리턴 오류남");
                return string.Empty;
            }
        }
        // 각 스토리의 결과에 따른 다음 스토리 Index가 기록된 NextDialogueTable 경로 반환.
        public string GetNextDialogueTable(DialogueStoryType dialogueStoryType)
        {
            if (dialogueStoryType == DialogueStoryType.CookingStoryType)
            {
                return Path.Combine(Application.streamingAssetsPath, "Dialogues", "CookingStorys", "NextDialogueTable.json");
            }
            else if (dialogueStoryType == DialogueStoryType.CharacterStoryType)
            {
                return Path.Combine(Application.streamingAssetsPath, "Dialogues", "CharacterStorys", "NextDialogueTable.json");
            }
            else
            {
                UnityEngine.Debug.Log($"DialogueActorTablePath 리턴 오류남");
                return string.Empty;
            }
        }
        // 각 스토리의 본적 있는지의 여부에 따른 분기 Index가 기록된 StroyBranchTable 경로 반환.
        public string GetStroyBranchTable(DialogueStoryType dialogueStoryType)
        {
            if (dialogueStoryType == DialogueStoryType.CookingStoryType)
            {
                return Path.Combine(Application.streamingAssetsPath, "Dialogues", "CookingStorys", "StroyBranchTable.json");
            }
            else if (dialogueStoryType == DialogueStoryType.CharacterStoryType)
            {
                return Path.Combine(Application.streamingAssetsPath, "Dialogues", "CharacterStorys", "StroyBranchTable.json");
            }
            else
            {
                UnityEngine.Debug.Log($"DialogueActorTablePath 리턴 오류남");
                return string.Empty;
            }
        }

        // 각 스토리에서 필요한 DialogueActor Index가 기록된 DialogueActorTable 경로 반환
        public string GetDialogueActorTablePath(DialogueStoryType dialogueStoryType)
        {
            if (dialogueStoryType == DialogueStoryType.CookingStoryType)
            {
                return Path.Combine(Application.streamingAssetsPath, "Dialogues", "CookingStorys", "DialogueActorTable.json");
            }
            else if (dialogueStoryType == DialogueStoryType.CharacterStoryType)
            {
                return Path.Combine(Application.streamingAssetsPath, "Dialogues", "CharacterStorys", "DialogueActorTable.json");
            }
            else
            {
                UnityEngine.Debug.Log($"DialogueActorTablePath 리턴 오류남");
                return string.Empty;
            }
        }
        // DialogueActor Index를 통해, 해당 DialogueActor 경로 반환
        public string[] GetDialogueActorTextrue2DPath(string[] detailPath)
        {
            string[] filePath = new string[detailPath.Length];

            for(int i = 0; i < detailPath.Length; ++i)
            {
                filePath[i] = Path.Combine(Application.streamingAssetsPath, "DialogueActors", detailPath[i] + ".png");
            }

            return filePath;
        }

        // 저장 파일 경로 반환.
        public string GetSaveAndLoadCombinePath()
        {
            return Path.Combine(Application.streamingAssetsPath, "SaveData", "Continue.json");
        }
    }
}