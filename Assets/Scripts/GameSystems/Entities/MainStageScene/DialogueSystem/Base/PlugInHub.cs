using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.Entities.MainStageScene
{
    // PlugInHub
    // Action
    public class PlugInHub<T> where T : class
    {
        protected Dictionary<string, T> PlugIns;

        public PlugInHub()
        {
            this.PlugIns = new Dictionary<string, T>();
        }

        public void RegisterPlugIn(string key, T plugIn)
        {
            if (this.PlugIns.ContainsKey(key)) return;

            this.PlugIns.Add(key, plugIn);
        }

        public void RemovePlugIn(string key)
        {
            if (!this.PlugIns.ContainsKey(key)) return;

            this.PlugIns.Remove(key);
        }
    }
}