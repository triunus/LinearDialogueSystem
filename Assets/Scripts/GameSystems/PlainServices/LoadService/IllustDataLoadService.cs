using System;
using System.Collections;
using System.Threading.Tasks;
using System.IO;

using UnityEngine;

namespace GameSystems.PlainServices
{
    public interface IIllustDataLoadService
    {
        public Task<Texture2D> LoadIllustTexture2D(string detailPath);
    }

    public class IllustDataLoadService : IPlainService, IIllustDataLoadService
    {
        private string illustDirectoryPath;

        public IllustDataLoadService()
        {
            this.illustDirectoryPath = Path.Combine(Application.streamingAssetsPath, "Illustrations");
        }

        public async Task<Texture2D> LoadIllustTexture2D(string detailPath)
        {
            string path = Path.Combine(this.illustDirectoryPath, "Illust_" + detailPath + ".png");

            try
            {
                if (!File.Exists(path))
                {
                    Debug.LogError($"[StreamingAssetsLoader] File not found: {path}");
                    return null;
                }

                byte[] fileData = await File.ReadAllBytesAsync(path);
                Texture2D returnTexture2D = new Texture2D(2, 2);
                returnTexture2D.name = "Illust_" + detailPath;
                returnTexture2D.LoadImage(fileData);

                return returnTexture2D;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[StreamingAssetsLoader] Load failed: {ex.GetType().Name} - {ex.Message}");
                return null;
            }
        }
    }
}