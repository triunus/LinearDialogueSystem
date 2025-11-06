using System.Collections.Generic;
using System.Linq;

namespace GameSystems.DialogueDirectingService.Datas
{

    public interface IDialogueDirectingControlData
    {
        public void RegisterDialogueDirectingData(DialogueDirectingDataGroup dialogueDirectingDataGroup);
        public void RemoveDialogueDirectingData();
        public bool TryGetDialogueDirectingData(int nextDirectIndex, out DialogueDirectingData dialogueDirectingData);

        public void RegisterCoroutineControlData(DialogueDirectingCoroutineControlData dialogueDirectingCoroutineControlData);
        public void RemoveCoroutineControlData(DialogueDirectingCoroutineControlData dialogueDirectingCoroutineControlData);
        public bool TryGetCoroutineControlData(int dataIndex, out DialogueDirectingCoroutineControlData coroutineControlData);
        public IEnumerable<DialogueDirectingCoroutineControlData> CoroutineControlDataList { get; }
        public bool IsCoroutinesCompleted();

        public DialogueDirectingData LastDirectingData { get; }
    }

    public class DialogueDirectingControlData : IDialogueDirectingControlData
    {
        // 연출 데이터 테이블.
        private DialogueDirectingDataGroup DialogueDirectingDataGroup;

        // CanvasUIUX, BackGround, Actor의 Action 코루틴 제어를 위한 데이터.
        private List<DialogueDirectingCoroutineControlData> DirectingCoroutineControlDatas = new();
        // 연속적인 연출 수행을 위해, 마지막으로 수행한 연출 데이터의 내용을 기록해 놓습니다.
        private DialogueDirectingData _LastDirectingData = new();

        public DialogueDirectingData LastDirectingData { get => this._LastDirectingData; }

        public DialogueDirectingControlData()
        {
            this.DirectingCoroutineControlDatas = new();
        }

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

        // 연출 제어 데이터 그룹.
        public void RegisterCoroutineControlData(DialogueDirectingCoroutineControlData dialogueDirectingCoroutineControlData)
            => this.DirectingCoroutineControlDatas.Add(dialogueDirectingCoroutineControlData);
        public void RemoveCoroutineControlData(DialogueDirectingCoroutineControlData dialogueDirectingCoroutineControlData)
            => this.DirectingCoroutineControlDatas.Remove(dialogueDirectingCoroutineControlData);
        public bool TryGetCoroutineControlData(int dataIndex, out DialogueDirectingCoroutineControlData coroutineControlData)
        {
            coroutineControlData = this.DirectingCoroutineControlDatas.FirstOrDefault(x => x.DirectingIndex == dataIndex);

            if (coroutineControlData == default) return false;
            return true;
        }
        public IEnumerable<DialogueDirectingCoroutineControlData> CoroutineControlDataList => this.DirectingCoroutineControlDatas;

        public bool IsCoroutinesCompleted()
        {
            if (this.DirectingCoroutineControlDatas.Count == 0) return true;
            return false;
        }
    }
}