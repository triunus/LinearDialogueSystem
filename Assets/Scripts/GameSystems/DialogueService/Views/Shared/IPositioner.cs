using System.Collections;

using UnityEngine;

using GameSystems.DialogueDirectingService.Datas;

namespace GameSystems.DialogueDirectingService.Views
{
    public interface IPositioner
    {
        public void DirectPosition(Vector3 position);
        public IEnumerator Move(Vector3[] positions, float[] durationsout, BehaviourToken behaviourToken);
    }
}