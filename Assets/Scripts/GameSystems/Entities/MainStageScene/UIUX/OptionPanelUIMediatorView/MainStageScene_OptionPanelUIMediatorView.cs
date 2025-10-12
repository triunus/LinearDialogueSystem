using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.Entities.MainStageScene
{
    public interface IMainStageScene_OptionPanelUIMediatorView
    {
        public void SetActive_OptionPanel(bool value);
    }


    public class MainStageScene_OptionPanelUIMediatorView : MonoBehaviour, IEntity, IMainStageScene_OptionPanelUIMediatorView
    {
        [SerializeField] private MainStageScene_OptionPanelUIActivationView MainStageScene_OptionPanelUIActivationView;

        private void Awake()
        {
            var LocalRepository = Repository.MainStageSceneRepository.Instance;

            // µî·Ï
            LocalRepository.Entity_LazyReferenceRepository.RegisterReference<MainStageScene_OptionPanelUIMediatorView>(this);
        }

        private void Start()
        {
            this.SetActive_OptionPanel(false);
        }

        public void SetActive_OptionPanel(bool value)
        {
            if (value)
                this.MainStageScene_OptionPanelUIActivationView.Show();
            else
                this.MainStageScene_OptionPanelUIActivationView.Hide();
        }
    }
}