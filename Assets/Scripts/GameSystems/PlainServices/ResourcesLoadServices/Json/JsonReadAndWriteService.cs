using System;
using System.Threading.Tasks;
using System.IO;

using UnityEngine;

namespace GameSystems.PlainServices
{
    public interface IJsonReadAndWriteService
    {
        public bool TryWrite<T>(T targetData, string filePath) where T : class;
        public T Read<T>(string filePath) where T : class;
        public bool TryRead<T>(string filePath, out T resultData) where T : class;
        public Task<T> ReadAsync<T>(string filePath) where T : class;
    }

    public class JsonReadAndWriteService : IPlainService, IJsonReadAndWriteService
    {
        public bool TryWrite<T>(T targetData, string filePath) where T : class
        {
            // 경로값이 Empty임
            if (filePath.Equals(string.Empty))
            {
                Debug.LogError($"filePath값이 정상적이지 않음");
                return false;
            }

            // 상위 경로에 폴더가 없으면 생성.
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Debug.Log($"경로에 필요한 폴더 생성함.");
                Directory.CreateDirectory(directoryPath);
            }

            string json = JsonUtility.ToJson(targetData, true);
            File.WriteAllText(filePath, json);
            return true;
        }

        public T Read<T>(string filePath) where T : class
        {
            // 경로값이 Empty임
            if (filePath.Equals(string.Empty))
            {
                Debug.LogError($"filePath값이 정상적이지 않음");
                return null;
            }

            // 파일이 없으면, null 리턴
            if (!File.Exists(filePath))
            {
                Debug.LogError($"[StreamingAssetsLoader] File not found: {filePath}");
                return null;
            }

            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(json);
        }

        public bool TryRead<T>(string filePath, out T resultData) where T : class
        {
            // 경로값이 Empty임
            if (filePath.Equals(string.Empty))
            {
                Debug.LogError($"filePath값이 정상적이지 않음");
                resultData = null;
                return false;
            }

            // 파일이 없으면, null 리턴
            if (!File.Exists(filePath))
            {
                Debug.LogError($"[StreamingAssetsLoader] File not found: {filePath}");
                resultData = null;
                return false;
            }

            string json = File.ReadAllText(filePath);
            resultData = JsonUtility.FromJson<T>(json);
            return true;
        }

        public async Task<T> ReadAsync<T>(string filePath) where T : class
        {
            // 경로값이 Empty임
            if (filePath.Equals(string.Empty))
            {
                Debug.LogError($"filePath값이 정상적이지 않음");
                return null;
            }

            try
            {
                // 파일이 없으면, null 리턴
                if (!File.Exists(filePath))
                {
                    Debug.LogError($"[StreamingAssetsLoader] File not found: {filePath}");
                    return null;
                }

                string jsonText = await File.ReadAllTextAsync(filePath);
                return JsonUtility.FromJson<T>(jsonText);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[StreamingAssetsLoader] Load failed: {ex.GetType().Name} - {ex.Message}");
                return null;
            }
        }
    }
}