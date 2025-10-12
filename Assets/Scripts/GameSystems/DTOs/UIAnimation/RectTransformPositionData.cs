using System;

using UnityEngine;


namespace GameSystems.Entities.Shared
{
    [Serializable]
    public struct RectTransformPositionData
    {
        [SerializeField] private RectTransform _UIRectTransform;
        [SerializeField] private Vector3 hidePosition;
        [SerializeField] private Vector3 showPosition;
        [SerializeField] private float animationDuration;

        public RectTransform UIRectTransform { get => _UIRectTransform; }
        public Vector3 HidePosition { get => hidePosition; }
        public Vector3 ShowPosition { get => showPosition; }
        public float AnimationDuration { get => animationDuration; }
    }
}
