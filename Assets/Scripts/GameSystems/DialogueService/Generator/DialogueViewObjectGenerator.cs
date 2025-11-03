using UnityEngine;

using GameSystems.DialogueDirectingService.PlainServices;
using GameSystems.DialogueDirectingService.Datas;

namespace GameSystems.DialogueDirectingService.Generator
{
    public interface IDialogueOutputViewBinding
    {
        public void InitialBinding(string key, IDialogueViewObjectData dialogueViewModel, IFadeInAndOutService fadeInAndOutService = null);
    }

    // ViewObject를 생성하기 위한, 리소스를 가져와야 됨.
    // Resource 정보를 모아놓은 json이 있음.
    // 이러한 Json 파일을 연출Index에 대응되도록 SO 파일이 있어.

    // PrefabKey에 대응되는 Prefab이 있어.

    // 2개의 SO를 가져와야 되고,
    // 1개의 Json을 파싱해서 Data화 시켜야 되.
    public class DialogueViewObjectGenerator
    {
        private IDialogueDirectingResourceJsonDataSO DialogueDirectingResourceJsonDataSO;
        private IDialogueDirectingPrefabDataSO DialogueDirectingPrefabDataSO;

        private IDialogueViewObjectData DialogueViewObjectData;

        private string resourceJsonDataSOPath = "ScriptableObject/DialogueDirectingService/ResourceJsonDataSOPath";
        private string prefabSOPath = "ScriptableObject/DialogueDirectingService/PrefabSOPath";

        public DialogueViewObjectGenerator(IDialogueViewObjectData dialogueViewModel)
        {
            this.DialogueViewObjectData = dialogueViewModel;

            this.SetScriptableObject();
        }

        private void SetScriptableObject()
        {
            ScriptableObjectLoader ScriptableObjectLoader = new();

            // 해당 연출 Index에 대응되는 필요 리소스정보들이 담긴 SO를 가져오지 못한 경우 리턴.
            if (!ScriptableObjectLoader.TryGetLoadScriptableObject<DialogueDirectingResourceJsonDataSO>(this.resourceJsonDataSOPath, out var dialogueDirectingResourceJsonDataSO))
            {
                Debug.Log($"해당 연출 Index에 대응되는 필요 리소스정보들이 담긴 SO를 가져오지 못함.");
                return;
            }
            this.DialogueDirectingResourceJsonDataSO = dialogueDirectingResourceJsonDataSO;

            // 연출 서비스에서 필요로하는 Prefab 리소스 정보 SO를 가져오지 못한 경우 리턴.
            if (!ScriptableObjectLoader.TryGetLoadScriptableObject<DialogueDirectingPrefabDataSO>(this.prefabSOPath, out var dialogueDirectingPrefabDataSO))
            {
                Debug.Log($"연출 서비스에서 필요로하는 Prefab 리소스 정보 SO를 가져오지 못함.");
                return;
            }
            this.DialogueDirectingPrefabDataSO = dialogueDirectingPrefabDataSO;
        }

        // 특정 연출 Index에 대응되는 ViewObject를 미리 설정해 놓습니다.
        public void SetDialogueResource(int dialogueIndex)
        {
            JsonFileConverter JsonFileConverter = new();

            // 해당 연출 Index에 대응되는 필요 리소스정보 Json 파일을 가져오지 못한 경우 리턴.
            if (!this.DialogueDirectingResourceJsonDataSO.TryGetDialogueDirectingResourceJsonData(dialogueIndex, out var dialogueDirectingResourceJsonData))
            {
                Debug.Log($"해당 연출 Index에 대응되는 필요 리소스정보 Json 파일을 가져오지 못함.");
                return;
            }

            // 가져온 Json 파일을 Data로 파싱.
            if(!JsonFileConverter.TryParseJsonToData<DialogueDirectingResourceData>(dialogueDirectingResourceJsonData.JsonFile, out var dialogueDirectingResourceData))
            {
                Debug.LogError("jsonFile이 비어있습니다.");
                return;
            }

            FadeInAndOutService fadeInAndOutService = new();

            // CanvasUIUX Generate
            if (dialogueDirectingResourceData.TryGetCanvasUIUXKeys(out var canvasUIUXs))
            {
                foreach (var key in canvasUIUXs)
                {
                    if (this.DialogueDirectingPrefabDataSO.TryGetPrefabData(key, out var prefabData))
                    {
                        this.GenerateViewObject(prefabData, fadeInAndOutService);
                    }
                }
            }
            // Actor Generate
            if (dialogueDirectingResourceData.TryGetActorKeys(out var actorKeys))
            {
                foreach(var key in actorKeys)
                {
                    if(this.DialogueDirectingPrefabDataSO.TryGetPrefabData(key, out var prefabData))
                    {
                        this.GenerateViewObject(prefabData, fadeInAndOutService);
                    }
                }
            }
            // Sprite Generate
            if (dialogueDirectingResourceData.TryGetSpriteKeys(out var spriteKeys))
            {
                foreach (var key in spriteKeys)
                {
                    if (this.DialogueDirectingPrefabDataSO.TryGetPrefabData(key, out var prefabData))
                    {
                        this.GenerateViewObject(prefabData, fadeInAndOutService);
                    }
                }
            }
        }

        public void ResetDialogueService()
        {

        }

        public void GenerateViewObject(DialogueDirectingPrefabData dialogueDirectingPrefabData, FadeInAndOutService fadeInAndOutService)
        {
            GameObject newViewPrefab = MonoBehaviour.Instantiate(dialogueDirectingPrefabData.Prefab, dialogueDirectingPrefabData.PrefabParent);
            this.DialogueViewObjectData.RegisterViewObject(dialogueDirectingPrefabData.PrefabKey, newViewPrefab);

            var newOutputView = newViewPrefab.GetComponent<IDialogueOutputViewBinding>();
            if(newOutputView != null)
                newOutputView.InitialBinding(dialogueDirectingPrefabData.PrefabKey, this.DialogueViewObjectData, fadeInAndOutService);
        }
    }
}