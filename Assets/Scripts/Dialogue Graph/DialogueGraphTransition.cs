using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Architect.Dialogue
{
	[System.Serializable]
	public class DialogueGraphTransition
	{
		public string name;
		public DialogueGraphNode from;
		public DialogueGraphNode to;

		public DialogueGraphTransition(string name, DialogueGraphNode from, DialogueGraphNode to)
		{
			this.name = name;
			this.from = from;
			this.to = to;
		}
	}
}
