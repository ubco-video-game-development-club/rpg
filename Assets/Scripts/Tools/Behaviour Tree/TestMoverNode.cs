using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class TestMoverNode : IBehaviourTreeNode
{
    public void Init(BehaviourTreeNode self)
    {
        self.AddProperty("noise-smoothing", new VariableProperty(VariableProperty.Type.Number));
        self.AddProperty("move-speed", new VariableProperty(VariableProperty.Type.Number));
    }

    public NodeStatus Tick(Tree<BehaviourTreeNode>.Node self, Agent agent)
    {
        Vector2 worldPos = agent.transform.position;
        float noiseSmoothing = (float)self.Element.GetProperty("noise-smoothing").GetNumber();
        float rotation = Mathf.PerlinNoise(worldPos.x / noiseSmoothing, worldPos.y / noiseSmoothing);

        Vector2 direction = Quaternion.Euler(0, 0, rotation) * Vector2.up;

        float moveSpeed = (float)self.Element.GetProperty("move-speed").GetNumber();
        agent.transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

        return NodeStatus.Running;
    }
}
