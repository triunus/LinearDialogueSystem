using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.OutputLayer.DialogueDirectingSystem
{
    public interface IBaseWorldObjectAdapter
    {
        public void SetParent(string parentObjectKey, Transform childObjectTransform);
    }

    public class BaseWorldObjectAdapter : MonoBehaviour, IBaseWorldObjectAdapter
    {
        [SerializeField] private List<WorldObjectParentTransformData> WorldObjectParentTransformDatas;

        public void SetParent(string parentObjectKey, Transform childObjectTransform)
        {
            var data = this.WorldObjectParentTransformDatas.Find(x => x.ParentObjectKey == parentObjectKey);

            if (childObjectTransform == null || data == default)
                Debug.Log($"childObject가 비정상 값이던가, {parentObjectKey}에 해당하는 Data가 없습니다.");
            else
                childObjectTransform.SetParent(data.ParentTransform);
        }
    }

    [Serializable]
    public class WorldObjectParentTransformData
    {
        [SerializeField] private string parentObjectKey;
        [SerializeField] private Transform parentTransform;

        public string ParentObjectKey { get => parentObjectKey; }
        public Transform ParentTransform { get => parentTransform; }
    }
}