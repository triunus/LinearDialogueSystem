using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.OutputLayer.DialogueDirectingSystem
{
    public interface IActorSpriteSetter
    {
        public IEnumerator SetActorBaseSprtie(string key, Action onCompleted = null);
        public IEnumerator SetActorDetailSprite(string key, Action onCompleted = null);
    }

    public class ActorSpriteSetter : MonoBehaviour, IActorSpriteSetter
    {
        [SerializeField] private SpriteRenderer ActorSprite;

        [SerializeField] private List<ActorSpriteData> ActorBaseData;
        [SerializeField] private List<ActorSpriteData> ActorDetailDatas;

        // ISpriteSetter
        public IEnumerator SetActorBaseSprtie(string key, Action onCompleted = null)
        {
            var data = this.ActorBaseData.Find(x => x.Key == key);

            if (this.ActorSprite != null && data != default)
                this.ActorSprite.sprite = data.Sprite;

            yield return Time.deltaTime;
            if (onCompleted != null) onCompleted.Invoke();
        }

        public IEnumerator SetActorDetailSprite(string key, Action onCompleted = null)
        {
            var data = this.ActorDetailDatas.Find(x => x.Key == key);

            if (this.ActorSprite != null && data != default)
                this.ActorSprite.sprite = data.Sprite;

            yield return Time.deltaTime;
            if (onCompleted != null) onCompleted.Invoke();
        }
    }

    [System.Serializable]
    public class ActorSpriteData
    {
        [SerializeField] private string _Key;
        [SerializeField] private Sprite _Sprite;

        public string Key { get => _Key; }
        public Sprite Sprite { get => _Sprite; }
    }
}