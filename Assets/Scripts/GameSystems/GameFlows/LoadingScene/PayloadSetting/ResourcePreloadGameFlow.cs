using UnityEngine;

using GameSystems.DTOs;
using GameSystems.Models;
using GameSystems.UnityServices;
using GameSystems.PlainServices;

namespace GameSystems.GameFlows.LoadingScene
{
    public interface IResourcePreloadGameFlow
    {
        public void PreloadResources();
    }

    public class ResourcePreloadGameFlow : MonoBehaviour, IGameFlow, IResourcePreloadGameFlow
    {
        private RuntimeUserDataModel RuntimeUserDataModel;

        // Payload�� ���� Ư�� Scenario ���ҽ� ��ġ�� �ľ�.
        private IResourcesPathResolver ResourcesPathResolver;
        private IJsonReadAndWriteService JsonReadAndWriteService;

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

            this.ResourcesPathResolver = GlobalRepository.PlainServices_LazyReferenceRepository.GetOrCreate<ResourcesPathResolver>();
            this.JsonReadAndWriteService = GlobalRepository.PlainServices_LazyReferenceRepository.GetOrCreate<JsonReadAndWriteService>();

            this.ScenePayloadService = GlobalRepository.PlainServices_LazyReferenceRepository.GetOrCreate<ScenePayloadService>();
            this.SceneService = GlobalRepository.UnityService_LazyReferenceRepository.GetOrCreate<SceneService>();
        }

        public void PreloadResources()
        {
            // Payload Get & Clear
            if (!this.ScenePayloadService.TryGetPayload<LoadingScenePayload>(out var payloadData))
            {
                Debug.Log($"Payload �����Ͱ� ����.");
                return;
            }
            // Payload�� �Ͻ����� ������ ���� ���������� �ٷ� �ʱ�ȭ �����ݴϴ�.
            this.ScenePayloadService.ClearPayload();
            
            // Loading Scene���� � �۾��� �����ϰ� � Scene���� �Ѿ�� �����մϴ�.
            switch (payloadData.LoadingScenePayloadState)
            {
                // MainStgaeScene���� LobbyScene���� ���� �κ�.
                case LoadingScenePayloadState.ToLobbyScene:

                    Debug.Log($"LoadingScenePayloadState.ToLobbyScene");


                    // �� ��ȯ
                    this.SceneService.ChangeScene("LobbyScene");
                    break;

                // LobbyScene���� MainStgaeScene���� ���� �κ�.
                // ���ο� ����, ���� �̾��ϱ�, ���� �ٽý���, ���� ���Ǽҵ�?���� �Ѿ��. ����Դϴ�.
                // ���ҽ��� �������� �ʿ��� ������, LoadingScene���� �Ѿ���� ����, ���� Model ���� ��ϵ˴ϴ�.
                case LoadingScenePayloadState.ToMainStageScene:
                    Debug.Log($"LoadingScenePayloadState.ToMainStageScene");

                    // �ʿ��� ������ Read.
                    // ������ �����Ͱ� ������, return.
//                    if (!this.ScenarioDataLoadService.TryLoadDialogueData(this.RuntimeUserDataModel, out var dialogueDataTable)) return;

                    // Payload�� ���ҽ� ���� �Ҵ�.
//                    MainStageScenePayload mainStageScenePayload = new MainStageScenePayload(dialogueDataTable);
//                    this.ScenePayloadService.SetPayload<MainStageScenePayload>(mainStageScenePayload);
                    // �� ��ȯ
                    this.SceneService.ChangeScene("MainStageScene");
                    break;

                // �ƹ��� �� �߻� ����. ����.s
                case LoadingScenePayloadState.ToCookingScene:
                    Debug.Log($"LoadingScenePayloadState.ToCookingScene");

                    // �ʿ��� ������ Read.
                    // ������ �����Ͱ� ������, return.


                    break;
                case LoadingScenePayloadState.ToCharacterScene:
                    Debug.Log($"LoadingScenePayloadState.ToCharacterScene");
                    break;
                case LoadingScenePayloadState.None:
                    Debug.Log($"LoadingScenePayloadState.None");
                    break;
                default:
                    break;
            }
        }


    }
}