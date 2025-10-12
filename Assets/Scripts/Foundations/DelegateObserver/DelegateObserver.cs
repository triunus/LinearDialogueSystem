using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foundations.DelegateObserver
{
    public interface IDelegateObserver_Publisher<T>
    {
        public DelegateObserver<T> Publisher { get; }
    }

    public class DelegateObserver<T>
    {
        private event Action<T> _subscribers;

        public void Subscribe(Action<T> onUpdated) => _subscribers += onUpdated;
        public void Unsubscribe(Action<T> onUpdated) => _subscribers -= onUpdated;
        public void Notify(T data) => _subscribers?.Invoke(data);
    }
}