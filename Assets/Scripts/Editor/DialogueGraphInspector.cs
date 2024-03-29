using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dialogue
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

            if (selectedNodeIndex >= 0 && selectedNodeIndex < nodes.arraySize)
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
            for (int i = 0; i < graph.transitions.Count; i++)
            {
                DialogueGraphTransition t = graph.transitions[i];
                if (t.from == graph.selectedNode || t.to == graph.selectedNode)
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
            if (transition.to >= 0)
            {
                to = graph.nodes[transition.to].name;
            }

            float width = Screen.width * 0.3f; //75% of available width is for text; ~40% of that space is for each word. Thus, 30% is for each word.
            GUIStyle style = GUI.skin.label;
            from = EditorUtils.TrimStringToFit(from, width, style);
            to = EditorUtils.TrimStringToFit(to, width, style);

            Rect rect = EditorGUILayout.BeginHorizontal();
            GUI.Box(rect, GUIContent.none);

            GUILayout.Label($"{from} -> {to}");

            if (GUILayout.Button("Remove"))
            {
                graph.transitions.Remove(transition);
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawNode(SerializedProperty node)
        {
            // Don't show "option" text on entry node
            if (graph.selectedNode > 0)
            {
                SerializedProperty displayName = node.FindPropertyRelative("displayName");
                GUILayout.Space(15);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Name: ");
                displayName.stringValue = GUILayout.TextArea(displayName.stringValue);
                GUILayout.EndHorizontal();

                SerializedProperty name = node.FindPropertyRelative("name");
                GUILayout.Label("Option");
                name.stringValue = GUILayout.TextArea(name.stringValue, GUILayout.Height(100));
            }

            GUILayout.Space(10);

            SerializedProperty body = node.FindPropertyRelative("body");
            GUILayout.Label("Response");
            body.stringValue = GUILayout.TextArea(body.stringValue, GUILayout.Height(100));

            GUILayout.Space(10);

            // We don't allow alignment mods on entry nodes
            if (graph.selectedNode > 0)
            {
                SerializedProperty moralsMod = node.FindPropertyRelative("moralsMod");
                EditorGUILayout.PropertyField(moralsMod);
                SerializedProperty leaningsMod = node.FindPropertyRelative("leaningsMod");
                EditorGUILayout.PropertyField(leaningsMod);
                SerializedProperty sexinessMod = node.FindPropertyRelative("sexinessMod");
                EditorGUILayout.PropertyField(sexinessMod);

                GUILayout.Space(10);
            }

            SerializedProperty questNotes = node.FindPropertyRelative("questNotes");
            EditorGUILayout.PropertyField(questNotes);

            GUILayout.Space(10);

            SerializedProperty dialogueIndexOverrides = node.FindPropertyRelative("dialogueIndexOverrides");
            EditorGUILayout.PropertyField(dialogueIndexOverrides);

            GUILayout.Space(10);

            SerializedProperty customBehaviour = node.FindPropertyRelative("customBehaviour");
            EditorGUILayout.PropertyField(customBehaviour);
        }
    }
}
