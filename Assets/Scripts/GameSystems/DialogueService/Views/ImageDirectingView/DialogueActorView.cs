using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Foundations.PlugInHub;
using GameSystems.DialogueDirectingService.PlainServices;
using GameSystems.DialogueDirectingService.Datas;


namespace GameSystems.DialogueDirectingService.Views
{
    public class DialogueActorView : MonoBehaviour, IDialogueViewBinding, IActivation, IFadeInAndOut, ISpriteSetter, IPositioner
    {
        private IFadeInAndOutService FadeInAndOutService;

        [SerializeField] private GameObject SpriteGameObject;
        [SerializeField] private SpriteRenderer AttitudeSpriteRenderer;
        [SerializeField] private SpriteRenderer FaceSpriteRenderer;

        [SerializeField] private float ShowedAlpha;
        [SerializeField] private float HidedAlpha;

        [SerializeField] private Color SpeakerColor;
        [SerializeField] private Color ListenerColor;

        [SerializeField] private List<ActorAttitudeData> ActorAttitudeDatas;
        [SerializeField] private List<ActorFaceData> ActorFaceDatas;
        
        private void Awake()
        {
            this.Hide();
        }
        public void InitialBinding(string key, IMultiPlugInHub multiPlugInHub, IFadeInAndOutService fadeInAndOutService = null)
        {
            multiPlugInHub.RegisterPlugIn<IActivation>(key, this);
            multiPlugInHub.RegisterPlugIn<IFadeInAndOut>(key, this);
            multiPlugInHub.RegisterPlugIn<ISpriteSetter>(key, this);
            multiPlugInHub.RegisterPlugIn<IPositioner>(key, this);

            this.FadeInAndOutService = fadeInAndOutService;
        }

        // IActivation
        public void Show()
        {
            this.SpriteGameObject.SetActive(true);
        }
        public void Hide()
        {
            this.SpriteGameObject.SetActive(false);
        }

        // IFadeInAndOut
        public IEnumerator FadeIn(float duration, BehaviourToken behaviourToken)
        {
            // 일단 FadeOut 값으로 만들기.
            this.FadeInAndOutService.SetAlphaValue( new SpriteRenderer[] { this.AttitudeSpriteRenderer, this.FaceSpriteRenderer }, this.HidedAlpha);
            // 활성화가 안되어 있으면 활성화.
            this.Show();

            yield return Time.deltaTime;

            yield return this.FadeInAndOutService.FadeOperation(new SpriteRenderer[] { this.AttitudeSpriteRenderer, this.FaceSpriteRenderer }, this.HidedAlpha, this.ShowedAlpha, duration, behaviourToken);
        }
        public IEnumerator FadeOut(float duration, BehaviourToken behaviourToken)
        {
            yield return this.FadeInAndOutService.FadeOperation(new SpriteRenderer[] { this.AttitudeSpriteRenderer, this.FaceSpriteRenderer }, this.ShowedAlpha, this.HidedAlpha, duration, behaviourToken);

            this.Hide();
        }

        // ISpriteSetter
        public void SetAttitude(AttitudeType attitudeType)
        {
            Debug.Log($"자세 설정 : {attitudeType}");

            var data = this.ActorAttitudeDatas.FirstOrDefault(x => x.AttitudeType == attitudeType);
            if (data == default) return;

            this.AttitudeSpriteRenderer.sprite = Sprite.Create(data.Texture2D, new Rect(0, 0, data.Texture2D.width, data.Texture2D.height), new Vector2(0.5f, 0.5f));
            this.AttitudeSpriteRenderer.sprite.name = data.Texture2D.name;
        }
        public void SetFace(FaceType faceType)
        {
            Debug.Log($"표정 설정 : {faceType}");

            var data = this.ActorFaceDatas.FirstOrDefault(x => x.FaceType == faceType);
            if (data == default) return;

            this.FaceSpriteRenderer.sprite = Sprite.Create(data.Texture2D, new Rect(0, 0, data.Texture2D.width, data.Texture2D.height), new Vector2(0.5f, 0.5f));
            this.FaceSpriteRenderer.sprite.name = data.Texture2D.name;
        }

        // IPositioner
        public void DirectPosition(Vector3 position)
        {
            this.SpriteGameObject.transform.position = position;

            this.TempFlipX();
        }
        public IEnumerator Move(Vector3[] positions, float[] duration, BehaviourToken behaviourToken)
        {
            this.SpriteGameObject.transform.position = positions[0];
            this.Show();

            yield return Time.deltaTime;

            for(int i = 0; i < duration.Length; ++i)
            {
                if (behaviourToken.IsRequestEnd) break;
                // 초기값.
                Vector3 start = positions[i];
                Vector3 end = positions[i+1];
                float elapsed = 0f;

                // FilpX
                float directionX = end.x - start.x;
                this.AttitudeSpriteRenderer.flipX = directionX < 0f;

                while (elapsed < duration[i])
                {
                    if (behaviourToken.IsRequestEnd) break;

                    float t = elapsed / duration[i];
                    this.SpriteGameObject.transform.position = Vector3.Lerp(start, end, t);

                    elapsed += Time.deltaTime;
                    yield return Time.deltaTime;
                }

                // 마지막 위치 지정
                this.SpriteGameObject.transform.position = end;
            }

            // 마지막 위치 지정
            this.SpriteGameObject.transform.position = positions[positions.Length-1];
            // FilpX
            this.TempFlipX();
        }
        private void TempFlipX()
        {
            Camera cam = Camera.main;

            Vector3 screenPos = cam.WorldToViewportPoint(transform.position);

            // 화면 절반보다 오른쪽이면 flipX = true, 왼쪽이면 false
            AttitudeSpriteRenderer.flipX = screenPos.x > 0.5f;
        }

        public void SetSpeakerColor()
        {
            this.AttitudeSpriteRenderer.color = this.SpeakerColor;
            this.FaceSpriteRenderer.color = this.SpeakerColor;
        }
        public void SetListenerColor()
        {
            this.AttitudeSpriteRenderer.color = this.ListenerColor;
            this.FaceSpriteRenderer.color = this.ListenerColor;
        }
    }

    [System.Serializable]
    public class ActorAttitudeData
    {
        [SerializeField] private AttitudeType _AttitudeType;
        [SerializeField] private Texture2D _Texture2D;

        public AttitudeType AttitudeType { get => _AttitudeType; }
        public Texture2D Texture2D { get => _Texture2D; }
    }
    [System.Serializable]
    public class ActorFaceData
    {
        [SerializeField] private FaceType _FaceType;
        [SerializeField] private Texture2D _Texture2D;

        public FaceType FaceType { get => _FaceType; }
        public Texture2D Texture2D { get => _Texture2D; }
    }
}