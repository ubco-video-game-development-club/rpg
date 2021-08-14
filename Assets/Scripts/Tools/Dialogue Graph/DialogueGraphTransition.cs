using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [System.Serializable]
    public class DialogueGraphTransition
    {
        public string name;
        public int from;
        public int to;

        public DialogueGraphTransition(string name, int from, int to)
        {
            this.name = name;
            this.from = from;
            this.to = to;
        }
    }
}
