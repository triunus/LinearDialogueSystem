using System;
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
            this.PlugIns = new();
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


    public interface IMultiPlugInHub
    {
        public void Register<T>(string key, T plugIn) where T : class;
        public void Remove<T>(string key) where T : class;
        public bool TryGet<T>(string key, out T plugIn) where T : class;
        public bool TryGetAll<T>(out T[] plugIns) where T : class;
    }
    public class MultiPlugInHub : IMultiPlugInHub
    {
        private Dictionary<Type, object> PlugInHubs;

        public MultiPlugInHub()
        {
            this.PlugInHubs = new();
        }

        public void Register<T>(string key, T plugIn) where T : class
            => GetHub<T>().RegisterPlugIn(key, plugIn);

        public void Remove<T>(string key) where T : class
            => GetHub<T>().RemovePlugIn(key);

        public bool TryGet<T>(string key, out T plugIn) where T : class
            => GetHub<T>().TryGetPlugIn(key, out plugIn);

        public bool TryGetAll<T>(out T[] plugIns) where T : class
            => GetHub<T>().TryGetPlugIns(out plugIns);

        private PlugInHub<T> GetHub<T>() where T : class
        {
            var key = typeof(T);
            if (!PlugInHubs.TryGetValue(key, out var hub))
            {
                hub = new PlugInHub<T>();
                PlugInHubs[key] = hub;
            }

            return (PlugInHub<T>)hub;
        }
    }
}