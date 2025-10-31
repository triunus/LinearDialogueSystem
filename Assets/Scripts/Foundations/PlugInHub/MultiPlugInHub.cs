using System;
using System.Collections.Generic;

namespace Foundations.PlugInHub
{
    public interface IMultiPlugInHub
    {
        public void RegisterPlugIn<T>(string key, T plugIn) where T : class;
        public void RemovePlugIn<T>(string key) where T : class;
        public bool TryGetPlugIn<T>(string key, out T plugIn) where T : class;
        public bool TryGetAllPlugIn<T>(out T[] plugIns) where T : class;
    }

    public class MultiPlugInHub : IMultiPlugInHub
    {
        private Dictionary<Type, object> PlugInHubs;

        public MultiPlugInHub()
        {
            this.PlugInHubs = new();
        }

        public void RegisterPlugIn<T>(string key, T plugIn) where T : class
            => GetHub<T>().RegisterPlugIn(key, plugIn);

        public void RemovePlugIn<T>(string key) where T : class
            => GetHub<T>().RemovePlugIn(key);

        public bool TryGetPlugIn<T>(string key, out T plugIn) where T : class
            => GetHub<T>().TryGetPlugIn(key, out plugIn);

        public bool TryGetAllPlugIn<T>(out T[] plugIns) where T : class
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