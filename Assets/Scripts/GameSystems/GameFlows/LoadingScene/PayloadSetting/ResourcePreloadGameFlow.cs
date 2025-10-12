using UnityEngine;

using GameSystems.DTOs;
using GameSystems.Models;
using GameSystems.UnityServices;
using GameSystems.PlainServices;

namespace GameSystems.GameFlows.EmptyScene
{
    public interface IResourcePreloadGameFlow
    {
        public void PreloadResources();
    }

    public class ResourcePreloadGameFlow : MonoBehaviour, IGameFlow, IResourcePreloadGameFlow
    {
        private RuntimeUserDataModel RuntimeUserDataModel;

        // Payload를 통해 특정 Scenario 리소스 위치를 파악.
        private IScenarioDataLoadService ScenarioDataLoadService;

        // Payload service
        private IScenePayloadService ScenePayloadService;
        // scene service
        private ISceneService SceneService;

        private void Awake()
        {
            var LocalRepository = Repository.EmptySceneRepository.Instance;
            var GlobalRepository = Repository.GlobalSceneRepository.Instance;

            // 등록
            LocalRepository.GameFlow_LazyReferenceRepository.RegisterReference<ResourcePreloadGameFlow>(this);

            // 참조
            this.RuntimeUserDataModel = GlobalRepository.Model_LazyReferenceRepository.GetOrCreate<RuntimeUserDataModel>();

            this.ScenarioDataLoadService = GlobalRepository.PlainServices_LazyReferenceRepository.GetOrCreate<ScenarioDialogueDataLoadService>();

            this.ScenePayloadService = GlobalRepository.PlainServices_LazyReferenceRepository.GetOrCreate<ScenePayloadService>();
            this.SceneService = GlobalRepository.UnityService_LazyReferenceRepository.GetOrCreate<SceneService>();
        }

        public void PreloadResources()
        {
            // Payload Get & Clear
            if (!this.ScenePayloadService.TryGetPayload<EmptyScenePayload>(out var payloadData))
            {
                Debug.Log($"Payload 데이터가 없음.");
                return;
            }
            // Payload는 일시적인 정보로 값을 가져왔으면 바로 초기화 시켜줍니다.
            this.ScenePayloadService.ClearPayload();
            
            // Loading Scene에서 어떤 작업을 수행하고 어떤 Scene으로 넘어갈지 결정합니다.
            switch (payloadData.EmptyScenePayloadState)
            {
                // MainStgaeScene에서 LobbyScene으로 가는 부분.
                case EmptyScenePayloadState.ToLobbyScene:

                    Debug.Log($"EmptyScenePayloadState.ToLobby");


                    // 씬 전환
                    this.SceneService.ChangeScene("LobbyScene");
                    break;

                // LobbyScene에서 MainStgaeScene으로 가는 부분.
                // 새로운 게임, 게임 이어하기, 게임 다시시작, 다음 애피소드?으로 넘어가기. 기능입니다.
                // 리소스를 가져오는 필요한 값들은, LoadingScene으로 넘어오기 전에, 전역 Model 값에 등록됩니다.
                case EmptyScenePayloadState.ToMainStageScene:
                    Debug.Log($"EmptyScenePayloadState.ToMainStageScene");

                    // 필요한 데이터 Read.
                    // 가져올 데이터가 없으면, return.
                    if (!this.ScenarioDataLoadService.TryLoadDialogueData(this.RuntimeUserDataModel, out var dialogueDataTable)) return;

                    // Payload에 리소스 정보 할당.
                    MainStageScenePayload mainStageScenePayload = new MainStageScenePayload(dialogueDataTable);
                    this.ScenePayloadService.SetPayload<MainStageScenePayload>(mainStageScenePayload);
                    // 씬 전환
                    this.SceneService.ChangeScene("MainStageScene");
                    break;

                // 아무런 일 발생 안함. 오류.
                case EmptyScenePayloadState.None:
                default:
                    break;
            }
        }


    }
}