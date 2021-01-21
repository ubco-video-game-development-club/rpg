using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dialogue
{
	public class DialogueGraphWindow : EditorWindow
	{
		public Vector2 CentreOfWindow { get { return this.position.size / 2.0f; } }
		private DialogueGraph selectedGraph = null;
		private bool isCreatingTransition = false;
		private DialogueGraphNode transitionFrom = null;
        private Vector2 panOffset = new Vector2(0, 0);

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

			Event currentEvent = Event.current;
			ProcessEvents(currentEvent);

			if(isCreatingTransition)
			{
				Vector2 from = transitionFrom.position + CentreOfWindow;
				Vector2 to = currentEvent.mousePosition;
				Handles.DrawBezier(from, to, from, to, DialogueGraph.lineColour, null, DialogueGraph.LINE_WIDTH);
				GUI.changed = true;
			} else 
			{
				selectedGraph.ProcessEvents(currentEvent);
			}

			Vector2 offset = CentreOfWindow + panOffset;
			selectedGraph.Draw(offset);

			if(GUI.changed)
			{
				Repaint();
			}
		}

		void OnSelectionChange()
		{
			UpdateSelectedGraph();
		}

		private void ProcessEvents(Event e)
		{
			if(e.type == EventType.MouseDown)
			{
                if(e.button == 0)
                {
                    DialogueGraphNode hoverNode = selectedGraph.GetNodeAt(e.mousePosition);
					if(isCreatingTransition && hoverNode != null)
					{
						EndTransition(hoverNode);
					} else if(hoverNode == null)
                    {
                        selectedGraph.selectedNode = -1;
                    }
                } else if(e.button == 1)
				{
					CreateContextMenu(e.mousePosition);
					e.Use();
				}
			} else if(e.type == EventType.MouseDrag && e.button == 2)
            {
                panOffset += e.delta;
                e.Use();
            }
		}

		private void CreateContextMenu(Vector2 position)
		{
			GenericMenu menu = new GenericMenu();

			DialogueGraphNode hoveredNode = selectedGraph.GetNodeAt(position);
            if(hoveredNode != selectedGraph.exitNode)
            {
                if(hoveredNode != null)
                {
                    menu.AddItem(
                        new GUIContent("Create transition"),
                        false,
                        () => BeginTransition(hoveredNode)
                    );

                    if(selectedGraph.nodes.Count > 1)
                    {
                        menu.AddItem(
                            new GUIContent("Delete node"),
                            false,
                            () => selectedGraph.RemoveNode(hoveredNode)
                        );
                    }
                } else 
                {
                    menu.AddItem(
                        new GUIContent("Create node"),
                        false,
                        () => selectedGraph.CreateNode("New node", position - CentreOfWindow)
                    );
                }
            }

			menu.ShowAsContext();
		}

		private void BeginTransition(DialogueGraphNode from)
		{
			isCreatingTransition = true;
			transitionFrom = from;
		}

		private void EndTransition(DialogueGraphNode to)
		{
			isCreatingTransition = false;
			
			if(transitionFrom == to) return;

			int fromIndex = selectedGraph.nodes.IndexOf(transitionFrom);
			int toIndex = selectedGraph.nodes.IndexOf(to);
			DialogueGraphTransition transition = new DialogueGraphTransition("Transition", fromIndex, toIndex);
			selectedGraph.transitions.Add(transition);
		}

		private void UpdateSelectedGraph()
		{
			if(Selection.activeObject is DialogueGraph)
			{
				selectedGraph = Selection.activeObject as DialogueGraph;
				if(selectedGraph.nodes.Count <= 0) selectedGraph.nodes.Add(new DialogueGraphNode("Entry", new Vector2(-200, 0)));
                panOffset = new Vector2(0, 0);
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
