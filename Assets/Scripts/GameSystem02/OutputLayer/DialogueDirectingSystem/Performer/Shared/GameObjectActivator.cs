using System;
using System.Collections;
using UnityEngine;

namespace GameSystems.OutputLayer.DialogueDirectingSystem
{
    public interface IGameObjectActivator
    {
        public IEnumerator Show(Action onCompleted = null);
        public IEnumerator Hide(Action onCompleted = null);
    }

    public class GameObjectActivator : MonoBehaviour, IGameObjectActivator
    {
        [SerializeField] private GameObject ActorRoot;

        public IEnumerator Show(Action onCompleted)
        {
            this.ActorRoot.SetActive(true);

            yield return Time.deltaTime;
            if (onCompleted != null) onCompleted.Invoke();
        }

        public IEnumerator Hide(Action onCompleted)
        {
            this.ActorRoot.SetActive(false);

            yield return Time.deltaTime;
            if (onCompleted != null) onCompleted.Invoke();
        }
    }
}