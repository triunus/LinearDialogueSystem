using UnityEngine;

using GameSystems.Models;
using GameSystems.PlainServices;
using GameSystems.UnityServices;
using GameSystems.DTOs;

namespace GameSystems.GameFlows.MainStageScene
{
    public interface IMainStageRestart_SceneConvertGameFlow
    {
        public void RestartMainStageScene();
    }

    public class MainStageRestart_SceneConvertGameFlow : MonoBehaviour, IGameFlow, IMainStageRestart_SceneConvertGameFlow
    {
        private IScenePayloadService ScenePayloadService;
        private ISceneService SceneService;

        private void Awake()
        {
            var GlobalRepository = Repository.GlobalSceneRepository.Instance;
            var LocalRepository = Repository.MainStageSceneRepository.Instance;

            // 등록
            LocalRepository.GameFlow_LazyReferenceRepository.RegisterReference<MainStageRestart_SceneConvertGameFlow>(this);

            // 참조
            this.ScenePayloadService = GlobalRepository.PlainServices_LazyReferenceRepository.GetOrCreate<ScenePayloadService>();
            this.SceneService = GlobalRepository.UnityService_LazyReferenceRepository.GetOrCreate<SceneService>();
        }

        public void RestartMainStageScene()
        {
            EmptyScenePayload emptyScenePayload = new(EmptyScenePayloadState.ToMainStageScene);
            ScenePayloadService.SetPayload<EmptyScenePayload>(emptyScenePayload);

            this.SceneService.ChangeScene("LoadingScene");
        }
    }
}