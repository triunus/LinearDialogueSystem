using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.InfrastructureLayer.DialogueDirectingSystem
{
    public interface RuntimeActorDB
    {
        public void RegisterActor(string key, GameObject value);
        public void RemoveActor(string key);
        public bool TryGetActor(string key, out GameObject value);

        public IEnumerable<KeyValuePair<string, GameObject>> GetAllActorPair();
    }

    public interface RuntimeBackGroundDB
    {
        public void RegisterBackGround(string key, GameObject value);
        public void RemoveBackGround(string key);
        public bool TryGetBackGround(string key, out GameObject value);

        public IEnumerable<KeyValuePair<string, GameObject>> GetAllBackGroundPair();
    }

    public interface RuntimeUniqueObjectDB
    {
        public void RegisterUniqueObject(string key, GameObject value);
        public void RemoveUniqueObject(string key);
        public bool TryGetUniqueObject(string key, out GameObject value);

        public IEnumerable<GameObject> GetAllUniqueObjects();
    }

    public class RuntimeObjectDB : RuntimeActorDB, RuntimeBackGroundDB, RuntimeUniqueObjectDB
    {
        private KeyValueData<string, GameObject> ActorObjects;
        private KeyValueData<string, GameObject> BackGroundObjects;
        private KeyValueData<string, GameObject> UniqueObjects;

        public RuntimeObjectDB()
        {
            this.ActorObjects = new();
            this.BackGroundObjects = new();
            this.UniqueObjects = new();
        }

        public void RegisterActor(string key, GameObject value) => this.ActorObjects.RegisterValue(key, value);
        public void RemoveActor(string key) => this.ActorObjects.RemoveValue(key);
        public bool TryGetActor(string key, out GameObject value) => this.ActorObjects.TryGetValue(key, out value);
        public IEnumerable<KeyValuePair<string, GameObject>> GetAllActorPair() => this.ActorObjects.GetAllPairs;

        public void RegisterBackGround(string key, GameObject value) => this.BackGroundObjects.RegisterValue(key, value);
        public void RemoveBackGround(string key) => this.BackGroundObjects.RemoveValue(key);
        public bool TryGetBackGround(string key, out GameObject value) => this.BackGroundObjects.TryGetValue(key, out value);
        public IEnumerable<KeyValuePair<string, GameObject>> GetAllBackGroundPair() => this.BackGroundObjects.GetAllPairs;

        public void RegisterUniqueObject(string key, GameObject value) => this.UniqueObjects.RegisterValue(key, value);
        public void RemoveUniqueObject(string key) => this.UniqueObjects.RemoveValue(key);
        public bool TryGetUniqueObject(string key, out GameObject value) => this.UniqueObjects.TryGetValue(key, out value);
        public IEnumerable<GameObject> GetAllUniqueObjects() => this.UniqueObjects.GetAllValues;
    }
}