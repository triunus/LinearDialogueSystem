using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.InfrastructureLayer.DialogueDirectingSystem
{
    public interface IRuntimeActorDB
    {
        public void RegisterActor(string key, GameObject value);
        public void RemoveActor(string key);
        public bool TryGetActor(string key, out GameObject value);

        public IEnumerable<GameObject> GetAllActors();
        public IEnumerable<KeyValuePair<string, GameObject>> GetAllActorPair();
    }

    public interface IRuntimeBackGroundDB
    {
        public void RegisterBackGround(string key, GameObject value);
        public void RemoveBackGround(string key);
        public bool TryGetBackGround(string key, out GameObject value);

        public IEnumerable<GameObject> GetAllBackGrounds();
        public IEnumerable<KeyValuePair<string, GameObject>> GetAllBackGroundPair();
    }

    // 1개만 존재한다는 의미로 Unique라고 명칭했습니다.
    public interface IRuntimeUniqueObjectDB
    {
        public GameObject CutSceneObject { get; set; }
        public GameObject FixedBottomTextUIObject { get; set; }
    }

    public class RuntimeObjectDB : IRuntimeActorDB, IRuntimeBackGroundDB, IRuntimeUniqueObjectDB
    {
        private KeyValueData<string, GameObject> ActorObjects;
        private KeyValueData<string, GameObject> BackGroundObjects;
        private GameObject _CutSceneObject;
        private GameObject _FixedBottomTextUIObject;

        public RuntimeObjectDB()
        {
            this.ActorObjects = new();
            this.BackGroundObjects = new();
        }

        public void RegisterActor(string key, GameObject value) => this.ActorObjects.RegisterValue(key, value);
        public void RemoveActor(string key) => this.ActorObjects.RemoveValue(key);
        public bool TryGetActor(string key, out GameObject value) => this.ActorObjects.TryGetValue(key, out value);
        public IEnumerable<GameObject> GetAllActors() => this.ActorObjects.GetAllValues;
        public IEnumerable<KeyValuePair<string, GameObject>> GetAllActorPair() => this.ActorObjects.GetAllPairs;

        public void RegisterBackGround(string key, GameObject value) => this.BackGroundObjects.RegisterValue(key, value);
        public void RemoveBackGround(string key) => this.BackGroundObjects.RemoveValue(key);
        public bool TryGetBackGround(string key, out GameObject value) => this.BackGroundObjects.TryGetValue(key, out value);
        public IEnumerable<GameObject> GetAllBackGrounds() => this.BackGroundObjects.GetAllValues;
        public IEnumerable<KeyValuePair<string, GameObject>> GetAllBackGroundPair() => this.BackGroundObjects.GetAllPairs;

        public GameObject CutSceneObject { get => _CutSceneObject; set => _CutSceneObject = value; }
        public GameObject FixedBottomTextUIObject { get => _FixedBottomTextUIObject; set => _FixedBottomTextUIObject = value; }

    }
}