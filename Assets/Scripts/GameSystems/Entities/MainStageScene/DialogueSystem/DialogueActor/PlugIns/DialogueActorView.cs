using System.Collections;
using UnityEngine;

using GameSystems.PlainServices;
using GameSystems.DTOs;

namespace GameSystems.Entities.MainStageScene
{
    public class DialogueActorView : MonoBehaviour, IActivation, IFadeInAndOut, IPositioner, IFaceSetter
    {
        [SerializeField] private string Key;

        private DialogueActorActivationPlugInHub DialogueActorActivationPlugInHub;
        private DialogueActorFaderPlugInHub DialogueActorFaderPlugInHub;
        private DialogueActorFaceSetterPlugInHub DialogueActorFaceSetterPlugInHub;
        private DialogueActorPositionerPlugInHub DialogueActorPositionerPlugInHub;

        private IFadeInAndOutService FadeInAndOutService;

        [SerializeField] private GameObject ActorObject;
        [SerializeField] private SpriteRenderer ActorSpriteRenderer;

        [SerializeField] private float ShowedAlpha;
        [SerializeField] private float HidedAlpha;

        private void Awake()
        {
            var GlobalRepository = Repository.GlobalSceneRepository.Instance;

            this.FadeInAndOutService = GlobalRepository.PlainServices_LazyReferenceRepository.GetOrCreate<FadeInAndOutService>();

            var LocalEntityRepository = Repository.MainStageSceneRepository.Instance.Entity_LazyReferenceRepository;
            this.DialogueActorActivationPlugInHub = LocalEntityRepository.GetOrCreate<DialogueActorActivationPlugInHub>();
            this.DialogueActorFaderPlugInHub = LocalEntityRepository.GetOrCreate<DialogueActorFaderPlugInHub>();
            this.DialogueActorPositionerPlugInHub = LocalEntityRepository.GetOrCreate<DialogueActorPositionerPlugInHub>();

            this.DialogueActorFaceSetterPlugInHub = LocalEntityRepository.GetOrCreate<DialogueActorFaceSetterPlugInHub>();

            this.DialogueActorActivationPlugInHub.RegisterPlugIn(this.Key, this);
            this.DialogueActorFaderPlugInHub.RegisterPlugIn(this.Key, this);
            this.DialogueActorPositionerPlugInHub.RegisterPlugIn(this.Key, this);

            this.DialogueActorFaceSetterPlugInHub.RegisterPlugIn(this.Key, this);

            this.Hide();
        }

        private void OnDestroy()
        {
            var LocalEntityRepository = Repository.MainStageSceneRepository.Instance.Entity_LazyReferenceRepository;
            this.DialogueActorActivationPlugInHub = LocalEntityRepository.GetOrCreate<DialogueActorActivationPlugInHub>();
            this.DialogueActorFaderPlugInHub = LocalEntityRepository.GetOrCreate<DialogueActorFaderPlugInHub>();
            this.DialogueActorPositionerPlugInHub = LocalEntityRepository.GetOrCreate<DialogueActorPositionerPlugInHub>();

            this.DialogueActorFaceSetterPlugInHub = LocalEntityRepository.GetOrCreate<DialogueActorFaceSetterPlugInHub>();

            this.DialogueActorActivationPlugInHub.RemovePlugIn(this.Key);
            this.DialogueActorFaderPlugInHub.RemovePlugIn(this.Key);
            this.DialogueActorPositionerPlugInHub.RemovePlugIn(this.Key);

            this.DialogueActorFaceSetterPlugInHub.RemovePlugIn(this.Key);
        }

        public void Show()
        {
            this.ActorObject.SetActive(true);
        }
        public void Hide()
        {
            this.ActorObject.SetActive(false);
        }

        public IEnumerator FadeIn(float duration)
        {
            this.FadeInAndOutService.SetAlphaValue(this.ActorSpriteRenderer, this.HidedAlpha);
            this.Show();

            yield return Time.deltaTime;

            yield return this.FadeInAndOutService.FadeOperation(this.ActorSpriteRenderer, this.HidedAlpha, this.ShowedAlpha, duration);
        }
        public IEnumerator FadeOut(float duration)
        {
            yield return this.FadeInAndOutService.FadeOperation(this.ActorSpriteRenderer, this.ShowedAlpha, this.HidedAlpha, duration);

            this.Hide();
        }

        public void SetFace(FaceType faceType)
        {
            Debug.Log($"표정 설정 : {faceType}");
        }

        public void DirectPosition(Vector3 position)
        {
            this.ActorObject.transform.position = position;
        }
        public IEnumerator Move(Vector3[] positions, float[] durations)
        {
            if (positions.Length < 2 || durations.Length < 1)
                yield break;

            this.ActorObject.transform.position = positions[0];
            this.Show();

            yield return Time.deltaTime;

            for (int i = 0; i < durations.Length; i++)
            {
                Vector3 start = positions[i];
                Vector3 end = positions[i + 1];
                float duration = durations[i];
                float elapsed = 0f;

                float directionX = end.x - start.x;
                this.ActorSpriteRenderer.flipX = directionX < 0f;

                while (elapsed < duration)
                {
                    float t = elapsed / duration;
                    this.ActorObject.transform.position = Vector3.Lerp(start, end, t);

                    elapsed += Time.deltaTime;
                    yield return Time.deltaTime;
                }

                this.ActorObject.transform.position = end; // 정확히 마지막 위치 보정
            }

            this.TempFlipX();
        }

        private void TempFlipX()
        {
            Camera cam = Camera.main;

            Vector3 screenPos = cam.WorldToViewportPoint(transform.position);

            // 화면 절반보다 오른쪽이면 flipX = true, 왼쪽이면 false
            ActorSpriteRenderer.flipX = screenPos.x > 0.5f;
        }
    }
}