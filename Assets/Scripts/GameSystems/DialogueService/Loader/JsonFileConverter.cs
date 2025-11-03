using UnityEngine;

namespace GameSystems.DialogueDirectingService.PlainServices
{
    public class JsonFileConverter
    {
        public bool TryParseJsonToData<T>(TextAsset jsonFile , out T resultResourceData) where T : class
        {
            resultResourceData = null;
            if (jsonFile == null || string.IsNullOrWhiteSpace(jsonFile.text))
            {
                Debug.LogError("jsonFile이 비어있습니다.");
                return false;
            }

            resultResourceData = JsonUtility.FromJson<T>(jsonFile.text);
            return true;
        }
    }
}