namespace GameSystems.DialogueDirectingService.Datas
{
    public interface IDialogueDirectingContentRepository
    {
        public void RegisterDialogueDirectingData(DialogueDirectingDataGroup dialogueDirectingDataGroup);
        public void RemoveDialogueDirectingData();
        public bool TryGetDialogueDirectingData(int nextDirectIndex, out DialogueDirectingData dialogueDirectingData);

//        public DialogueDirectingCoroutineControlData TextDirectingCoroutineControlData { get; }
    }

    public interface IDialogueDirectingControlData
    {

    }

    public class DialogueDirectingControlData : IDialogueDirectingContentRepository, IDialogueDirectingControlData
    {
        // 연출 데이터 테이블.
        private DialogueDirectingDataGroup DialogueDirectingDataGroup;


/*        // Text 출력 코루틴 제어를 위한 데이터.
        private DialogueDirectingCoroutineControlData _TextDirectingCoroutineControlData;
        // CanvasUIUX, BackGround, Actor의 Action 코루틴 제어를 위한 데이터.
        private List<DialogueDirectingCoroutineControlData> ActionDirectingCoroutineControlDatas = new();
        // 연속적인 연출 수행을 위해, 마지막으로 수행한 연출 데이터의 내용을 기록해 놓습니다.
        private DialogueDirectingData LastDirectingData = new();

        // 자동 재생 기능 코루틴 제어 데이터.
        private AutoDialogueDirectingData AutoPlayDirectingData = new(2f);

        public DialogueDirectingControlData()
        {
            this._TextDirectingCoroutineControlData = new();
            this.ActionDirectingCoroutineControlDatas = new();
        }*/

        // DialogueDirectingDataGroup 등록/해제/Get
        public void RegisterDialogueDirectingData(DialogueDirectingDataGroup dialogueDirectingDataGroup)
        {
            this.DialogueDirectingDataGroup = dialogueDirectingDataGroup;
        }
        public void RemoveDialogueDirectingData()
        {
            this.DialogueDirectingDataGroup = null;
        }
        public bool TryGetDialogueDirectingData(int nextDirectIndex, out DialogueDirectingData dialogueDirectingData)
        {
            dialogueDirectingData = null;

            if (this.DialogueDirectingDataGroup.Count <= nextDirectIndex) return false;
            if (!this.DialogueDirectingDataGroup.TryGetDialogueDirectingData(nextDirectIndex, out dialogueDirectingData)) return false;

            return true;
        }

/*        // Text 출력 코루틴 제어를 위한 데이터.
        public DialogueDirectingCoroutineControlData TextDirectingCoroutineControlData { get; }*/
    }
}