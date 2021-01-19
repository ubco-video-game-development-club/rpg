using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Architect.Dialogue
{
	[CreateAssetMenu(fileName = "Dialogue Graph", menuName = "Dialogue Graph", order = 1)]
	public class DialogueGraph : ScriptableObject
	{
		private static readonly Color entryColour = new Color(192.0f / 255.0f, 216.0f / 255.0f, 227.0f / 255.0f);
		private static readonly Color exitColour = new Color(225.0f / 255.0f, 138.0f / 255.0f, 122.0f / 255.0f);

		public List<DialogueGraphNode> nodes;
		[System.NonSerialized] private DialogueGraphNode exitNode = new DialogueGraphNode("Exit", new Vector2(200, 0));

		public void Draw(Vector2 offset)
		{
			//Draw entry node
			GUI.color = entryColour;
			nodes[0].Draw(offset);
			GUI.color = Color.white;

			//Draw other nodes
			for(int i = 1; i < nodes.Count; i++)
			{
				nodes[i].Draw(offset);
			}

			//Draw exit node
			GUI.color = exitColour;
			exitNode.Draw(offset);
			GUI.color = Color.white;
		}

		public void ProcessEvents(Event e)
		{
			for(int i = 0; i < nodes.Count; i++)
			{
				nodes[i].ProcessEvents(e);
			}

			exitNode.ProcessEvents(e);
		}
	}
}
