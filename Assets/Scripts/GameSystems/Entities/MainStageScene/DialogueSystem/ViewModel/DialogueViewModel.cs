using System.Collections.Generic;

using UnityEngine;

using Foundations.PlugInHub;
using GameSystems.DTOs;

namespace GameSystems.Entities.MainStageScene
{
    public interface IDialogueDirectingModel
    {
        public void RegisterDialogueDirectingData(List<DialogueDirectingData> dialogueDirectingDatas);
        public bool TryGetDialogueDirectingData(int nextDirectIndex, out DialogueDirectingData nextDialogueDirectingData);

        public DialogueDirectingCoroutineControlData TextDirectingCoroutineControlData { get; }
    }

    public class DialogueDirectingModel
    {
        private List<DialogueDirectingData> DialogueDirectingDatas;

        // Text 출력 코루틴 제어를 위한 데이터.
        private DialogueDirectingCoroutineControlData _TextDirectingCoroutineControlData;
        // CanvasUIUX, BackGround, Actor의 Action 코루틴 제어를 위한 데이터.
        private List<DialogueDirectingCoroutineControlData> ActionDirectingCoroutineControlDatas = new();
        // 연속적인 연출 수행을 위해, 마지막으로 수행한 연출 데이터의 내용을 기록해 놓습니다.
        private DialogueDirectingData LastDirectingData = new();

        // 자동 재생 기능 코루틴 제어 데이터.
        private AutoDialogueDirectingData AutoPlayDirectingData = new(2f);

        public DialogueDirectingModel()
        {
            this.DialogueDirectingDatas = new();

            this._TextDirectingCoroutineControlData = new();
            this.ActionDirectingCoroutineControlDatas = new();
        }

        // DialogueDirecting 등록/해제/Get
        public void RegisterDialogueDirectingData(List<DialogueDirectingData> dialogueDirectingDatas)
        {
            this.DialogueDirectingDatas = dialogueDirectingDatas;
        }
        public void RemoveDialogueDirectingData()
        {
            this.DialogueDirectingDatas = null;
        }
        public bool TryGetDialogueDirectingData(int nextDirectIndex, out DialogueDirectingData nextDialogueDirectingData)
        {
            nextDialogueDirectingData = null;

            if (this.DialogueDirectingDatas.Count <= nextDirectIndex || this.DialogueDirectingDatas[nextDirectIndex] == null) return false;

            nextDialogueDirectingData = this.DialogueDirectingDatas[nextDirectIndex];
            return true;
        }

        // Text 출력 코루틴 제어를 위한 데이터.
        public DialogueDirectingCoroutineControlData TextDirectingCoroutineControlData { get; }
    }

    public interface IDialogueViewObjectModel
    {
        public void RegisterViewObject(string key, GameObject viewObject);
        public void RemoveViewObject(string key);
        public bool TryGetViewObject(string key, out GameObject viewObject);
    }

    public class DialogueViewModel : IDialogueViewObjectModel, IMultiPlugInHub
    {
        private MultiPlugInHub MultiPlugInHub;
        private Dictionary<string, GameObject> ViewObjects;

        public DialogueViewModel()
        {
            this.MultiPlugInHub = new();
            this.ViewObjects = new();
        }

        // T 위임된 등록/해제/Get 기능
        public void RegisterPlugIn<T>(string key, T plugIn) where T : class => this.MultiPlugInHub.RegisterPlugIn<T>(key, plugIn);
        public void RemovePlugIn<T>(string key) where T : class => this.MultiPlugInHub.RemovePlugIn<T>(key);
        public bool TryGetPlugIn<T>(string key, out T plugIn) where T : class => this.MultiPlugInHub.TryGetPlugIn<T>(key, out plugIn);
        public bool TryGetAllPlugIn<T>(out T[] plugIns) where T : class => this.MultiPlugInHub.TryGetAllPlugIn<T>(out plugIns);

        // DialogueViewObjects 등록/해제
        public void RegisterViewObject(string key, GameObject viewObject)
        {
            if (this.ViewObjects.ContainsKey(key)) return;

            this.ViewObjects.Add(key, viewObject);
        }
        public void RemoveViewObject(string key)
        {
            if (!this.ViewObjects.ContainsKey(key)) return;

            this.ViewObjects.Remove(key);
        }
        public bool TryGetViewObject(string key, out GameObject viewObject)
        {
            viewObject = null;
            if (!this.ViewObjects.ContainsKey(key)) return false;

            viewObject = this.ViewObjects[key];
            return true;
        }
    }
}