using UnityEngine;
using UnityEngine.UI;

using GameSystems.GameFlows.MainStageScene;

namespace GameSystems.Entities.MainStageScene
{
    public interface IMenuPanelUIMediatorView
    {
        public void SetActive_MenuPanel(bool value);
    }

    // Menu Panel View의 기능 및, 연결 정보.
    public class MenuPanelUIMediatorView : MonoBehaviour, IEntity, IMenuPanelUIMediatorView
    {
        // 버튼을 통해 연결되는 GameFlow
        private IMenuPanelUIGameFlow MenuPanelUIGameFlow;       // 게임으로 돌아가기.
        private IMainStageRestart_SceneConvertGameFlow MainStageRestart_SceneConvertGameFlow;   // 재시작
        private IOptionPanelUIGameFlow OptionPanelUIGameFlow;
        private IMainStageToLobby_SceneConvertGameFlow MainStageToLobby_SceneConvertGameFlow;   // 로비로 돌아가기.
        private IMainStageScene_ExitGameFlow MainStageScene_ExitGameFlow;       // 게임 나가기.

        [SerializeField] private MenuPanelUIActivationView MenuPanelUIActivationView;

        [SerializeField] private Button ReturnToGameButton;
        [SerializeField] private Button RestartButton;
        [SerializeField] private Button OptionButton;
        [SerializeField] private Button ConvertToLobbyButton;
        [SerializeField] private Button ExitGameButton;

        private void Awake()
        {
            var LocalRepository = Repository.MainStageSceneRepository.Instance;

            // 등록
            LocalRepository.Entity_LazyReferenceRepository.RegisterReference<MenuPanelUIMediatorView>(this);

            // 버튼 연결을 위한 참조
            var GameFlowRepository = LocalRepository.GameFlow_LazyReferenceRepository;

            this.MenuPanelUIGameFlow =
                GameFlowRepository.GetOrWaitReference<MenuPanelUIGameFlow>(x => this.MenuPanelUIGameFlow = x);
            this.MainStageRestart_SceneConvertGameFlow =
                GameFlowRepository.GetOrWaitReference<MainStageRestart_SceneConvertGameFlow>(x => this.MainStageRestart_SceneConvertGameFlow = x);
            this.OptionPanelUIGameFlow =
                GameFlowRepository.GetOrWaitReference<OptionPanelUIGameFlow>(x => this.OptionPanelUIGameFlow = x);
            this.MainStageToLobby_SceneConvertGameFlow =
                GameFlowRepository.GetOrWaitReference<MainStageToLobby_SceneConvertGameFlow>(x => this.MainStageToLobby_SceneConvertGameFlow = x);
            this.MainStageScene_ExitGameFlow =
                GameFlowRepository.GetOrWaitReference<MainStageScene_ExitGameFlow>(x => this.MainStageScene_ExitGameFlow = x);


            // 버튼 연결.
            this.ReturnToGameButton.onClick.AddListener(this.OnClicked_ReturnToGameButton);
            this.RestartButton.onClick.AddListener(this.OnClicked_RestartButton);
            this.OptionButton.onClick.AddListener(this.OnClicked_OptionButton);
            this.ConvertToLobbyButton.onClick.AddListener(this.OnClicked_ConvertToLobbyButton);
            this.ExitGameButton.onClick.AddListener(this.OnClicked_ExitGameButton);
        }

        private void Start()
        {
            this.SetActive_MenuPanel(false);
        }

        public void SetActive_MenuPanel(bool value)
        {
            if (value)
                this.MenuPanelUIActivationView.Show();
            else
                this.MenuPanelUIActivationView.Hide();
        }


        public void OnClicked_ReturnToGameButton()
        {
            this.MenuPanelUIGameFlow.DisActivateMenuPanel();
        }

        public void OnClicked_RestartButton()
        {
            this.MainStageRestart_SceneConvertGameFlow.RestartMainStageScene();
        }

        public void OnClicked_OptionButton()
        {
            this.OptionPanelUIGameFlow.ActivateOptionPanel();
        }

        public void OnClicked_ConvertToLobbyButton()
        {
            this.MainStageToLobby_SceneConvertGameFlow.ConvertMainStageToLobby();
        }

        public void OnClicked_ExitGameButton()
        {
            this.MainStageScene_ExitGameFlow.OperateExitGame();
        }
    }
}