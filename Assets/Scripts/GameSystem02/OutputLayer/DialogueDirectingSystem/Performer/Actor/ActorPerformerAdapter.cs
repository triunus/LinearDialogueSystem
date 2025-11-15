using UnityEngine;

using GameSystems.InputLayer.DialogueDirectingSystem;

namespace GameSystems.OutputLayer.DialogueDirectingSystem
{
    public interface IActorPerformerAdapter
    {
        public void OperateDirectShow(int directingIndex);
        public void OperateDirectHide(int directingIndex);

        public void OperateFadeIn(int directingIndex, float duration);
        public void OperateFadeOut(int directingIndex, float duration);
        
        public void SetPosition(int directingIndex, Vector3 position);
        public void SetMove(int directingIndex, Vector3[] positions, float[] durations);

        public void SetActorBaseSprtie(int directingIndex, string key);
        public void SetActorDetailSprite(int directingIndex, string key);

        public void StopPerformer();
    }

    public class ActorPerformerAdapter : MonoBehaviour, IActorPerformerAdapter
    {
        private IDialogueDirectingRequestor dialogueDirectingRequestor;

        [SerializeField] private GameObjectActivator ActorActivator;
        [SerializeField] private SpriteRendererAlphaSetter ActorAlphaSetter;
        [SerializeField] private ActorSpriteSetter ActorSpriteSetter;
        [SerializeField] private TransformPositioner ActorPositioner;

        public void InitialBinding(IDialogueDirectingRequestor dialogueDirectingRequestor)
        {
            this.dialogueDirectingRequestor = dialogueDirectingRequestor;
        }

        public void OperateDirectShow(int directingIndex)
        {
            this.dialogueDirectingRequestor.RegisterPerformAction(directingIndex);
            StartCoroutine(this.ActorActivator.Show(() => this.dialogueDirectingRequestor.OnCompletedPerformAction(directingIndex)));
        }
        public void OperateDirectHide(int directingIndex)
        {
            this.dialogueDirectingRequestor.RegisterPerformAction(directingIndex);
            StartCoroutine(this.ActorActivator.Hide(() => this.dialogueDirectingRequestor.OnCompletedPerformAction(directingIndex)));
        }

        public void OperateFadeIn(int directingIndex, float duration)
        {
            this.dialogueDirectingRequestor.RegisterPerformAction(directingIndex);
            StartCoroutine(this.ActorAlphaSetter.FadeIn(duration, () => this.dialogueDirectingRequestor.OnCompletedPerformAction(directingIndex)));
        }
        public void OperateFadeOut(int directingIndex, float duration)
        {
            this.dialogueDirectingRequestor.RegisterPerformAction(directingIndex);
            StartCoroutine(this.ActorAlphaSetter.FadeOut(duration, () => this.dialogueDirectingRequestor.OnCompletedPerformAction(directingIndex)));
        }

        public void SetActorBaseSprtie(int directingIndex, string key)
        {
            this.dialogueDirectingRequestor.RegisterPerformAction(directingIndex);
            StartCoroutine(this.ActorSpriteSetter.SetActorBaseSprtie(key, () => this.dialogueDirectingRequestor.OnCompletedPerformAction(directingIndex)));
        }
        public void SetActorDetailSprite(int directingIndex, string key)
        {
            this.dialogueDirectingRequestor.RegisterPerformAction(directingIndex);
            StartCoroutine(this.ActorSpriteSetter.SetActorDetailSprite(key, () => this.dialogueDirectingRequestor.OnCompletedPerformAction(directingIndex)));
        }

        public void SetPosition(int directingIndex, Vector3 position)
        {
            this.dialogueDirectingRequestor.RegisterPerformAction(directingIndex);
            StartCoroutine(this.ActorPositioner.SetPosition(position, () => this.dialogueDirectingRequestor.OnCompletedPerformAction(directingIndex)));
        }
        public void SetMove(int directingIndex, Vector3[] positions, float[] durations)
        {
            this.dialogueDirectingRequestor.RegisterPerformAction(directingIndex);
            StartCoroutine(this.ActorPositioner.OperateMoveWithFilpX(positions, durations, () => this.dialogueDirectingRequestor.OnCompletedPerformAction(directingIndex)));
        }

        public void StopPerformer()
        {
            this.ActorAlphaSetter.IsRequestToStop = true;
            this.ActorPositioner.IsRequestToStop = true;
        }
    }
}