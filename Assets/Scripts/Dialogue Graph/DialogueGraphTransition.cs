using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Architect.Dialogue
{
	[System.Serializable]
	public class DialogueGraphTransition
	{
		public readonly string name;
		public readonly DialogueGraphNode from;
		public readonly DialogueGraphNode to;

		public DialogueGraphTransition(string name, DialogueGraphNode from, DialogueGraphNode to)
		{
			this.name = name;
			this.from = from;
			this.to = to;
		}
	}
}
