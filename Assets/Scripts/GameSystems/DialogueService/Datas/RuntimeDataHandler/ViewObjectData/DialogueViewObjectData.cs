using System.Collections.Generic;
using UnityEngine;

using Foundations.PlugInHub;

namespace GameSystems.DialogueDirectingService.Datas
{
    public interface IDialogueViewObjectData
    {
        public void RegisterViewObject(string key, GameObject viewObject);
        public void RemoveViewObject(string key);
        public bool TryGetViewObject(string key, out GameObject viewObject);
    }

    public class DialogueViewObjectData : MonoBehaviour, IMultiPlugInHub, IDialogueViewObjectData
    {
        private MultiPlugInHub MultiPlugInHub;
        private Dictionary<string, GameObject> ViewObjects;

        public DialogueViewObjectData()
        {
            this.MultiPlugInHub = new();
            this.ViewObjects = new();
        }

        // T 위임된 등록/해제/Get 기능
        public void RegisterPlugIn<T>(string key, T plugIn) where T : class => this.MultiPlugInHub.RegisterPlugIn<T>(key, plugIn);
        public void RemovePlugIn<T>(string key) where T : class => this.MultiPlugInHub.RemovePlugIn<T>(key);
        public bool TryGetPlugIn<T>(string key, out T plugIn) where T : class => this.MultiPlugInHub.TryGetPlugIn<T>(key, out plugIn);
        public bool TryGetAllPlugIn<T>(out T[] plugIns) where T : class => this.MultiPlugInHub.TryGetAllPlugIn<T>(out plugIns);

        // DialogueViewObjects 등록/해제
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
        public bool TryGetViewObject(string key, out GameObject viewObject)
        {
            viewObject = null;
            if (!this.ViewObjects.ContainsKey(key)) return false;

            viewObject = this.ViewObjects[key];
            return true;
        }
    }
}