using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.InputControllers.MainStageScene
{
    public class MainStageScene_InputControllerManager : MonoBehaviour
    {
        public MainStageSceneInputState MainStageSceneInputState { get; set; }

        private void Awake()
        {
            MainStageSceneInputState = MainStageSceneInputState.DialogueSystem;


        }
    }

    public enum MainStageSceneInputState
    {
        DialogueSystem,
        TutorialSystem,
        ResultSystem
    }
}
