using System;
using System.Threading.Tasks;
using System.IO;

using UnityEngine;

namespace GameSystems.PlainServices
{
    public interface ITextrue2DLoadService
    {
        public Task<Texture2D> LoadIllustTexture2D(string detailPath);
    }

    public class Textrue2DLoadService : IPlainService, ITextrue2DLoadService
    {
        public async Task<Texture2D> LoadIllustTexture2D(string detailPath)
        {
            try
            {
                if (!File.Exists(detailPath))
                {
                    Debug.LogError($"[StreamingAssetsLoader] File not found: {detailPath}");
                    return null;
                }

                byte[] fileData = await File.ReadAllBytesAsync(detailPath);
                // 2x2 크기의 빈 텍스처 객체를 생성
                Texture2D returnTexture2D = new Texture2D(2, 2);
                returnTexture2D.name = detailPath;
                // 이미지 파일의 바이너리 데이터를 해석
                returnTexture2D.LoadImage(fileData);    

                return returnTexture2D;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[StreamingAssetsLoader] Load failed: {ex.GetType().Name} - {ex.Message}");
                return null;
            }
        }

        public async Task<Texture2D[]> LoadIllustTexture2D(string[] detailPath)
        {
            Task<Texture2D>[] resultTexture2Ds = new Task<Texture2D>[detailPath.Length];

            for (int i = 0; i < detailPath.Length; ++i)
                resultTexture2Ds[i] = this.LoadIllustTexture2D(detailPath[i]); // await 안 함

            return await Task.WhenAll(resultTexture2Ds); // 모든 비동기 작업이 끝날 때까지 기다림
        }
    }
}