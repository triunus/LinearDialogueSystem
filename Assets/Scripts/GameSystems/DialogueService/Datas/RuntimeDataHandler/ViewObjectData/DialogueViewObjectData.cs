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

    public class DialogueViewObjectData : IMultiPlugInHub, IDialogueViewObjectData
    {
        private Dictionary<string, GameObject> ViewObjects;

        private MultiPlugInHub MultiPlugInHub;

        public DialogueViewObjectData()
        {
            this.MultiPlugInHub = new();
            this.ViewObjects = new();
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