using System.Collections.Generic;
using UnityEngine;

using GameSystems.InputLayer.DialogueDirectingSystem;

namespace GameSystems.OutputLayer.DialogueDirectingSystem
{
    public interface ICutSceneAdapter
    {
        public void OperateDirectShow(int directingIndex);
        public void OperateDirectHide(int directingIndex);

        public void OperateFadeIn(int directingIndex, float duration);
        public void OperateFadeOut(int directingIndex, float duration);

        public void SetCutSceneTitle(int directingIndex, string title, string subTitle);

        public void StopPerformer();
    }

    public class CutSceneAdapter : MonoBehaviour, ICutSceneAdapter
    {
        private IDialogueDirectingRequestor dialogueDirectingRequestor;

        [SerializeField] private GameObjectActivator CutSceneActivator;
        [SerializeField] private CanvasUIUXAlphaSetter CanvasUIUXAlphaSetter;
        [SerializeField] private CutSceneTitleSetter CutSceneTitleSetter;

        public void InitialBinding(IDialogueDirectingRequestor dialogueDirectingRequestor)
        {
            this.dialogueDirectingRequestor = dialogueDirectingRequestor;
        }

        public void OperateDirectShow(int directingIndex)
        {
            this.dialogueDirectingRequestor.RegisterPerformAction(directingIndex);
            StartCoroutine(this.CutSceneActivator.Show(() => this.dialogueDirectingRequestor.OnCompletedPerformAction(directingIndex)));
        }
        public void OperateDirectHide(int directingIndex)
        {
            this.dialogueDirectingRequestor.RegisterPerformAction(directingIndex);
            StartCoroutine(this.CutSceneActivator.Hide(() => this.dialogueDirectingRequestor.OnCompletedPerformAction(directingIndex)));
        }

        public void OperateFadeIn(int directingIndex, float duration)
        {
            this.dialogueDirectingRequestor.RegisterPerformAction(directingIndex);
            StartCoroutine(this.CanvasUIUXAlphaSetter.FadeIn(duration, () => this.dialogueDirectingRequestor.OnCompletedPerformAction(directingIndex)));
        }
        public void OperateFadeOut(int directingIndex, float duration)
        {
            this.dialogueDirectingRequestor.RegisterPerformAction(directingIndex);
            StartCoroutine(this.CanvasUIUXAlphaSetter.FadeOut(duration, () => this.dialogueDirectingRequestor.OnCompletedPerformAction(directingIndex)));
        }

        public void SetCutSceneTitle(int directingIndex, string title, string subTitle)
        {
            this.dialogueDirectingRequestor.RegisterPerformAction(directingIndex);
            StartCoroutine(this.CutSceneTitleSetter.SetCutSceneTitle(title, subTitle,() => this.dialogueDirectingRequestor.OnCompletedPerformAction(directingIndex)));
        }

        public void StopPerformer()
        {
            this.CanvasUIUXAlphaSetter.IsRequestToStop = true;
        }
    }
}