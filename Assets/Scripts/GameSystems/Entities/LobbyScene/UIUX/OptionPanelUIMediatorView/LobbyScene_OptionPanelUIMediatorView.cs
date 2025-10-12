using UnityEngine;
using UnityEngine.UI;

using GameSystems.GameFlows.LobbyScene;

namespace GameSystems.Entities.LobbyScene
{
    public interface ILobbyScene_OptionPanelUIMediatorView
    {
        public void SetActive_OptionPanel(bool activationState);
    }

    public class LobbyScene_OptionPanelUIMediatorView : MonoBehaviour, IEntity, ILobbyScene_OptionPanelUIMediatorView
    {
        private ILobbyScene_OptionPanelUIGameFlow LobbyScene_OptionPanelUIGameFlow;

        [SerializeField] private LobbyScene_OptionPanelUIActivationView LobbyScene_OptionPanelUIActivationView;

        [SerializeField] private Button ReturnToGame;

        private void Awake()
        {
            var LocalRepository = Repository.LobbySceneRepository.Instance;
            // 등록
            LocalRepository.Entity_LazyReferenceRepository.RegisterReference<LobbyScene_OptionPanelUIMediatorView>(this);

            // 참조
            var GameFlow_LazyReferenceRepository = LocalRepository.GameFlow_LazyReferenceRepository;
            this.LobbyScene_OptionPanelUIGameFlow
                = GameFlow_LazyReferenceRepository.GetOrWaitReference<LobbyScene_OptionPanelUIGameFlow>(x => this.LobbyScene_OptionPanelUIGameFlow = x);

            this.ReturnToGame.onClick.AddListener(this.OnClicked_ReturnToGame);
        }

        private void Start()
        {
            this.LobbyScene_OptionPanelUIGameFlow.DisActivateOptionPanel();
        }

        public void SetActive_OptionPanel(bool activationState)
        {
            if (activationState)
                this.LobbyScene_OptionPanelUIActivationView.Show();
            else
                this.LobbyScene_OptionPanelUIActivationView.Hide();
        }

        public void OnClicked_ReturnToGame()
        {
            this.LobbyScene_OptionPanelUIGameFlow.DisActivateOptionPanel();
        }
    }
}