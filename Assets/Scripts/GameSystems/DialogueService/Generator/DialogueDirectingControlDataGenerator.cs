using UnityEngine;

using GameSystems.DialogueDirectingService.PlainServices;
using GameSystems.DialogueDirectingService.Datas;

namespace GameSystems.DialogueDirectingService.Generator
{
    // 연출 Index에 대응되는 연출 데이터 테이블이 기록된 Json이 있음.
    // 연출 Index에 대응되는 Json이 모여있는 SO가 있음.

    // index를 입력받고, SO에서 해당 Index에 대응되는 JsonFile을 가져와서 파싱하여 등록하면 됨.
    public class DialogueDirectingControlDataGenerator
    {
        private IDialogueDirectingJsonDataSO DialogueDirectingDataGroupSO;

        private IDialogueDirectingContentRepository DialogueDirectingControlData;

        private string dialogueDirectingJsonDataSOPath = "ScriptableObject/DialogueDirectingService/DialogueDirectingJsonDataSO";

        public DialogueDirectingControlDataGenerator(IDialogueDirectingContentRepository dialogueDirectingControlData)
        {
            this.DialogueDirectingControlData = dialogueDirectingControlData;

            this.SetScriptableObject();
        }

        private void SetScriptableObject()
        {
            ScriptableObjectLoader ScriptableObjectLoader = new();

            // 해당 연출 Index에 대응되는 필요 리소스정보들이 담긴 SO를 가져오지 못한 경우 리턴.
            if (!ScriptableObjectLoader.TryGetLoadScriptableObject<DialogueDirectingJsonDataSO>(this.dialogueDirectingJsonDataSOPath, out var dialogueDirectingJsonDataSO))
            {
                Debug.Log($"해당 연출 Index에 대응되는 필요 리소스정보들이 담긴 SO를 가져오지 못함.");
                return;
            }
            this.DialogueDirectingDataGroupSO = dialogueDirectingJsonDataSO;
        }

        public void SetDialogueResoruce(int dialogueIndex)
        {
            JsonFileConverter JsonFileConverter = new();

            // 해당 연출 Index에 대응되는 필요 리소스정보 Json 파일을 가져오지 못한 경우 리턴.
            if (!this.DialogueDirectingDataGroupSO.TryGetDialogueDirectingDatas(dialogueIndex, out var directingJsonData))
            {
                Debug.Log($"해당 연출 Index에 대응되는 필요 리소스정보 Json 파일을 가져오지 못함.");
                return;
            }

            // 가져온 Json 파일을 Data로 파싱.
            if (!JsonFileConverter.TryParseJsonToData<DialogueDirectingDataGroup>(directingJsonData, out var dialogueDirectingDataGroup))
            {
                Debug.LogError("jsonFile이 비어있습니다.");
                return;
            }

            this.DialogueDirectingControlData.RegisterDialogueDirectingData(dialogueDirectingDataGroup);
        }
    }
}