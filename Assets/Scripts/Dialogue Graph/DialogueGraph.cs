using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Architect.Dialogue
{
	[CreateAssetMenu(fileName = "Dialogue Graph", menuName = "Dialogue Graph", order = 1)]
	public class DialogueGraph : ScriptableObject
	{
		public List<DialogueGraphNode> nodes;

		public void Draw(Vector2 offset)
		{
			
		}

		public void ProcessEvents(Event e)
		{

		}
	}
}
