    using UnityEngine;

namespace GameSystems.DialogueDirectingService.Datas
{
    public interface IParentGameObjectData
    {
        public string ParentKey { get; }
        public GameObject ParentGmaeObject { get; }
    }

    public class ParentGameObjectData : MonoBehaviour, IParentGameObjectData
    {
        [SerializeField] private string Key;

        public string ParentKey => this.Key;
        public GameObject ParentGmaeObject => this.gameObject;
    }
}