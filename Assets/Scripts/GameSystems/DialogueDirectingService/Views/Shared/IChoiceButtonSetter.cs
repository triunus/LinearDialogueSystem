using System.Collections;
using GameSystems.DialogueDirectingService.Datas;

namespace GameSystems.DialogueDirectingService.Views
{
    public interface IChoiceButtonSetter
    {
        public IEnumerator OperateChoiceButtonDisplay(string content, int nextBranchPoint, BehaviourToken behaviourToken);
        public void SetCondition(string[] needSelectButtonKeys);
        public void UpdateChoiceButtonView(string selectedButtonKey);
    }
}