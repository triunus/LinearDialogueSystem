using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.DTOs
{
    public class DirectingCoroutineControlData
    {
        public int DirectingIndex { get; set; }

        public Coroutine BehaviourCoroutine { get; set; }
        public Coroutine ControlCoroutine { get; set; }

        public BehaviourToken BehaviourToken { get; set; }

        public bool IsOperationEnd()
        {
            if (this.ControlCoroutine == null && this.BehaviourCoroutine == null) return true;
            else return false;
        }
    }

    public class BehaviourToken
    {
        public BehaviourToken(bool isRequestEnd)
        {
            IsRequestEnd = isRequestEnd;
        }

        public bool IsRequestEnd { get; set; }
    }
}
