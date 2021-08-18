using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

namespace BehaviourTree
{
    public class TestMoverNode : IBehaviourTreeNode
    {
        public void Init(Behaviour behaviour)
        {
            behaviour.SetProperty("noise-smoothing", new VariableProperty(VariableProperty.Type.Number));
            behaviour.SetProperty("move-speed", new VariableProperty(VariableProperty.Type.Number));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, Agent agent)
        {
            Vector2 worldPos = agent.transform.position;
            float noiseSmoothing = (float)self.Element.GetProperty("noise-smoothing").GetNumber();
            float rotation = Mathf.PerlinNoise(worldPos.x / noiseSmoothing, worldPos.y / noiseSmoothing) * 360.0f;

            Vector2 direction = Quaternion.Euler(0, 0, rotation) * Vector2.up;

            float moveSpeed = (float)self.Element.GetProperty("move-speed").GetNumber();
            agent.transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

            return NodeStatus.Running;
        }
    }
}
