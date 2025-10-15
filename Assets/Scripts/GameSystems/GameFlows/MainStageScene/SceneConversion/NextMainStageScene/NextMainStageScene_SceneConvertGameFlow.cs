using UnityEngine;
using GameSystems.PlainServices;
using GameSystems.UnityServices;
using GameSystems.DTOs;

namespace GameSystems.GameFlows.MainStageScene
{
    public interface INextMainStageScene_SceneConvertGameFlow
    {

    }

    public class NextMainStageScene_SceneConvertGameFlow : MonoBehaviour, IGameFlow, INextMainStageScene_SceneConvertGameFlow
    {
        private IScenePayloadService ScenePayloadService;
        private ISceneService SceneService;

        private void Awake()
        {
            var GlobalRepository = Repository.GlobalSceneRepository.Instance;
            var LocalRepository = Repository.MainStageSceneRepository.Instance;

            // 등록
            LocalRepository.GameFlow_LazyReferenceRepository.RegisterReference<NextMainStageScene_SceneConvertGameFlow>(this);

            // 참조            
            this.ScenePayloadService = GlobalRepository.PlainServices_LazyReferenceRepository.GetOrCreate<ScenePayloadService>();
            this.SceneService = GlobalRepository.UnityService_LazyReferenceRepository.GetOrCreate<SceneService>();
        }

        public void NextMainStageScene()
        {
            // 다음 Story를 구분하기 위한 데이터 갱신 작업은, 결과창에서 수행할 것입니다.

            LoadingScenePayload emptyScenePayload = new(LoadingScenePayloadState.ToMainStageScene);
            ScenePayloadService.SetPayload<LoadingScenePayload>(emptyScenePayload);

            this.SceneService.ChangeScene("LoadingScene");
        }
    }
}