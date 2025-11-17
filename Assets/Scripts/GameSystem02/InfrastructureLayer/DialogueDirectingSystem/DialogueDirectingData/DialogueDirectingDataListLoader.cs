using UnityEngine;

namespace GameSystems.InfrastructureLayer.DialogueDirectingSystem
{
    public interface IDialogueDirectingDataListLoader
    {
        public bool TryDialogueSetting(string[] dialogueIndexs);
    }


    public class DialogueDirectingDataListLoader
    {
        private IDialogueDirectingJsonDataSO dialogueDirectingJsonDataSO;

        private IRuntimeDialogueDirectingDB runtimeDialogueDirectingDB;

        private string DialogueDirectingJsonDataSOPath = "ScriptableObjects/DialogueDirectingSystem/DialogueDirectingJsonDataSO";

        public DialogueDirectingDataListLoader(IRuntimeDialogueDirectingDB runtimeDialogueDirectingDB)
        {
            this.runtimeDialogueDirectingDB = runtimeDialogueDirectingDB;

            this.LoadScriptableObject();
        }

        private void LoadScriptableObject()
        {
            this.dialogueDirectingJsonDataSO = Resources.Load<DialogueDirectingJsonDataSO>(this.DialogueDirectingJsonDataSOPath);

            // 파일 못찾으면 false 리턴.
            if (this.dialogueDirectingJsonDataSO == null)
                Debug.LogError($"DialogueDirectingJsonDataSO 불러오기 실패.");
        }

        public bool TryDialogueSetting(string[] dialogueIndexs)
        {
            if (this.dialogueDirectingJsonDataSO == null || this.runtimeDialogueDirectingDB == null)
            {
                Debug.LogError($"dialogueDirectingJsonDataSO 또는 runtimeDialogueDirectingDB null 상태.");
                return false;
            }

            foreach (string key in dialogueIndexs)
            {
                if (!this.dialogueDirectingJsonDataSO.TryGetDialogueDirectingJsonData(key, out var data))
                    Debug.LogError($"{key}에 해당되는 DialogueDirectingJsonData 등록되지 않음.");

                IDialogueDirectingDataList dataList = JsonUtility.FromJson<DialogueDirectingDataList>(data.DialogueJson.text);

                if (dataList == null)
                    Debug.LogError($"{key}에 해당되는 Json 파일을  DialogueDirectingDataList로 변환 실패");
                else
                    this.runtimeDialogueDirectingDB.RegisterDialogueDirectingDataList(key, dataList);
            }

            return true;
        }
    }

}