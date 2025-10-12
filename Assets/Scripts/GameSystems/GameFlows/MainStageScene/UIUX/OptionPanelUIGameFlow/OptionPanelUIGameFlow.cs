using UnityEngine;
using UnityEngine.UI;

using GameSystems.Entities.MainStageScene;

namespace GameSystems.GameFlows.MainStageScene
{
    public interface IOptionPanelUIGameFlow
    {
        public void ActivateOptionPanel();
        public void DisActivateOptionPanel();
    }

    public class OptionPanelUIGameFlow : MonoBehaviour, IGameFlow, IOptionPanelUIGameFlow
    {
        private IMainStageScene_OptionPanelUIMediatorView OptionPanelUIMediatorView;

        [SerializeField] private Button ExitOption;

        private void Awake()
        {
            var LocalRepository = Repository.MainStageSceneRepository.Instance;

            // 등록
            LocalRepository.GameFlow_LazyReferenceRepository.RegisterReference<OptionPanelUIGameFlow>(this);

            // 참조
            var entityRepository = LocalRepository.Entity_LazyReferenceRepository;

            this.OptionPanelUIMediatorView =
                entityRepository.GetOrWaitReference<MainStageScene_OptionPanelUIMediatorView>(x => this.OptionPanelUIMediatorView = x);

            this.ExitOption.onClick.AddListener(this.DisActivateOptionPanel);
        }

        public void ActivateOptionPanel()
        {
            // 일단, 활성화만.
            this.OptionPanelUIMediatorView.SetActive_OptionPanel(true);
        }

        public void DisActivateOptionPanel()
        {
            this.OptionPanelUIMediatorView.SetActive_OptionPanel(false);
        }
    }
}