using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Architect.Dialogue
{
	[CustomEditor(typeof(DialogueGraph))]
	public class DialogueGraphInspector : Editor
	{
		private DialogueGraph graph;
		private SerializedProperty nodes;
		private SerializedProperty transitions;

		void OnEnable()
		{
			graph = serializedObject.targetObject as DialogueGraph;
			nodes = serializedObject.FindProperty("nodes");
			transitions = serializedObject.FindProperty("transitions");
		}

		public override void OnInspectorGUI()
		{
			int selectedNodeIndex = graph.selectedNode;
			serializedObject.Update();
			
			if(selectedNodeIndex < nodes.arraySize)
			{
				SerializedProperty selectedNode = nodes.GetArrayElementAtIndex(selectedNodeIndex);
				DrawNode(selectedNode);
				GUILayout.Space(25);
				DrawTransitions();
				serializedObject.ApplyModifiedProperties();
			}

			Repaint();
		}

		private void DrawTransitions()
		{
			GUILayout.Label("Transitions");
			for(int i = 0; i < graph.transitions.Count; i++)
			{
				DialogueGraphTransition t = graph.transitions[i];
				if(t.from == graph.selectedNode || t.to == graph.selectedNode)
				{
					DrawTransition(t);
					GUILayout.Space(5);
				}
			}
		}

		private void DrawTransition(DialogueGraphTransition transition)
		{

			string from = graph.nodes[transition.from].name;
			string to = "Exit";
			if(transition.to >= 0)
			{
				to = graph.nodes[transition.to].name;
			}

			Rect rect = EditorGUILayout.BeginHorizontal();
			GUI.Box(rect, GUIContent.none);
			GUILayout.Label($"{from} -> {to}");

			if(GUILayout.Button("Remove"))
			{
				graph.transitions.Remove(transition);
			}

			EditorGUILayout.EndHorizontal();
		}

		private void DrawNode(SerializedProperty node)
		{
			SerializedProperty name = node.FindPropertyRelative("name");
			EditorGUILayout.PropertyField(name);

			GUILayout.Space(10);

			SerializedProperty body = node.FindPropertyRelative("body");
			GUILayout.Label("Body");
			body.stringValue = GUILayout.TextArea(body.stringValue, GUILayout.Height(100));

		}
	}
}