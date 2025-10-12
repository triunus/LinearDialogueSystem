using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameSystems.GameFlows.LobbyScene;

namespace GameSystems.Boots
{
    public class LobbySceneBoot : MonoBehaviour
    {
        private IUserModelSettingGameFlow UserModelSettingGameFlow;

        private void Awake()
        {
            // ÂüÁ¶
            var LocalRepository = Repository.LobbySceneRepository.Instance;

            var gameFlowRepository = LocalRepository.GameFlow_LazyReferenceRepository;

            this.UserModelSettingGameFlow
                = gameFlowRepository.GetOrWaitReference<UserModelSettingGameFlow>(x => this.UserModelSettingGameFlow = x);
        }

        private void Start()
        {
            this.UserModelSettingGameFlow.SetUserModels();
        }
    }
}