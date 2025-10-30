using System.Collections.Generic;
using System.Linq;

namespace GameSystems.Entities.MainStageScene
{
    public interface IPlugInHub<T>
    {
        public void RegisterPlugIn(string key, T plugIn);
        public void RemovePlugIn(string key);
        public bool TryGetPlugIn(string key, out T plugIn);
        public bool TryGetPlugIns(out T[] plugIns);
    }

    public class PlugInHub<T> : IPlugInHub<T> where T : class
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

        public bool TryGetPlugIn(string key, out T plugIn)
        {
            plugIn = null;
            if (!this.PlugIns.ContainsKey(key)) return false;

            plugIn = (T)this.PlugIns[key];
            return true;
        }

        public bool TryGetPlugIns(out T[] plugIns)
        {
            if (this.PlugIns == null || this.PlugIns.Count == 0)
            {
                plugIns = default;
                return false;
            }

            plugIns = this.PlugIns.Values.ToArray();
            return true;
        }
    }
}