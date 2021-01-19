using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Architect.Dialogue
{
	[System.Serializable]
	public class DialogueGraphNode
	{
		private static readonly Vector2 nodeSize = new Vector2(200, 50);

		public string name;
		public Vector2 position;
		public List<DialogueGraphTransition> transitions;
		private Rect displayRect;
		private bool isSelected = false;

		public DialogueGraphNode(string name, Vector2 position)
		{
			this.name = name;
			this.position = position;
			transitions = new List<DialogueGraphTransition>();
		}

		public void Draw(Vector2 offset)
		{
			Vector2 pos = position - nodeSize / 2.0f;
			displayRect = new Rect(pos + offset, nodeSize);
			GUI.Box(displayRect, name, GUI.skin.button);
		}

		public void ProcessEvents(Event e)
		{
			switch(e.type)
			{
				case EventType.MouseDown:
					if(e.button == 0 && displayRect.Contains(e.mousePosition))
					{
						isSelected = true;
						GUI.changed = true;
					}
					break;
				case EventType.MouseUp:
					if(e.button == 0)
					{
						isSelected = false;
						GUI.changed = true;
					}
					break;
				case EventType.MouseDrag:
					if(isSelected)
					{
						position += e.delta;
						GUI.changed = true;
					}
					break;
				default:
					break;
			}
		}

		public bool Contains(Vector2 position) => displayRect.Contains(position);
	}
}