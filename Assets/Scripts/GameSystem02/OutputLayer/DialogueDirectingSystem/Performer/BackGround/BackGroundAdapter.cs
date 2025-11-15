using UnityEngine;

using GameSystems.InputLayer.DialogueDirectingSystem;

namespace GameSystems.OutputLayer.DialogueDirectingSystem
{
    public interface IBackGroundAdapter
    {
        public void OperateDirectShow(int directingIndex);
        public void OperateDirectHide(int directingIndex);

        public void OperateFadeIn(int directingIndex, float duration);
        public void OperateFadeOut(int directingIndex, float duration);

        public void SetPosition(int directingIndex, Vector3 position);
        public void SetMove(int directingIndex, Vector3[] positions, float[] durations);

        public void StopPerformer();
    }

    public class BackGroundAdapter : MonoBehaviour, IBackGroundAdapter
    {
        private IDialogueDirectingRequestor dialogueDirectingRequestor;

        [SerializeField] private GameObjectActivator BackGroundActivator;
        [SerializeField] private SpriteRendererAlphaSetter BackGroundAlphaSetter;
        [SerializeField] private TransformPositioner BackGroundPositioner;

        public void InitialBinding(IDialogueDirectingRequestor dialogueDirectingRequestor)
        {
            this.dialogueDirectingRequestor = dialogueDirectingRequestor;
        }

        public void OperateDirectShow(int directingIndex)
        {
            this.dialogueDirectingRequestor.RegisterPerformAction(directingIndex);
            StartCoroutine(this.BackGroundActivator.Show(() => this.dialogueDirectingRequestor.OnCompletedPerformAction(directingIndex)));
        }
        public void OperateDirectHide(int directingIndex)
        {
            this.dialogueDirectingRequestor.RegisterPerformAction(directingIndex);
            StartCoroutine(this.BackGroundActivator.Hide(() => this.dialogueDirectingRequestor.OnCompletedPerformAction(directingIndex)));
        }

        public void OperateFadeIn(int directingIndex, float duration)
        {
            this.dialogueDirectingRequestor.RegisterPerformAction(directingIndex);
            StartCoroutine(this.BackGroundAlphaSetter.FadeIn(duration, () => this.dialogueDirectingRequestor.OnCompletedPerformAction(directingIndex)));
        }
        public void OperateFadeOut(int directingIndex, float duration)
        {
            this.dialogueDirectingRequestor.RegisterPerformAction(directingIndex);
            StartCoroutine(this.BackGroundAlphaSetter.FadeOut(duration, () => this.dialogueDirectingRequestor.OnCompletedPerformAction(directingIndex)));
        }

        public void SetPosition(int directingIndex, Vector3 position)
        {
            this.dialogueDirectingRequestor.RegisterPerformAction(directingIndex);
            StartCoroutine(this.BackGroundPositioner.SetPosition(position, () => this.dialogueDirectingRequestor.OnCompletedPerformAction(directingIndex)));
        }
        public void SetMove(int directingIndex, Vector3[] positions, float[] durations)
        {
            this.dialogueDirectingRequestor.RegisterPerformAction(directingIndex);
            StartCoroutine(this.BackGroundPositioner.OperateMove(positions, durations, () => this.dialogueDirectingRequestor.OnCompletedPerformAction(directingIndex)));
        }

        public void StopPerformer()
        {
            this.BackGroundAlphaSetter.IsRequestToStop = true;
            this.BackGroundPositioner.IsRequestToStop = true;
        }
    }
}