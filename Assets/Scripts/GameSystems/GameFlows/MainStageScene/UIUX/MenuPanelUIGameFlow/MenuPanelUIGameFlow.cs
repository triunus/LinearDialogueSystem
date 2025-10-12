using UnityEngine;

using GameSystems.Entities.MainStageScene;

namespace GameSystems.GameFlows.MainStageScene
{
    public  interface IMenuPanelUIGameFlow
    {
        public void ActivateMenuPanel();
        public void DisActivateMenuPanel();
    }

    public class MenuPanelUIGameFlow : MonoBehaviour, IGameFlow, IMenuPanelUIGameFlow
    {
        private IMenuPanelUIMediatorView MenuPanelUIMediatorView;

        private void Awake()
        {
            var LocalRepository = Repository.MainStageSceneRepository.Instance;

            // 등록
            LocalRepository.GameFlow_LazyReferenceRepository.RegisterReference<MenuPanelUIGameFlow>(this);

            // 참조
            var entityRepository = LocalRepository.Entity_LazyReferenceRepository;

            this.MenuPanelUIMediatorView =
                entityRepository.GetOrWaitReference<MenuPanelUIMediatorView>(x => this.MenuPanelUIMediatorView = x);
        }

        public void ActivateMenuPanel()
        {
            // 활성화, 입력 통제, 정지
            this.MenuPanelUIMediatorView.SetActive_MenuPanel(true);
        }

        public void DisActivateMenuPanel()
        {
            this.MenuPanelUIMediatorView.SetActive_MenuPanel(false);
        }
    }
}