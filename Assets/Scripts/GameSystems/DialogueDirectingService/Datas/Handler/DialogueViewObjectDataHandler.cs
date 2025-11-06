using UnityEngine;
using Foundations.PlugInHub;
using GameSystems.DialogueDirectingService.Generator;

namespace GameSystems.DialogueDirectingService.Datas
{
    public interface IDialogueViewObjectDataHandler
    {
        public bool TryGetPlugIn<T>(string key, out T plugIn) where T : class;
        public bool TryGetPlugIn<T>(string prefabKey, string viewObjectKey, out T plugIn) where T : class;
        public bool TryGetAllPlugIn<T>(out T[] plugIns) where T : class;
    }
    // Data 참조 진입점.
    // 외부에서 특정 key 값을 갖는 ViewObject의 PlugIn 요청.
    // DialogueViewObjectData에 해당 Key에 해당하는 값 존재 시, 리턴.
    // 없을 시, Generator 한번 수행 후, 리턴.
    // Generator 수행하고도 없으면 false 리턴.
    public class DialogueViewObjectDataHandler : IDialogueViewObjectDataHandler
    {
        private IDialogueViewObjectGenerator DialogueViewObjectGenerator;

        private IMultiPlugInHub multiPlugInHub;

        public DialogueViewObjectDataHandler(IMultiPlugInHub multiPlugInHub, IDialogueViewObjectGenerator dialogueViewObjectGenerator)
        {
            this.multiPlugInHub = multiPlugInHub;
            this.DialogueViewObjectGenerator = dialogueViewObjectGenerator;
        }

        public bool TryGetAllPlugIn<T>(out T[] plugIns) where T : class
        {
            if (this.multiPlugInHub.TryGetAllPlugIn<T>(out plugIns)) return true;
            return false;
        }
        public bool TryGetPlugIn<T>(string key, out T plugIn) where T : class
        {
            // 기존 데이터에 해당 값이 있는지 확인.
            if (this.multiPlugInHub.TryGetPlugIn<T>(key, out plugIn)) return true;
            // 없는 경우, 새로 만들어서 돌려주고자 함.
            else
            {
                // 만들기 실패. ( Prpefab이 없는 경우임 )
                if (!this.DialogueViewObjectGenerator.TryGenerateViewObject(key)) return false;
                // 만들기 성공한 경우.
                else
                {
                    // 해당 PlugIn 리턴해줌.
                    if (this.multiPlugInHub.TryGetPlugIn<T>(key, out plugIn)) return true;
                    // 이런 경우는 없을 듯.
                    else return false;
                }
            }
        }
        public bool TryGetPlugIn<T>(string prefabKey, string viewObjectKey, out T plugIn) where T : class
        {
            // 기존 데이터에 해당 값이 있는지 확인.
            if (this.multiPlugInHub.TryGetPlugIn<T>(viewObjectKey, out plugIn)) return true;
            // 없는 경우, 새로 만들어서 돌려주고자 함.
            else
            {
                // 만들기 실패. ( Prpefab이 없는 경우임 )
                if (!this.DialogueViewObjectGenerator.TryGenerateViewObject(prefabKey, viewObjectKey)) return false;    
                // 만들기 성공한 경우.
                else
                {
                    // 해당 PlugIn 리턴해줌.
                    if (this.multiPlugInHub.TryGetPlugIn<T>(viewObjectKey, out plugIn)) return true;
                    // 이런 경우는 없을 듯.
                    else return false;
                }
            }
        }
    }
}