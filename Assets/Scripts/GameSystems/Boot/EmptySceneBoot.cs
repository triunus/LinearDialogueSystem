using System.Collections;
using UnityEngine;

using GameSystems.GameFlows.EmptyScene;

namespace GameSystems.Boots
{
    public class EmptySceneBoot : MonoBehaviour
    {
        private IResourcePreloadGameFlow ResourcePreloadGameFlow;

        private void Awake()
        {
            var LocalRepository = Repository.EmptySceneRepository.Instance;

            this.ResourcePreloadGameFlow = LocalRepository.GameFlow_LazyReferenceRepository.
                GetOrWaitReference<ResourcePreloadGameFlow>(x => this.ResourcePreloadGameFlow = x);
        }

        private void Start()
        {
            this.StartCoroutine(this.Test());

/*            yield return new WaitForSeconds(0.5f);

            this.ResourcePreloadGameFlow.PreloadResources();*/
        }

        private IEnumerator Test()
        {
            yield return new WaitForSeconds(1f);
            this.ResourcePreloadGameFlow.PreloadResources();
        }
    }
}