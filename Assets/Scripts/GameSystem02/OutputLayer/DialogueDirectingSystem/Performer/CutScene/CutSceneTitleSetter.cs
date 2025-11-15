using System;
using System.Collections;
using UnityEngine;
using TMPro;

namespace GameSystems.OutputLayer.DialogueDirectingSystem
{
    public class CutSceneTitleSetter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI TitleUI;
        [SerializeField] private TextMeshProUGUI SubTitleUI;

        public IEnumerator SetCutSceneTitle(string title, string subTitle, Action onCompleted = null)
        {
            if (this.TitleUI == null || this.SubTitleUI == null)
            {
                Debug.Log($"Text UIUX 관련 SerializeField 연결 오류");
            }
            else
            {
                this.TitleUI.text = title;
                this.SubTitleUI.text = subTitle;
            }

            yield return Time.deltaTime;
            if (onCompleted != null) onCompleted.Invoke();
        }
    }
}