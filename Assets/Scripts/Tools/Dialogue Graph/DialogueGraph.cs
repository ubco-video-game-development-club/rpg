using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "Dialogue Graph", menuName = "Dialogue Graph", order = 70)]
    public class DialogueGraph : ScriptableObject
    {
        public const float LINE_WIDTH = 5.0f;
        public static readonly Color lineColour = new Color(238.0f / 255.0f, 185.0f / 255.0f, 162.0f / 255.0f);

        private static readonly Color entryColour = new Color(192.0f / 255.0f, 216.0f / 255.0f, 227.0f / 255.0f);
        private static readonly Color exitColour = new Color(225.0f / 255.0f, 138.0f / 255.0f, 122.0f / 255.0f);
        private static readonly Color selectedColour = new Color(238.0f / 255.0f, 185.0f / 255.0f, 162.0f / 255.0f);

        public List<DialogueGraphNode> nodes;
        public List<DialogueGraphTransition> transitions;
        [System.NonSerialized] public readonly DialogueGraphNode exitNode = new DialogueGraphNode("Exit", new Vector2(200, 0));
        [System.NonSerialized] public int selectedNode;

        public void Draw(Vector2 offset)
        {
            DrawTransitions(offset);
            DrawNodes(offset);
        }

        public void ProcessEvents(Event e)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].ProcessEvents(e))
                {
                    selectedNode = i;
                }
            }

            exitNode.ProcessEvents(e);
        }

        public void CreateNode(string name, Vector2 position)
        {
            nodes.Add(new DialogueGraphNode(name, position));
            GUI.changed = true;
        }

        public DialogueGraphNode GetNodeAt(Vector2 position)
        {
            if (exitNode.Contains(position)) return exitNode;

            foreach (DialogueGraphNode node in nodes)
            {
                if (node.Contains(position))
                {
                    return node;
                }
            }

            return null;
        }

        public void RemoveNode(DialogueGraphNode node)
        {
            int index = nodes.IndexOf(node);
            nodes.Remove(node);

            for (int i = 0; i < transitions.Count; i++)
            {
                DialogueGraphTransition transition = transitions[i];
                if (transition.from == index || transition.to == index)
                {
                    transitions.Remove(transition);
                    i--;
                }
            }

            GUI.changed = true;
        }

        private void DrawNodes(Vector2 offset)
        {
            //Draw entry node
            GUI.color = entryColour;
            nodes[0].Draw(offset);
            GUI.color = Color.white;

            //Draw other nodes
            for (int i = 1; i < nodes.Count; i++)
            {
                if (i == selectedNode) continue;
                nodes[i].Draw(offset);
            }

            //Draw exit node
            GUI.color = exitColour;
            exitNode.Draw(offset);

            //Draw selected node
            if (selectedNode >= 0 && selectedNode < nodes.Count)
            {
                GUI.color = selectedColour;
                nodes[selectedNode].Draw(offset);
                GUI.color = Color.white;
            }
        }

        private void DrawTransitions(Vector2 offset)
        {
            foreach (DialogueGraphTransition transition in transitions)
            {
                Vector2 from = nodes[transition.from].position + offset;

                Vector2 to;
                if (transition.to < 0)
                {
                    to = exitNode.position + offset;
                }
                else
                {
                    to = nodes[transition.to].position + offset;
                }

                Handles.DrawBezier(from, to, from, to, lineColour, null, LINE_WIDTH);
            }
        }
    }
}
