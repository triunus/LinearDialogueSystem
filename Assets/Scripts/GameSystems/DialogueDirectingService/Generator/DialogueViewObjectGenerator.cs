using UnityEngine;

using Foundations.PlugInHub;
using GameSystems.DialogueDirectingService.PlainServices;
using GameSystems.DialogueDirectingService.Datas;

namespace GameSystems.DialogueDirectingService.Generator
{
    public interface IDialogueOutputViewBinding
    {
        public void InitialBinding(string viewObejctKey, IMultiPlugInHub  multiPlugInHub, IFadeInAndOutService fadeInAndOutService = null);
    }

    public interface IDialogueViewObjectGenerator
    {
        // 따로 Key 값이 존재하는 경우, ( Prefab을 중복으로 사용하여, 따로 고유키가 필요 )
        public bool TryGenerateViewObject(string prefabKey, string viewObjectKey);
        // Prefab Key값이 곧 고유키가 되는 경우. ( Prefab을 중복 사용하지 않음. )
        public bool TryGenerateViewObject(string prefabkey);
    }

    public class DialogueViewObjectGenerator : IDialogueViewObjectGenerator
    {
        private IDialogueDirectingPrefabDataSO DialogueDirectingPrefabDataSO;
        private string prefabDataSOPath = "ScriptableObjects/DialogueDirectingService/PrefabSO";

        private IMultiPlugInHub MultiPlugInHub;
        private IDialogueParentObjectData DialogueParentObjectData;
        private IDialogueViewObjectData DialogueViewObjectData;

        private FadeInAndOutService fadeInAndOutService = new();

        public DialogueViewObjectGenerator(IMultiPlugInHub multiPlugInHub, IDialogueParentObjectData dialogueParentObjectData, IDialogueViewObjectData dialogueViewObjectData)
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
            if (!ScriptableObjectLoader.TryGetLoadScriptableObject<DialogueDirectingPrefabDataSO>(this.prefabDataSOPath, out var dialogueDirectingPrefabDataSO))
            {
                Debug.Log($"연출 서비스에서 필요로하는 Prefab 리소스 정보 SO를 가져오지 못함.");
                return;
            }
            this.DialogueDirectingPrefabDataSO = dialogueDirectingPrefabDataSO;
        }

        public void GenerateAllNeedViewObject(DialogueResoruceData dialogueResoruceData)
        {
            foreach (var data in dialogueResoruceData.PrefabKeys)
            {
                this.TryGenerateViewObject(data);
            }
        }

        public bool TryGenerateViewObject(string prefabKey, string viewObjectKey)
        {
            // PrefabKey 값에 대응되는 prefab 정보를 가져옴.
            if (!this.DialogueDirectingPrefabDataSO.TryGetPrefabData(prefabKey, out var dialogueDirectingPrefabData))
            {
                Debug.Log($"SO에 Key에 대응되는 Prefab이 등록되어 있지 않습니다.");
                return false;
            }

            // Prefab의 부모Key 값에 대응되는 GameObject를 가져옴.
            if (!this.DialogueParentObjectData.TryGetParentGameObject(dialogueDirectingPrefabData.PrefabParentKey, out GameObject parentObject))
            {
                Debug.Log($"해당 Prefab의 부모 GameObject가 등록되어 있지 않습니다.");
                return false;
            }

            // Prefab 정보를 통해서, 인스턴스 생성.
            var newOutputViewObject = MonoBehaviour.Instantiate(dialogueDirectingPrefabData.Prefab, parentObject.GetComponent<Transform>());
            // GameObject 객체 등록.
            this.DialogueViewObjectData.RegisterViewObject(viewObjectKey, newOutputViewObject);

            // outputView 객체가 필요로하는 공통 기능 참조전달.
            var newOutputView = newOutputViewObject.GetComponent<IDialogueOutputViewBinding>();
            if (newOutputView != null)
                newOutputView.InitialBinding(viewObjectKey, this.MultiPlugInHub, this.fadeInAndOutService);

            return true;
        }
        public bool TryGenerateViewObject(string prefabkey)
        {
            if (this.DialogueViewObjectData.IsViewObjectContained(prefabkey)) return false;

            // PrefabKey 값에 대응되는 prefab 정보를 가져옴.
            if (!this.DialogueDirectingPrefabDataSO.TryGetPrefabData(prefabkey, out var dialogueDirectingPrefabData))
            {
                Debug.Log($"SO에 Key에 대응되는 Prefab이 등록되어 있지 않습니다.");
                return false;
            }

            // Prefab의 부모Key 값에 대응되는 GameObject를 가져옴.
            if (!this.DialogueParentObjectData.TryGetParentGameObject(dialogueDirectingPrefabData.PrefabParentKey, out GameObject parentObject))
            {
                Debug.Log($"해당 Prefab의 부모 GameObject가 등록되어 있지 않습니다.");
                return false;
            }

            // Prefab 정보를 통해서, 인스턴스 생성.
            var newOutputViewObject = MonoBehaviour.Instantiate(dialogueDirectingPrefabData.Prefab, parentObject.GetComponent<Transform>());
            // GameObject 객체 등록.
            this.DialogueViewObjectData.RegisterViewObject(prefabkey, newOutputViewObject);

            // outputView 객체가 필요로하는 공통 기능 참조전달.
            var newOutputView = newOutputViewObject.GetComponent<IDialogueOutputViewBinding>();
            if (newOutputView != null)
                newOutputView.InitialBinding(prefabkey, this.MultiPlugInHub, this.fadeInAndOutService);

            return true;
        }
    }
}