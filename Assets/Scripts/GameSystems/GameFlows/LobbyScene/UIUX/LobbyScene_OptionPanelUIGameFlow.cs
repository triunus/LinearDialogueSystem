using UnityEngine;

using GameSystems.Entities.LobbyScene;

namespace GameSystems.GameFlows.LobbyScene
{
    public interface ILobbyScene_OptionPanelUIGameFlow
    {
        public void ActivateOptionPanel();
        public void DisActivateOptionPanel();
    }

    public class LobbyScene_OptionPanelUIGameFlow : MonoBehaviour, IGameFlow, ILobbyScene_OptionPanelUIGameFlow
    {
        private ILobbyScene_OptionPanelUIMediatorView LobbyScene_OptionPanelUIMediatorView;

        private void Awake()
        {
            var LocalRepository = Repository.LobbySceneRepository.Instance;

            // 등록
            LocalRepository.GameFlow_LazyReferenceRepository.RegisterReference<LobbyScene_OptionPanelUIGameFlow>(this);

            // 참조
            this.LobbyScene_OptionPanelUIMediatorView =
                LocalRepository.Entity_LazyReferenceRepository.GetOrWaitReference<LobbyScene_OptionPanelUIMediatorView>(x => this.LobbyScene_OptionPanelUIMediatorView = x);
        }

        public void ActivateOptionPanel()
        {
            this.LobbyScene_OptionPanelUIMediatorView.SetActive_OptionPanel(true);
        }

        public void DisActivateOptionPanel()
        {
            this.LobbyScene_OptionPanelUIMediatorView.SetActive_OptionPanel(false);
        }
    }
}
