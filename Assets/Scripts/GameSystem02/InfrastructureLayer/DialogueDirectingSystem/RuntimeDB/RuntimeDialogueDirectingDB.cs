namespace GameSystems.InfrastructureLayer.DialogueDirectingSystem
{
    public interface IRuntimeDialogueDirectingDB
    {
        public void RegisterDialogueDirectingDataList(string key, IDialogueDirectingDataList value);
        public void RemoveDialogueDirectingDataList(string key);
        public bool TryGetDialogueDirectingDataList(string key, out IDialogueDirectingDataList value);
    }

    public class RuntimeDialogueDirectingDB : IRuntimeDialogueDirectingDB
    {
        private KeyValueData<string, IDialogueDirectingDataList> DialogueDirectingDataLists;

        public RuntimeDialogueDirectingDB()
        {
            this.DialogueDirectingDataLists = new();
        }

        public void RegisterDialogueDirectingDataList(string key, IDialogueDirectingDataList value) => this.DialogueDirectingDataLists.RegisterValue(key, value);
        public void RemoveDialogueDirectingDataList(string key) => this.DialogueDirectingDataLists.RemoveValue(key);
        public bool TryGetDialogueDirectingDataList(string key, out IDialogueDirectingDataList value) => this.DialogueDirectingDataLists.TryGetValue(key, out value);
    }
}