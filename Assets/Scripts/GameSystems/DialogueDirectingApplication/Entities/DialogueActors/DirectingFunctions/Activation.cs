/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.DialogueDirectingApplication.Entities
{
    public class DirectingActionControlData
    {
        private int DirectingIndex;

        private bool isEnded;
        private bool isRequestEnd;

        public DirectingActionControlData(int directingIndex)
        {
            this.DirectingIndex = directingIndex;

            this.isEnded = false;
            this.isRequestEnd = false;
        }

        public bool IsEnded { get => isEnded; set => isEnded = value; }
        public bool IsRequestEnd { get => isRequestEnd; set => isRequestEnd = value; }
    }

    public interface IDirecting_Graphic_Fader
    {
        public IEnumerator FadeIn(float duration, DirectingActionControlData actionControlData);
        public IEnumerator FadeOut(float duration, DirectingActionControlData actionControlData);
    }

    public class Directing_Graphic_Fader : MonoBehaviour, IDirecting_Graphic_Fader
    {
        private List<Graphic> TargetObjects;

        [SerializeField] private float StartAlpha;
        [SerializeField] private float TargetAlpha;


    }

    public interface IDirecting_Activater
    {
        public void Show();
        public void Hide();
    }

    public class Directing_Activater : MonoBehaviour, IDirecting_Activater
    {
        [SerializeField] private List<GameObject> TargetObjects;

        public void Show()
        {
            if (this.TargetObjects == null) return;

            foreach (GameObject target in this.TargetObjects)
            {
                target.SetActive(true);
            }
        }

        public void Hide()
        {
            if (this.TargetObjects == null) return;

            foreach (GameObject target in this.TargetObjects)
            {
                target.SetActive(false);
            }
        }
    }
}*/