using UnityEngine;

namespace GameSystems.DialogueDirectingService.PlainServices
{
    public class ScriptableObjectLoader
    {
        public bool TryGetLoadScriptableObject<T>(string path, out T resultScriptableObject) where T : ScriptableObject
        {
            // SO를 못찾을 경우 false 리턴.
            resultScriptableObject = Resources.Load<T>(path);

            // 파일 못찾으면 false 리턴.
            if (resultScriptableObject == null) return false;
            return true;
        }
    }
}