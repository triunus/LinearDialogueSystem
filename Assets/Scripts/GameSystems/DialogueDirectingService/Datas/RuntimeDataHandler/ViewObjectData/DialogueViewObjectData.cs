using System.Collections.Generic;
using UnityEngine;

using Foundations.PlugInHub;

namespace GameSystems.DialogueDirectingService.Datas
{
    public interface IDialogueViewObjectData
    {
        public void RegisterViewObject(string key, GameObject viewObject);
        public void RemoveViewObject(string key);
        public bool IsViewObjectContained(string key);
    }

    public interface IDialogueParentObjectData
    {
        public void RegisterParentGameObjectDataHandler(string key, IParentGameObjectDataHandler parentGameObjectDataHandler);
        public void RemoveParentGameObjectDataHandler(string key);
        public bool TryGetParentGameObject(string key, out GameObject parentGameObject);
    }

    public class DialogueViewObjectData : IMultiPlugInHub, IDialogueParentObjectData, IDialogueViewObjectData
    {
        private Dictionary<string, IParentGameObjectDataHandler> DefaultParentObjectDataHandlers;

        private Dictionary<string, GameObject> ViewObjects;

        private MultiPlugInHub MultiPlugInHub;

        public DialogueViewObjectData()
        {
            this.DefaultParentObjectDataHandlers = new();

            this.MultiPlugInHub = new();
            this.ViewObjects = new();
        }

        public void RegisterParentGameObjectDataHandler(string key, IParentGameObjectDataHandler parentGameObjectDataHandler)
        {
            if (this.DefaultParentObjectDataHandlers.ContainsKey(key)) return;

            this.DefaultParentObjectDataHandlers.Add(key, parentGameObjectDataHandler);
        }
        public void RemoveParentGameObjectDataHandler(string key)
        {
            if (!this.DefaultParentObjectDataHandlers.ContainsKey(key)) return;

            this.DefaultParentObjectDataHandlers.Remove(key);
        }
        // GameObject의 부모 GameObject의 위치 지저을 위한 Default GameObject들 데이터. Set / Get
        public bool TryGetParentGameObject(string key, out GameObject parentGameObject)
        {
            foreach(var handler in this.DefaultParentObjectDataHandlers)
            {
                foreach (var data in handler.Value.ParentGameObjectDataList)
                {
                    if (data.ParentKey == key)
                    {
                        parentGameObject = data.ParentGmaeObject;
                        return true;
                    }
                }
            }

            parentGameObject = null;
            return false;
        }

        // PlugIn 관리 위임.
        public void RegisterPlugIn<T>(string key, T plugIn) where T : class => this.MultiPlugInHub.RegisterPlugIn<T>(key, plugIn);
        public void RemovePlugIn<T>(string key) where T : class => this.MultiPlugInHub.RemovePlugIn<T>(key);
        public bool TryGetAllPlugIn<T>(out T[] plugIns) where T : class => this.MultiPlugInHub.TryGetAllPlugIn<T>(out plugIns);
        public bool TryGetPlugIn<T>(string key, out T plugIn) where T : class => this.MultiPlugInHub.TryGetPlugIn<T>(key, out plugIn);

        // Dialogue 시스템에서 새로 생성된 Object 관리.
        public void RegisterViewObject(string key, GameObject viewObject)
        {
            if (this.ViewObjects.ContainsKey(key)) return;

            this.ViewObjects.Add(key, viewObject);
        }
        public void RemoveViewObject(string key)
        {
            if (!this.ViewObjects.ContainsKey(key)) return;

            this.ViewObjects.Remove(key);
        }
        public bool IsViewObjectContained(string key)
        {
            if (this.ViewObjects.ContainsKey(key)) return true;
            else return false;
        }
    }
}