using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using GameSystems.GameFlows.MainStageScene;

namespace GameSystems.Entities.MainStageScene
{
    public interface IMainStageSceneButtonUIView
    {

    }

    public class MainStageSceneButtonUIView : MonoBehaviour, IEntity, IMainStageSceneButtonUIView
    {
        private IMenuPanelUIGameFlow MenuPanelUIGameFlow;

        [SerializeField] private Button MenuButton;

        private void Awake()
        {
            var LocalRepository = Repository.MainStageSceneRepository.Instance;

            // ���
            LocalRepository.Entity_LazyReferenceRepository.RegisterReference<MainStageSceneButtonUIView>(this);

            // ����
            var GameFlowRepository = LocalRepository.GameFlow_LazyReferenceRepository;

            this.MenuPanelUIGameFlow =
                GameFlowRepository.GetOrWaitReference<MenuPanelUIGameFlow>(x => this.MenuPanelUIGameFlow = x);

            // ��ư����
            this.MenuButton.onClick.AddListener(this.OnClicked_ActivateMenuPanelButton);
        }

        public void OnClicked_ActivateMenuPanelButton()
        {
            this.MenuPanelUIGameFlow.ActivateMenuPanel();
        }
    }
}