using GameSystems.InfrastructureLayer.DialogueDirectingSystem;

namespace GameSystems.DomainLayer.DialogueDirectingSystem
{
    public interface IDialogueDirectingDataAggregate
    {
        public bool TryGetDialogueDirectingData(int directingIndex, out DialogueDirectingData dialogueDirectingData);
    }

    public class DialogueDirectingDataAggregate : IDialogueDirectingDataAggregate
    {
        private IDialogueDirectingDataList CurrentDialogueDirectingDataList;

        public void UpdateDialogueDirectingDataList(IRuntimeDialogueDirectingDB runtimeDialogueDirectingDB, string key)
        {
           if(runtimeDialogueDirectingDB.TryGetDialogueDirectingDataList(key, out var dialogueDirectingDataList))
                this.CurrentDialogueDirectingDataList = dialogueDirectingDataList;
        }

        public bool TryGetDialogueDirectingData(int directingIndex, out DialogueDirectingData dialogueDirectingData)
            => this.CurrentDialogueDirectingDataList.TryGetDialogueDirectingData(directingIndex, out dialogueDirectingData);
    }
}