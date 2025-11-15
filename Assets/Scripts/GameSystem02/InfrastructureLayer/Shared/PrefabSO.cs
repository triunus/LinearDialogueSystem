using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.InfrastructureLayer.DialogueDirectingSystem
{
    public interface IPrefabSO
    {
        public bool TryGetPrefabData(string key, out GameObject prefab);
    }

    [Serializable]
    public class PrefabSO : ScriptableObject, IPrefabSO
    {
        [SerializeField] private List<PrefabData> PrefabDatas;

        public bool TryGetPrefabData(string key, out GameObject prefab)
        {
            prefab = null;
            foreach(var data in this.PrefabDatas)
            {
                if(data.Key == key)
                {
                    prefab = data.Prefab;
                    return true;
                }
            }
            return false;
        }
    }

    [Serializable]
    public class PrefabData
    {
        [SerializeField] private string _Key;
        [SerializeField] private GameObject _Prefab;

        public string Key { get => _Key; }
        public GameObject Prefab { get => _Prefab; }
    }
}