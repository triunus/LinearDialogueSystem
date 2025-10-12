using System;
using System.Collections.Generic;

using UnityEngine;

namespace Foundations.LazyReferenceRepository
{
    // ROT : ReferenceObjectType
    public interface ILazyReferenceRepository<ROT>
    {
        public void RegisterReference<T>(T instance) where T : class, ROT;
        public T GetOrWaitReference<T>(Action<T> callback) where T : class, ROT;
        public T GetOrCreate<T>() where T : class, ROT, new();
    }

    public class LazyReferenceRepository<ROT> : ILazyReferenceRepository<ROT>
    {
        private Dictionary<Type, object> _ObjectReferences;
        private Dictionary<Type, List<Delegate>> WaitedReferenceRequests;

        public LazyReferenceRepository()
        {
            this._ObjectReferences = new();
            this.WaitedReferenceRequests = new();
        }

        public void RegisterReference<T>(T instance) where T : class, ROT
        {
            Type key = typeof(T);

            // 아직 없었으면 생성.
            if (!this._ObjectReferences.ContainsKey(key))
                this._ObjectReferences.Add(key, instance);

            // 대기 중인 메소드를 호출하여, 참조 전달.
            if (this.WaitedReferenceRequests.TryGetValue(key, out var list))
            {
                // 순환하여 참조 전달.
                foreach (var request in list) ((Action<T>)request).Invoke(instance);
                // 해당 Key 리소스 삭제.
                this.WaitedReferenceRequests.Remove(key);
            }
        }

        public T GetOrWaitReference<T>(Action<T> callback) where T : class, ROT
        {
            Type key = typeof(T);

            // 현재 등록된 객체에 대한 참조라면 해당 객체 참조 반환.
            if (this._ObjectReferences.ContainsKey(key))
                return (T)this._ObjectReferences[key];

            // 아직 등록되지 않은 객체에 대한 참조라면, 해당 객체에 대한 참조가 등록되는 순간 호출을 요청하는 delegate 등록.
            if (!this.WaitedReferenceRequests.TryGetValue(key, out var list))
            {
                list = new List<Delegate>();
                this.WaitedReferenceRequests.Add(key, list);
            }

            list.Add(callback);
            // 비록 이곳에서는 null을 반환하지만, 차후 RegisterReference을 통해 참조를 리턴하는 경우, 알맞게 참조가 연결된다.
            return null;
        }

        // MonoBehaviour를 상속받지 않는 객체에게 사용.
        public T GetOrCreate<T>() where T : class, ROT, new()
        {
            var key = typeof(T);

            if (this._ObjectReferences.TryGetValue(key, out var existing))
                return (T)existing;

            // 생성 & 등록 (대기 콜백 처리 포함)
            var created = new T();
            RegisterReference(created);
            return created;
        }
    }
}