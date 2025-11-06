using UnityEngine;
using System.Collections.Generic;

// Json을 사용하기 위한 데이터.
namespace GameSystems.DialogueDirectingService.Datas
{
    [System.Serializable]
    public class DialogueDirectingResourceData
    {
        [SerializeField] private string key;
        [SerializeField] private List<string> CanvasUIUXKeys;
        [SerializeField] private List<string> ActorKeys;
        [SerializeField] private List<string> SpriteKeys;

        public string Key { get => key; }

        public bool TryGetCanvasUIUXKeys(out string[] canvasUIUXKeys)
        {
            canvasUIUXKeys = default;
            if (this.CanvasUIUXKeys == null) return false;

            canvasUIUXKeys = this.CanvasUIUXKeys.ToArray();
            return true;
        }
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
    }
}
