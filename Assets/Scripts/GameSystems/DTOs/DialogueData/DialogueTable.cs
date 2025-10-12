using System;
using System.Collections.Generic;

namespace GameSystems.DTOs
{
    [Serializable]
    public class DialogueTable
    {
        public List<DialogueRow> DialogueRows;
    }

    [Serializable]
    public struct DialogueRow
    {
        public int DialogueIndex;

        public string Speaker;
        public string Line;

        public int IllustIndex;
        public int IllustPosition;
        public bool IllustFilpX;
    }
}