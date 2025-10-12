using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.GameFlows.MainStageScene
{
    public class MainStage_PayloadSettingGameFlow : MonoBehaviour, IGameFlow
    {
        private void Awake()
        {
            var LocalRepository = Repository.EmptySceneRepository.Instance;
            var GlobalRepository = Repository.GlobalSceneRepository.Instance;

            // µî·Ï
            LocalRepository.GameFlow_LazyReferenceRepository.RegisterReference<MainStage_PayloadSettingGameFlow>(this);
        }
    }
}