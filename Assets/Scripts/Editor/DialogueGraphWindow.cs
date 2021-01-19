using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Architect.Dialogue
{
	public class DialogueGraphWindow : EditorWindow
	{
		public Vector2 CentreOfWindow { get { return this.position.size / 2.0f; } }
		private DialogueGraph selectedGraph = null;

		void OnEnable()
		{
			UpdateSelectedGraph();
		}

		void OnGUI()
		{
			if(selectedGraph == null)
			{
				ShowMessage("Select a dialogue graph to begin editing.");
				return;
			}

			Vector2 offset = CentreOfWindow;
			selectedGraph.Draw(offset);
			selectedGraph.ProcessEvents(Event.current);

			if(GUI.changed)
			{
				Repaint();
			}
		}

		void OnSelectionChange()
		{
			UpdateSelectedGraph();
		}

		private void UpdateSelectedGraph()
		{
			if(Selection.activeObject is DialogueGraph)
			{
				selectedGraph = Selection.activeObject as DialogueGraph;
				if(selectedGraph.nodes.Count <= 0) selectedGraph.nodes.Add(new DialogueGraphNode("Entry", new Vector2(-200, 0)));
			} else 
			{
				selectedGraph = null;
			}

			Repaint();
		}

		private void ShowMessage(string message)
		{
			Vector2 position = CentreOfWindow;
			
			GUIContent content = new GUIContent(message);
			Vector2 size = GUI.skin.label.CalcSize(content);			

			position.x -= size.x / 2.0f; //Centre the text
			GUI.Label(new Rect(position, size), message);
		}

		[MenuItem("Window/Dialogue Graph")]
		public static void CreateWindow()
		{
			DialogueGraphWindow window = GetWindow<DialogueGraphWindow>();
			window.titleContent = new GUIContent("Dialogue Graph");
		}
	}
}
