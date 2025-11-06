using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Foundations.PlugInHub;
using GameSystems.DialogueDirectingService.PlainServices;
using GameSystems.DialogueDirectingService.Datas;

namespace GameSystems.DialogueDirectingService.Views
{
    public interface IChoiceButtonOnClicked
    {
        public bool IsBlockedState { get; }

        public string ChoiceButtonKey { get; }
        public int NextBranchPoint { get; }
    }

    public class DialogueDirectingChoiceButtonView : MonoBehaviour, IChoiceButtonSetter, IChoiceButtonOnClicked
    {
        [SerializeField] private GameObject ChoiceButtonObject;
        [SerializeField] private Image ButtonContentParent;
        [SerializeField] private TextMeshProUGUI ButtonContent;

        private Dictionary<string, bool> ConditionCheckSet;
        private bool isClickedOnce;
        
        private float safeWaitDuration = 0.3f;
        private float currentWaitDuration = 0f;

        public string ChoiceButtonKey { get; set; }
        public int NextBranchPoint { get; set; }
        public bool IsBlockedState { get; set; }

        public void InitialBinding(string key, IMultiPlugInHub multiPlugInHub, IFadeInAndOutService fadeInAndOutService = null)
        {
            this.ChoiceButtonKey = key;

            multiPlugInHub.RegisterPlugIn<IChoiceButtonSetter>(key, this);

            this.ConditionCheckSet = new();

            this.IsBlockedState = true;
        }

        public void SetCondition(string[] needSelectButtonKeys)
        {
            foreach (string otherButtonKey in needSelectButtonKeys)
            {
                if (this.ConditionCheckSet.ContainsKey(otherButtonKey)) continue;

                this.ConditionCheckSet.Add(otherButtonKey, false);
            }
        }

        public IEnumerator OperateChoiceButtonDisplay(string content, int nextBranchPoint, BehaviourToken behaviourToken)
        {
            // 버튼이 잘못 입력 되지 않도록 잠시 차단.
            this.IsBlockedState = true;

            // 조건이 null이면, 바로 true
            if(this.ConditionCheckSet == null)
            {
                // 해당 선택지 GameObject 활성화.
                this.ChoiceButtonObject.SetActive(true);

                // 한번 클릭된 버튼의 경우 색 변경.
                if (this.isClickedOnce)
                {
                    this.ButtonContentParent.color = new Color(225, 225, 225, 255);
                    this.ButtonContent.color = new Color(0, 0, 0, 200);
                }
                // 버튼 색 설정.
                else
                {
                    this.ButtonContentParent.color = new Color(255, 255, 255, 255);
                    this.ButtonContent.color = new Color(0, 0, 0, 255);
                }
            }
            else
            {
                // 조건 만족한 경우
                if (this.isConditionMet())
                {
                    // 해당 선택지 GameObject 활성화.
                    this.ChoiceButtonObject.SetActive(true);

                    // 한번 클릭된 버튼의 경우 색 변경.
                    if (this.isClickedOnce)
                    {
                        this.ButtonContentParent.color = new Color(225, 225, 225, 255);
                        this.ButtonContent.color = new Color(0, 0, 0, 200);
                    }
                    // 버튼 색 설정.
                    else
                    {
                        this.ButtonContentParent.color = new Color(255, 255, 255, 255);
                        this.ButtonContent.color = new Color(0, 0, 0, 255);
                    }
                }
            }

            // 선택지 내용 설정.
            this.ButtonContent.text = content;
            // 버튼 분기 설정
            this.NextBranchPoint = nextBranchPoint;

            // 안전하게 일정 시각 경과 후
            while (this.currentWaitDuration < this.safeWaitDuration)
            {
                if (behaviourToken.IsRequestEnd) break;

                this.currentWaitDuration += Time.deltaTime;
                yield return Time.deltaTime;
            }

            // 버튼 차단 해제.
            this.IsBlockedState = false;
            this.currentWaitDuration = 0;
        }

        private bool isConditionMet()
        {
            foreach (var data in this.ConditionCheckSet)
            {
                if (data.Value == false)
                    return false;
            }

            return true;
        }

        public void UpdateChoiceButtonView(string selectedButtonKey)
        {
            // 내가 갖는 조건에 들어가는 버튼이 클릭되었다면, 기록.
            if (this.ConditionCheckSet.ContainsKey(selectedButtonKey))
            {
                this.ConditionCheckSet[selectedButtonKey] = true;
            }

            // 내가 클릭된 거면, 깜빡임.
            if(this.ChoiceButtonKey == selectedButtonKey)
            {
                // 한번 클릭됨 명시.
                this.isClickedOnce = true;
                
                this.ButtonContentParent.color = new Color(255, 255, 255, 255);
                this.ButtonContent.color = new Color(0, 0, 0, 255);
            }
            // 다른 버튼이 클릭된 거면,
            else
            {
                this.ButtonContentParent.color = new Color(150, 150, 150, 200);
                this.ButtonContent.color = new Color(0, 0, 0, 200);
            }

            // 해당 선택지 GameObject 비활성화.
            this.ChoiceButtonObject.SetActive(false);
        }
    }
}