using UnityEngine;

namespace GameSystems.Entities.MainStageScene
{
    public class MainStageScene_OptionPanelUIActivationView : MonoBehaviour, IEntity, IActivation
    {
        [SerializeField] private GameObject OptionPanel;

        public void Show()
        {
            this.OptionPanel.SetActive(true);
        }

        public void Hide()
        {
            this.OptionPanel.SetActive(false);
        }
    }
}