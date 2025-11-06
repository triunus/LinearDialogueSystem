using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.DialogueDirectingService.Datas
{
    public interface IParentGameObjectDataHandler
    {
        public IEnumerable<IParentGameObjectData> ParentGameObjectDataList { get; }
    }
}