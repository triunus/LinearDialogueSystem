using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.DTOs
{
    public class AutoDialogueDirectingData
    {
        public Coroutine AutoCoroutine { get; set; }

        public float AutoWaitDuration { get; set; }
        public float CurrentWaitDuration { get; set; }

        public bool IsRequestEnd { get; set; }

        public AutoDialogueDirectingData(float duration)
        {
            this.AutoWaitDuration = duration;
        }

        public void Reset()
        {
            this.AutoCoroutine = null;
            this.CurrentWaitDuration = 0f;
            this.IsRequestEnd = false;
        }
    }

    public class DialogueDirectingCoroutineControlData
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
