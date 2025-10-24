using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameSystems.PlainServices;
using GameSystems.DTOs;

namespace GameSystems.Entities.MainStageScene
{
    public interface ITexture2DSetter
    {
        public void SetTexture2D(Texture2D texture2D);
    }

    public class DialogueActorSpriteRendererView : MonoBehaviour, IActivation, IFadeInAndOut, IPositioner, ISpriteSetter
    {
        [SerializeField] private string Key;

        private IFadeInAndOutService FadeInAndOutService;

        [SerializeField] private GameObject SpriteGameObject;
        [SerializeField] private SpriteRenderer AttitudeSpriteRenderer;
        [SerializeField] private SpriteRenderer FaceSpriteRenderer;

        [SerializeField] private float ShowedAlpha;
        [SerializeField] private float HidedAlpha;

        private Dictionary<AttitudeType, Texture2D> AttitudeTexture2DSet = new();
        private Dictionary<FaceType, Texture2D> FaceTexture2DSet = new();

        [SerializeField] private Texture2D DefaultAttitude;
        [SerializeField] private Texture2D TestAttitude;
        [SerializeField] private Texture2D DefaultFace;
        [SerializeField] private Texture2D TestFace;

        private void Awake()
        {
            var GlobalRepository = Repository.GlobalSceneRepository.Instance;

            this.FadeInAndOutService = GlobalRepository.PlainServices_LazyReferenceRepository.GetOrCreate<FadeInAndOutService>();

            var LocalEntityRepository = Repository.MainStageSceneRepository.Instance.Entity_LazyReferenceRepository;

            var DialogueActivationPlugInHub = LocalEntityRepository.GetOrCreate<DialogueActivationPlugInHub>();
            var DialogueFaderPlugInHub = LocalEntityRepository.GetOrCreate<DialogueFaderPlugInHub>();
            var DialogueSpriteSetterPlugInHub = LocalEntityRepository.GetOrCreate<DialogueSpriteSetterPlugInHub>();
            var DialoguePositionerPlugInHub = LocalEntityRepository.GetOrCreate<DialoguePositionerPlugInHub>();

            DialogueActivationPlugInHub.RegisterPlugIn(this.Key, this);
            DialogueFaderPlugInHub.RegisterPlugIn(this.Key, this);
            DialogueSpriteSetterPlugInHub.RegisterPlugIn(this.Key, this);
            DialoguePositionerPlugInHub.RegisterPlugIn(this.Key, this);

            this.Hide();
            this.AttitudeTexture2DSet.Add(AttitudeType.Default, DefaultAttitude);
            this.AttitudeTexture2DSet.Add(AttitudeType.Test, TestAttitude);
            this.FaceTexture2DSet.Add(FaceType.Default, DefaultFace);
            this.FaceTexture2DSet.Add(FaceType.Test, TestFace);
        }
        private void OnDestroy()
        {
            var LocalEntityRepository = Repository.MainStageSceneRepository.Instance.Entity_LazyReferenceRepository;

            var DialogueActivationPlugInHub = LocalEntityRepository.GetOrCreate<DialogueActivationPlugInHub>();
            var DialogueFaderPlugInHub = LocalEntityRepository.GetOrCreate<DialogueFaderPlugInHub>();
            var DialogueSpriteSetterPlugInHub = LocalEntityRepository.GetOrCreate<DialogueSpriteSetterPlugInHub>();
            var DialoguePositionerPlugInHub = LocalEntityRepository.GetOrCreate<DialoguePositionerPlugInHub>();

            DialogueActivationPlugInHub.RemovePlugIn(this.Key);
            DialogueFaderPlugInHub.RemovePlugIn(this.Key);
            DialogueSpriteSetterPlugInHub.RemovePlugIn(this.Key);
            DialoguePositionerPlugInHub.RemovePlugIn(this.Key);
        }

        public void IntialSetTexture2D(string key, SpriteAttitudeTexture2D[] spriteAttitudeTexture2Ds, SpriteFaceTexture2D[] SpriteFaceTexture2Ds)
        {
            this.Key = key;

            foreach (var data in spriteAttitudeTexture2Ds)
            {
                this.AttitudeTexture2DSet.Add(data.AttitudeType, data.Texture2D);
            }

            foreach (var data in SpriteFaceTexture2Ds)
            {
                this.FaceTexture2DSet.Add(data.FaceType, data.Texture2D);
            }
        }

        public void Show()
        {
            this.SpriteGameObject.SetActive(true);
        }
        public void Hide()
        {
            this.SpriteGameObject.SetActive(false);
        }

        public IEnumerator FadeIn(float duration, DTOs.BehaviourToken behaviourToken)
        {
            // 일단 FadeOut 값으로 만들기.
            this.FadeInAndOutService.SetAlphaValue( new SpriteRenderer[] { this.AttitudeSpriteRenderer, this.FaceSpriteRenderer }, this.HidedAlpha);
            // 활성화가 안되어 있으면 활성화.
            this.Show();

            yield return Time.deltaTime;

            yield return this.FadeInAndOutService.FadeOperation(new SpriteRenderer[] { this.AttitudeSpriteRenderer, this.FaceSpriteRenderer }, this.HidedAlpha, this.ShowedAlpha, duration, behaviourToken);
        }
        public IEnumerator FadeOut(float duration, DTOs.BehaviourToken behaviourToken)
        {
            yield return this.FadeInAndOutService.FadeOperation(new SpriteRenderer[] { this.AttitudeSpriteRenderer, this.FaceSpriteRenderer }, this.ShowedAlpha, this.HidedAlpha, duration, behaviourToken);

            this.Hide();
        }

        public void SetAttitude(AttitudeType attitudeType)
        {
            Debug.Log($"자세 설정 : {attitudeType}");

            Texture2D texture2D = this.AttitudeTexture2DSet[attitudeType];

            this.AttitudeSpriteRenderer.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
            this.AttitudeSpriteRenderer.sprite.name = texture2D.name;
        }
        public void SetFace(FaceType faceType)
        {
            Debug.Log($"표정 설정 : {faceType}");

            Texture2D texture2D = this.FaceTexture2DSet[faceType];

            this.FaceSpriteRenderer.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
            this.FaceSpriteRenderer.sprite.name = texture2D.name;
        }

        public void DirectPosition(Vector3 position)
        {
            this.SpriteGameObject.transform.position = position;

            this.TempFlipX();
        }
        public IEnumerator Move(Vector3[] positions, float[] duration, DTOs.BehaviourToken behaviourToken)
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
    }
}