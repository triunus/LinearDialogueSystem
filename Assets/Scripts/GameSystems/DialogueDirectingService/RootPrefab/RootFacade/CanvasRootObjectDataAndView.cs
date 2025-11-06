using System.Collections.Generic;
using UnityEngine;

using GameSystems.DialogueDirectingService.Datas;
using Foundations.PlugInHub;
using GameSystems.DialogueDirectingService.PlainServices;

namespace GameSystems.DialogueDirectingService.Views
{

    public class CanvasRootObjectDataAndView : MonoBehaviour, IParentGameObjectDataHandler, IActivation, IDialogueViewBinding
    {
        [SerializeField] private string _ParentGameObjectDataHandlerKey;
        // 한번에 동작하는 CanvasObject 객체.
        [SerializeField] private List<IDefaultCanvasObjectData> DefaultCanvasObjects = new();
        // 부모 GameObject 목록.
        [SerializeField] private List<IParentGameObjectData> ParentGameObjects = new();

        private void Awake()
        {
            this.DefaultCanvasObjects = new();
            this.ParentGameObjects = new();

            IDefaultCanvasObjectData[] defaultCanvasObjects = GetComponentsInChildren<IDefaultCanvasObjectData>(includeInactive: true);
            // 리스트에 등록
            foreach (var data in defaultCanvasObjects)
            {
                this.DefaultCanvasObjects.Add(data);
            }

            IParentGameObjectData[] parentObjects = GetComponentsInChildren<IParentGameObjectData>(includeInactive: true);
            // 리스트에 등록
            foreach (var data in parentObjects)
            {
                this.ParentGameObjects.Add(data);
            }

            this.Hide();
        }

        public string ParentGameObjectDataHandlerKey => this._ParentGameObjectDataHandlerKey;

        public void InitialBinding(string key, IMultiPlugInHub multiPlugInHub, IFadeInAndOutService fadeInAndOutService = null)
        {
            multiPlugInHub.RegisterPlugIn<IActivation>(key, this);
        }

        public void Show()
        {
            foreach (var data in this.DefaultCanvasObjects)
            {
                data.CanvasObject.SetActive(true);
            }
        }
        public void Hide()
        {
            foreach (var data in this.DefaultCanvasObjects)
            {
                data.CanvasObject.SetActive(false);
            }
        }

        public IEnumerable<IParentGameObjectData> ParentGameObjectDataList => this.ParentGameObjects;
    }
}