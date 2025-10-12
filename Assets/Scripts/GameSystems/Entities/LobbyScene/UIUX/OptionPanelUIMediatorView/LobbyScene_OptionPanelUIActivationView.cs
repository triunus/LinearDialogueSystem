using UnityEngine;

namespace GameSystems.Entities.LobbyScene
{
    public class LobbyScene_OptionPanelUIActivationView : MonoBehaviour
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