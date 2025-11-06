using UnityEngine;

using Foundations.PlugInHub;
using GameSystems.DialogueDirectingService.PlainServices;
using GameSystems.DialogueDirectingService.Datas;

namespace GameSystems.DialogueDirectingService.Generator
{
    public interface IDialogueDefaultObjectGenerator
    {
        public void GenerateAllDefaultGameObject();
    }

    public class DialogueDefaultObjectGenerator
    {
        private IDialogueDirectingPrefabDataSO defaultPrefabDataSO;
        private string defaultPrefabDataSOPath = "ScriptableObjects/DialogueDirectingService/DefaultPrefabSO";

        private IMultiPlugInHub MultiPlugInHub;
        private IDialogueParentObjectData DialogueParentObjectData;
        private IDialogueViewObjectData DialogueViewObjectData;

        public DialogueDefaultObjectGenerator(IMultiPlugInHub multiPlugInHub, IDialogueParentObjectData dialogueParentObjectData, IDialogueViewObjectData dialogueViewObjectData)
        {
            this.MultiPlugInHub = multiPlugInHub;
            this.DialogueParentObjectData = dialogueParentObjectData;
            this.DialogueViewObjectData = dialogueViewObjectData;

            this.SetScriptableObject();
        }
        // Generate를 수행할 SO를 미리 셋팅.
        private void SetScriptableObject()
        {
            ScriptableObjectLoader ScriptableObjectLoader = new();

            // 연출 서비스에서 필요로하는 Prefab 리소스 정보 SO를 가져오지 못한 경우 리턴.
            if (!ScriptableObjectLoader.TryGetLoadScriptableObject<DialogueDirectingPrefabDataSO>(this.defaultPrefabDataSOPath, out var dialogueDirectingPrefabDataSO))
            {
                Debug.Log($"연출 서비스에서 필요로하는 Prefab 리소스 정보 SO를 가져오지 못함.");
                return;
            }
            this.defaultPrefabDataSO = dialogueDirectingPrefabDataSO;
        }

        public void GenerateAllDefaultGameObject()
        {
            foreach(var data in this.defaultPrefabDataSO.DialogueDirectingPrefabDataList)
            {
                if (this.DialogueViewObjectData.IsViewObjectContained(data.PrefabKey)) continue;

                var ParentGameObjectData = MonoBehaviour.Instantiate(data.Prefab);

                IParentGameObjectDataHandler parentGameObjectDataHandler = ParentGameObjectData.GetComponent<IParentGameObjectDataHandler>();

                // Root 인스턴스 GameObject 등록.
                this.DialogueViewObjectData.RegisterViewObject(data.PrefabKey, ParentGameObjectData);
                // Root 객체가 갖고 있는 부모 객체 정보 연결.
                this.DialogueParentObjectData.RegisterParentGameObjectDataHandler(data.PrefabKey, parentGameObjectDataHandler);

                // Root 객체의 연출
                var newOutputView = ParentGameObjectData.GetComponent<IDialogueOutputViewBinding>();
                if (newOutputView != null)
                    newOutputView.InitialBinding(data.PrefabKey, this.MultiPlugInHub);
            }
        }
    }
}