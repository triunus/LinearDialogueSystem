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

            // 등록
            LocalRepository.GameFlow_LazyReferenceRepository.RegisterReference<UserModelSettingGameFlow>(this);

            // 참조
            // Model 참조.
            this.RuntimeUserDataModel = GlobalRepository.Model_LazyReferenceRepository.GetOrCreate<RuntimeUserDataModel>();

            // Service 참조
            this.SaveAndLoadSubGameFlow = GlobalRepository.PlainServices_LazyReferenceRepository.GetOrCreate<SaveAndLoadService>();

            // Entity 참조.
            this.LobbySceneButtonUIView = LocalRepository.Entity_LazyReferenceRepository.
                GetOrWaitReference<LobbySceneButtonUIView>(x => this.LobbySceneButtonUIView = x);
        }

        public void SetUserModels()
        {
            // Load할 데이터가 있다면, 읽고 등록.
            if(this.SaveAndLoadSubGameFlow.TryLoad(out var saveAndLoadData))
            {
                Debug.Log($"저장 데이터 있음");
                this.LobbySceneButtonUIView.SetActive_ContinueButton(true);
            }
            else
            {
                Debug.Log($"저장 데이터 없음.");
                this.LobbySceneButtonUIView.SetActive_ContinueButton(false);
            }


        }
    }
}