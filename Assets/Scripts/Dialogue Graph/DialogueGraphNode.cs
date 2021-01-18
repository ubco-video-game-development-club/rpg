using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Architect.Dialogue
{
	[System.Serializable]
	public class DialogueGraphNode
	{
		public string name;
		public Vector2 position;
		public List<DialogueGraphTransition> transitions;

		public DialogueGraphNode(string name, Vector2 position)
		{
			this.name = name;
			this.position = position;
			transitions = new List<DialogueGraphTransition>();
		}
	}
}