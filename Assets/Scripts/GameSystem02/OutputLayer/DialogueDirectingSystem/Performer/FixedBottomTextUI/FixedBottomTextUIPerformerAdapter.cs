using UnityEngine;

using GameSystems.InputLayer.DialogueDirectingSystem;

namespace GameSystems.OutputLayer.DialogueDirectingSystem
{
    public interface IFixedBottomTextUIPerformerAdapter
    {
        public void OperateDirectShow(int directingIndex);
        public void OperateDirectHide(int directingIndex);

        public void OperateTextDisplayPerformm(int directingIndex, string speaker, string content);

        public void StopPerformer();
    }

    public class FixedBottomTextUIPerformerAdapter : MonoBehaviour, IFixedBottomTextUIPerformerAdapter
    {
        private IDialogueDirectingRequestor dialogueDirectingRequestor;

        [SerializeField] private GameObjectActivator BottomTextUIActivator;
        [SerializeField] private TextDisplayer TextDisplayer;

        public void InitialBinding(IDialogueDirectingRequestor dialogueDirectingRequestor)
        {
            this.dialogueDirectingRequestor = dialogueDirectingRequestor;
        }

        public void OperateDirectShow(int directingIndex)
        {
            this.dialogueDirectingRequestor.RegisterPerformAction(directingIndex);
            StartCoroutine(this.BottomTextUIActivator.Show(() => this.dialogueDirectingRequestor.OnCompletedPerformAction(directingIndex)));
        }
        public void OperateDirectHide(int directingIndex)
        {
            this.dialogueDirectingRequestor.RegisterPerformAction(directingIndex);
            StartCoroutine(this.BottomTextUIActivator.Hide(() => this.dialogueDirectingRequestor.OnCompletedPerformAction(directingIndex)));
        }

        public void OperateTextDisplayPerformm(int directingIndex, string speaker, string content)
        {
            this.dialogueDirectingRequestor.RegisterPerformAction(directingIndex);
            StartCoroutine(this.TextDisplayer.TextDisplay(speaker, content,() => this.dialogueDirectingRequestor.OnCompletedPerformAction(directingIndex)));
        }

        public void StopPerformer()
        {
            this.TextDisplayer.IsRequestToStop = true;
        }
    }
}