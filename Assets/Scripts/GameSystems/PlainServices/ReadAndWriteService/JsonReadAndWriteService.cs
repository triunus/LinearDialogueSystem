using System.IO;
using UnityEngine;

namespace GameSystems.PlainServices
{
    public interface IJsonReadAndWriteService
    {
        public void Wirte<T>(T targetData, string filePath) where T : class;
        public T Read<T>(string filePath) where T : class;
    }

    public class JsonReadAndWriteService : IPlainService, IJsonReadAndWriteService
    {
        public void Wirte<T>(T targetData, string filePath) where T : class
        {
            // 상위 경로에 폴더가 없으면 생성.
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
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
    }
}