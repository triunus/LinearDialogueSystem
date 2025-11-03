using UnityEngine;

namespace GameSystems.DialogueDirectingService.Datas
{
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
}