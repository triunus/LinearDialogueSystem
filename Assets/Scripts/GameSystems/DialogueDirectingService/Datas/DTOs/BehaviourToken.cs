using System.Collections;
using System.Collections.Generic;

namespace GameSystems.DialogueDirectingService.Datas
{
    public class BehaviourToken
    {
        public BehaviourToken(bool isRequestEnd)
        {
            IsRequestEnd = isRequestEnd;
        }

        public bool IsRequestEnd { get; set; }
    }
}