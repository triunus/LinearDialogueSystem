using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.InputLayer.DialogueDirectingSystem
{
    public interface IDialogueDirectingRequestor
    {
        public void RegisterPerformAction(int key);
        public void OnCompletedPerformAction(int key);
    }

    public class DialogueDirectingRequestor : MonoBehaviour, IDialogueDirectingRequestor
    {
        private List<int> currentPerformedOperations;

        public void InitialBinding(/* Usecase를 넘겨받음. */)
        {

        }

        public void RegisterPerformAction(int key)
        {
            if (this.currentPerformedOperations.Contains(key)) return;
            this.currentPerformedOperations.Add(key);
        }

        public void OnCompletedPerformAction(int key)
        {
            if (!this.currentPerformedOperations.Contains(key)) return;
            this.currentPerformedOperations.Remove(key);

            if(this.currentPerformedOperations.Count == 0)
            {
                // Usecase 호출.
            }
        }
    }
}