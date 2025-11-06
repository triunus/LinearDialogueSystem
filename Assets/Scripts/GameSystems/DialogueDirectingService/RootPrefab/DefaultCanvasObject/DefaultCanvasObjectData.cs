using UnityEngine;
using GameSystems.DialogueDirectingService.Datas;

namespace GameSystems.DialogueDirectingService.Datas
{
    public interface IDefaultCanvasObjectData
    {
        public string CanvasObjectKey { get; }
        public GameObject CanvasObject { get; }
    }

    public class DefaultCanvasObjectData : MonoBehaviour, IDefaultCanvasObjectData
    {
        [SerializeField] private string Key;

        public string CanvasObjectKey => this.Key;
        public GameObject CanvasObject => this.gameObject;
    }
}