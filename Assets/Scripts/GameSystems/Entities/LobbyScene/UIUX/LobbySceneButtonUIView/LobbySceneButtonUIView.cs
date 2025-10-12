using UnityEngine;
using UnityEngine.UI;

using GameSystems.GameFlows.LobbyScene;

namespace GameSystems.Entities.LobbyScene
{
    public interface ILobbySceneButtonUIView
    {
        public void SetActive_ContinueButton(bool activationState);
    }

    public class LobbySceneButtonUIView : MonoBehaviour, IEntity, ILobbySceneButtonUIView
    {
        private ILobbyToMainStage_SceneConvertGameFlow LobbyToMainStage_SceneConvertGameFlow;   // New Game, Continue
        private ILobbyScene_OptionPanelUIGameFlow OptionPanelUIGameFlow;           // Option
        private ILobbyScene_ExitGameFlow LobbyScene_ExitGameFlow;       // Exit Game

        [SerializeField] private Button NewGameButton;
        [SerializeField] private Button ContinueButton;
        [SerializeField] private Button Optionutton;
        [SerializeField] private Button ExitButton;

        private void Awake()
        {
            var GlobalRepository = Repository.GlobalSceneRepository.Instance;
            var LocalRepository = Repository.LobbySceneRepository.Instance;

            // 등록
            LocalRepository.Entity_LazyReferenceRepository.RegisterReference<LobbySceneButtonUIView>(this);

            // 참조.
            var modelRepository = GlobalRepository.Model_LazyReferenceRepository;
            var gameFlowRepository = LocalRepository.GameFlow_LazyReferenceRepository;

            // GameFlow 참조.
            this.LobbyToMainStage_SceneConvertGameFlow =
                gameFlowRepository.GetOrWaitReference<LobbyToMainStage_SceneConvertGameFlow>(x => this.LobbyToMainStage_SceneConvertGameFlow = x);
            this.OptionPanelUIGameFlow =
                gameFlowRepository.GetOrWaitReference<LobbyScene_OptionPanelUIGameFlow>(x => this.OptionPanelUIGameFlow = x);
            this.LobbyScene_ExitGameFlow =
                gameFlowRepository.GetOrWaitReference<LobbyScene_ExitGameFlow>(x => this.LobbyScene_ExitGameFlow = x);

            // 버튼 등록
            this.NewGameButton.onClick.AddListener(this.OnClicked_NewGameButton);
            this.ContinueButton.onClick.AddListener(this.OnClicked_ContinueButton);
            this.Optionutton.onClick.AddListener(this.OnClicked_OptionButton);
            this.ExitButton.onClick.AddListener(this.OnClicked_ExitButton);
        }


        public void OnClicked_NewGameButton()
        {
            this.LobbyToMainStage_SceneConvertGameFlow.ConvertLobbyToMainStage_NewGame();
        }

        public void OnClicked_ContinueButton()
        {
            this.LobbyToMainStage_SceneConvertGameFlow.ConvertLobbyToMainStage_Continue();
        }

        public void OnClicked_OptionButton()
        {
            this.OptionPanelUIGameFlow.ActivateOptionPanel();
        }

        public void OnClicked_ExitButton()
        {
            this.LobbyScene_ExitGameFlow.OperateExitGame();
        }

        public void SetActive_ContinueButton(bool activationState)
        {
            if (activationState)
                this.ContinueButton.gameObject.SetActive(true);
            else
                this.ContinueButton.gameObject.SetActive(false);
        }
    }
}