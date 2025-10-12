using UnityEngine;

using GameSystems.Models;
using GameSystems.PlainServices;

using GameSystems.Entities.LobbyScene;

namespace GameSystems.GameFlows.LobbyScene
{
    public interface IUserModelSettingGameFlow
    {
        public void SetUserModels();
    }

    public class UserModelSettingGameFlow : MonoBehaviour, IGameFlow, IUserModelSettingGameFlow
    {
        private RuntimeUserDataModel RuntimeUserDataModel;

        private ISaveAndLoadService SaveAndLoadSubGameFlow;

        private ILobbySceneButtonUIView LobbySceneButtonUIView;

        private void Awake()
        {
            var LocalRepository = Repository.LobbySceneRepository.Instance;
            var GlobalRepository = Repository.GlobalSceneRepository.Instance;

            // ���
            LocalRepository.GameFlow_LazyReferenceRepository.RegisterReference<UserModelSettingGameFlow>(this);

            // ����
            // Model ����.
            this.RuntimeUserDataModel = GlobalRepository.Model_LazyReferenceRepository.GetOrCreate<RuntimeUserDataModel>();

            // Service ����
            this.SaveAndLoadSubGameFlow = GlobalRepository.PlainServices_LazyReferenceRepository.GetOrCreate<SaveAndLoadService>();

            // Entity ����.
            this.LobbySceneButtonUIView = LocalRepository.Entity_LazyReferenceRepository.
                GetOrWaitReference<LobbySceneButtonUIView>(x => this.LobbySceneButtonUIView = x);
        }

        public void SetUserModels()
        {
            // Load�� �����Ͱ� �ִٸ�, �а� ���.
            if(this.SaveAndLoadSubGameFlow.TryLoad(out var saveAndLoadData))
            {
                Debug.Log($"���� ������ ����");
                this.LobbySceneButtonUIView.SetActive_ContinueButton(true);
            }
            else
            {
                Debug.Log($"���� ������ ����.");
                this.LobbySceneButtonUIView.SetActive_ContinueButton(false);
            }


        }
    }
}