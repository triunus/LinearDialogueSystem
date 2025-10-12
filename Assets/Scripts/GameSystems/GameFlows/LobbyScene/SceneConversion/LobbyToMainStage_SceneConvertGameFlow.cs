using UnityEngine;

using GameSystems.PlainServices;
using GameSystems.UnityServices;
using GameSystems.Models;
using GameSystems.DTOs;

namespace GameSystems.GameFlows.LobbyScene
{
    public interface ILobbyToMainStage_SceneConvertGameFlow
    {
        public void ConvertLobbyToMainStage_NewGame();
        public void ConvertLobbyToMainStage_Continue();
    }

    public class LobbyToMainStage_SceneConvertGameFlow : MonoBehaviour, IGameFlow, ILobbyToMainStage_SceneConvertGameFlow
    {
        private RuntimeUserDataModel RuntimeUserDataModel;

        private ISaveAndLoadService SaveAndLoadService;
        private IScenePayloadService ScenePayloadService;
        private ISceneService SceneService;

        private void Awake()
        {
            var GlobalRepository = Repository.GlobalSceneRepository.Instance;
            var LocalRepository = Repository.LobbySceneRepository.Instance;

            // 등록
            LocalRepository.GameFlow_LazyReferenceRepository.RegisterReference<LobbyToMainStage_SceneConvertGameFlow>(this);

            // 참조
            this.RuntimeUserDataModel = GlobalRepository.Model_LazyReferenceRepository.GetOrCreate<RuntimeUserDataModel>();

            this.SaveAndLoadService = GlobalRepository.PlainServices_LazyReferenceRepository.GetOrCreate<SaveAndLoadService>();
            this.ScenePayloadService = GlobalRepository.PlainServices_LazyReferenceRepository.GetOrCreate<ScenePayloadService>();
            this.SceneService = GlobalRepository.UnityService_LazyReferenceRepository.GetOrCreate<SceneService>();
        }

        public void ConvertLobbyToMainStage_NewGame()
        {
            Debug.Log($"ConvertLobbyToMainStage_NewGame");

            // 새로운 게임을 수행할 시, 런타임 내 게임 데이터를 초기값으로 할당.
            this.RuntimeUserDataModel.SetNewGame();

            EmptyScenePayload emptyScenePayload = new(EmptyScenePayloadState.ToMainStageScene);
            ScenePayloadService.SetPayload<EmptyScenePayload>(emptyScenePayload);

            this.SceneService.ChangeScene("LoadingScene");
        }

        public void ConvertLobbyToMainStage_Continue()
        {
            Debug.Log($"ConvertLobbyToMainStage_Continue");

            // 저장된 데이터가 없으면 리턴. ( '이어하기' 버튼의 활성화 비활성화를 통해 이 에러는 발생할 일이 없을 것임. )
            if (!this.SaveAndLoadService.TryLoad(out var saveAndLoadData)) return;

            // 저장된 데이터를 현재 런타임 게임 데이터에 값을 복사합니다.
            // 이후, 임시로 사용한 저장 데이터는 초기화합니다.
            this.RuntimeUserDataModel.SetLoadGame(saveAndLoadData);
            saveAndLoadData = null;

            EmptyScenePayload emptyScenePayload = new(EmptyScenePayloadState.ToMainStageScene);
            ScenePayloadService.SetPayload<EmptyScenePayload>(emptyScenePayload);

            // 씬 전환.
            this.SceneService.ChangeScene("LoadingScene");
        }
    }
}
