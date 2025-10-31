using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.DTOs
{
    public interface IDialogueDirectingResourceDataDB
    {
        public bool TryGetDialogueDirectingResourceData(string key, out DialogueDirectingResourceData dialogueDirectingResourceData);
    }

    [System.Serializable]
    public class DialogueDirectingResourceDataDB : IDialogueDirectingResourceDataDB
    {
        [SerializeField] private List<DialogueDirectingResourceData> DialogueDirectingResourceDatas;

        public bool TryGetDialogueDirectingResourceData(string key, out DialogueDirectingResourceData dialogueDirectingResourceData)
        {
            dialogueDirectingResourceData = null;
            if (this.DialogueDirectingResourceDatas == null || this.DialogueDirectingResourceDatas.Count == 0) return false;

            foreach(var data in this.DialogueDirectingResourceDatas)
            {
                if(data.Key == key)
                {
                    dialogueDirectingResourceData = data;
                    return true;
                }
            }

            return false;
        }
    }

    [System.Serializable]
    public class DialogueDirectingResourceData
    {
        [SerializeField] private string key;
        [SerializeField] private List<string> ActorKeys;
        [SerializeField] private List<string> SpriteKeys;
        [SerializeField] private List<string> CanvasUIUXKeys;

        public string Key { get => key; }
        public bool TryGetActorKeys(out string[] actorKeys)
        {
            actorKeys = default;
            if (this.ActorKeys == null) return false;

            actorKeys = this.ActorKeys.ToArray();
            return true;
        }
        public bool TryGetSpriteKeys(out string[] spriteKeys)
        {
            spriteKeys = default;
            if (this.SpriteKeys == null) return false;

            spriteKeys = this.SpriteKeys.ToArray();
            return true;
        }
        public bool TryGetCanvasUIUXKeys(out string[] canvasUIUXKeys)
        {
            canvasUIUXKeys = default;
            if (this.CanvasUIUXKeys == null) return false;

            canvasUIUXKeys = this.CanvasUIUXKeys.ToArray();
            return true;
        }
    }
}