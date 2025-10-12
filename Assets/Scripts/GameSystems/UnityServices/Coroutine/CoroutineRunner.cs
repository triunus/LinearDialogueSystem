using System.Collections;
using UnityEngine;

namespace GameSystems.UnityServices
{
    public interface ICoroutineRunner
    {
        public YieldInstruction WaitSeconds(float second);
        public Coroutine Run(IEnumerator routine);
        public void Stop(Coroutine coroutine);
    }

    public class CoroutineRunner : MonoBehaviour, ICoroutineRunner, IUnityService
    {
        private void Awake()
        {
            var globalRepository = Repository.GlobalSceneRepository.Instance;

            globalRepository.UnityService_LazyReferenceRepository.RegisterReference<CoroutineRunner>(this);
        }

        public YieldInstruction WaitSeconds(float second)
        {
            return new WaitForSeconds(second);
        }

        public Coroutine Run(IEnumerator routine)
        {
            return StartCoroutine(routine);
        }

        public void Stop(Coroutine coroutine)
        {
            StopCoroutine(coroutine);
        }
    }
}