using System;
using System.Threading.Tasks;
using System.IO;

using UnityEngine;

namespace GameSystems.PlainServices
{
    public interface IJsonReadAndWriteService
    {
        public void Wirte<T>(T targetData, string filePath) where T : class;
        public T Read<T>(string filePath) where T : class;
        public Task<T> ReadAsync<T>(string filePath) where T : class;
    }

    public class JsonReadAndWriteService : IPlainService, IJsonReadAndWriteService
    {
        public void Wirte<T>(T targetData, string filePath) where T : class
        {
            // 상위 경로에 폴더가 없으면 생성.
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Debug.Log($"경로에 필요한 폴더 생성함.");
                Directory.CreateDirectory(directoryPath);
            }

            string json = JsonUtility.ToJson(targetData, true);
            File.WriteAllText(filePath, json);
        }

        public T Read<T>(string filePath) where T : class
        {
            // 저장된 정보가 없으면 null 리턴.
            if (!File.Exists(filePath)) return null;

            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(json);
        }

        public async Task<T> ReadAsync<T>(string filePath) where T : class
        {
            if (filePath.Equals(string.Empty)) return null;

            try
            {
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