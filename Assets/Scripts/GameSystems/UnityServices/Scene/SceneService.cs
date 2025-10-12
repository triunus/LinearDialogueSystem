using UnityEngine;

using UnityEngine.SceneManagement;

namespace GameSystems.UnityServices
{
    public interface ISceneService
    {
        public void ChangeScene(string sceneName);
        public void LoadAdditive(string sceneName);
        public void Unload(string sceneName);
    }

    public class SceneService : ISceneService, IUnityService
    {
        // 즉시 전환
        public void ChangeScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        // 추가 씬 로드.
        public void LoadAdditive(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

        // 특정 씬 언로드.
        public void Unload(string sceneName)
        {
            if (SceneManager.GetSceneByName(sceneName).isLoaded)
                SceneManager.UnloadSceneAsync(sceneName);
        }
    }
}