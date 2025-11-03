using System.Collections;

using UnityEngine;

namespace GameSystems.DialogueDirectingService.Views
{
    public interface IPositioner
    {
        public void DirectPosition(Vector3 position);
        public IEnumerator Move(Vector3[] positions, float[] durationsout, Datas.BehaviourToken behaviourToken);
    }
}