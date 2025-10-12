using UnityEngine;

using GameSystems.Models;
using GameSystems.DTOs;
using GameSystems.PlainServices;
using GameSystems.UnityServices;

namespace GameSystems.GameFlows.MainStageScene
{
    public interface IMainStageToLobby_SceneConvertGameFlow
    {
        public void ConvertMainStageToLobby();
    }

    public class MainStageToLobby_SceneConvertGameFlow : MonoBehaviour, IGameFlow, IMainStageToLobby_SceneConvertGameFlow
    {
        private RuntimeUserDataModel RuntimeUserDataModel;

        private ISaveAndLoadService SaveAndLoadService;
        // Payload service
        private IScenePayloadService ScenePayloadService;

        private ISceneService SceneService;

        private void Awake()
        {
            var GlobalRepository = Repository.GlobalSceneRepository.Instance;
            var LocalRepository = Repository.MainStageSceneRepository.Instance;

            // ���
            LocalRepository.GameFlow_LazyReferenceRepository.RegisterReference<MainStageToLobby_SceneConvertGameFlow>(this);

            // ����
            this.RuntimeUserDataModel = GlobalRepository.Model_LazyReferenceRepository.GetOrCreate<RuntimeUserDataModel>();

            this.SaveAndLoadService = GlobalRepository.PlainServices_LazyReferenceRepository.GetOrCreate<SaveAndLoadService>();
            this.ScenePayloadService = GlobalRepository.PlainServices_LazyReferenceRepository.GetOrCreate<ScenePayloadService>();
            this.SceneService = GlobalRepository.UnityService_LazyReferenceRepository.GetOrCreate<SceneService>();
        }

        public void ConvertMainStageToLobby()
        {
            Debug.Log($"ConvertMainStageToLobby");

            // ���� ���� ����.
            this.SaveAndLoadService.Save(this.RuntimeUserDataModel);

            EmptyScenePayload emptyScenePayload = new(EmptyScenePayloadState.ToLobbyScene);
            ScenePayloadService.SetPayload<EmptyScenePayload>(emptyScenePayload);

            // �� ��ȯ.
            this.SceneService.ChangeScene("LoadingScene");
        }
    }
}