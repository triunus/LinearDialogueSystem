using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.OutputLayer.DialogueDirectingSystem
{
    public interface IBaseCanvasObjectAdapter
    {
        public void SetParent(string parentObjectKey, RectTransform childObjectRectTransform);
    }

    public class BaseCanvasObjectAdapter : MonoBehaviour, IBaseCanvasObjectAdapter
    {
        [SerializeField] private List<CanvasObjectParentTransformData> CanvasObjectParentTransformDatas;

        public void SetParent(string parentObjectKey, RectTransform childObjectRectTransform)
        {
            var data = this.CanvasObjectParentTransformDatas.Find(x => x.ParentObjectKey == parentObjectKey);

            if (childObjectRectTransform == null || data == default)
                Debug.Log($"childObject가 비정상 값이던가, {parentObjectKey}에 해당하는 Data가 없습니다.");
            else
                childObjectRectTransform.SetParent(data.ParentRectTransform);
        }
    }

    [Serializable]
    public class CanvasObjectParentTransformData
    {
        [SerializeField] private string parentObjectKey;
        [SerializeField] private RectTransform parentRectTransform;

        public string ParentObjectKey { get => parentObjectKey; }
        public Transform ParentRectTransform { get => parentRectTransform; }
    }
}