using UnityEngine;

public interface IActivation
{
    public void Show();
    public void Hide();
}

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