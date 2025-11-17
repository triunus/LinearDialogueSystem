using UnityEngine;

namespace GameSystems.InfrastructureLayer.DialogueDirectingSystem
{
    public interface IRuntimeBaseObjectDB
    {
        public GameObject BaseWorldObject { get; set; }
        public GameObject BaseCanvasObject { get; set; }
    }

    public class RuntimeBaseObjectDB : IRuntimeBaseObjectDB
    {
        private GameObject _BaseWorldObject;
        private GameObject _BaseCanvasObject;

        public GameObject BaseWorldObject { get => _BaseWorldObject; set => _BaseWorldObject = value; }
        public GameObject BaseCanvasObject { get => _BaseCanvasObject; set => _BaseCanvasObject = value; }
    }
}