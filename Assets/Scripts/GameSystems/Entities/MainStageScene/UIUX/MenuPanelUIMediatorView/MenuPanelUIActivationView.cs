using UnityEngine;

namespace GameSystems.Entities.MainStageScene
{
    public class MenuPanelUIActivationView : MonoBehaviour, IEntity, IActivation
    {
        [SerializeField] private GameObject MenuPanel;

        public void Show()
        {
            this.MenuPanel.SetActive(true);
        }

        public void Hide()
        {
            this.MenuPanel.SetActive(false);
        }
    }
}