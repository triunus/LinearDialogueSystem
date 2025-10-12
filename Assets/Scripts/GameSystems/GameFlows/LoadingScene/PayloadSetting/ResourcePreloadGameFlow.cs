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

        // Payload�� ���� Ư�� Scenario ���ҽ� ��ġ�� �ľ�.
        private IScenarioDataLoadService ScenarioDataLoadService;

        // Payload service
        private IScenePayloadService ScenePayloadService;
        // scene service
        private ISceneService SceneService;

        private void Awake()
        {
            var LocalRepository = Repository.EmptySceneRepository.Instance;
            var GlobalRepository = Repository.GlobalSceneRepository.Instance;

            // ���
            LocalRepository.GameFlow_LazyReferenceRepository.RegisterReference<ResourcePreloadGameFlow>(this);

            // ����
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
                Debug.Log($"Payload �����Ͱ� ����.");
                return;
            }
            // Payload�� �Ͻ����� ������ ���� ���������� �ٷ� �ʱ�ȭ �����ݴϴ�.
            this.ScenePayloadService.ClearPayload();
            
            // Loading Scene���� � �۾��� �����ϰ� � Scene���� �Ѿ�� �����մϴ�.
            switch (payloadData.EmptyScenePayloadState)
            {
                // MainStgaeScene���� LobbyScene���� ���� �κ�.
                case EmptyScenePayloadState.ToLobbyScene:

                    Debug.Log($"EmptyScenePayloadState.ToLobby");


                    // �� ��ȯ
                    this.SceneService.ChangeScene("LobbyScene");
                    break;

                // LobbyScene���� MainStgaeScene���� ���� �κ�.
                // ���ο� ����, ���� �̾��ϱ�, ���� �ٽý���, ���� ���Ǽҵ�?���� �Ѿ��. ����Դϴ�.
                // ���ҽ��� �������� �ʿ��� ������, LoadingScene���� �Ѿ���� ����, ���� Model ���� ��ϵ˴ϴ�.
                case EmptyScenePayloadState.ToMainStageScene:
                    Debug.Log($"EmptyScenePayloadState.ToMainStageScene");

                    // �ʿ��� ������ Read.
                    // ������ �����Ͱ� ������, return.
                    if (!this.ScenarioDataLoadService.TryLoadDialogueData(this.RuntimeUserDataModel, out var dialogueDataTable)) return;

                    // Payload�� ���ҽ� ���� �Ҵ�.
                    MainStageScenePayload mainStageScenePayload = new MainStageScenePayload(dialogueDataTable);
                    this.ScenePayloadService.SetPayload<MainStageScenePayload>(mainStageScenePayload);
                    // �� ��ȯ
                    this.SceneService.ChangeScene("MainStageScene");
                    break;

                // �ƹ��� �� �߻� ����. ����.
                case EmptyScenePayloadState.None:
                default:
                    break;
            }
        }


    }
}