using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree 
{
    public class SimpleMoveNode : IBehaviourTreeNode
    {
        private const float MIN_DISTANCE = 0.5f;
        private const string PROP_POSITION_SRC = "position-source";
        private const string PROP_MOVE_SPEED = "move-speed";

        private Behaviour behaviour;

        public void Init(Behaviour behaviour)
        {
            this.behaviour = behaviour;
            behaviour.Properties.Add(PROP_POSITION_SRC, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_MOVE_SPEED, new VariableProperty(VariableProperty.Type.Number));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, Agent agent)
        {
            string src = behaviour.Properties[PROP_POSITION_SRC].GetString();
            if(agent.HasProperty(src))
            {
                Vector2 targetPosition = (Vector2)agent.GetProperty(src);
                Vector2 currentPosition = agent.transform.position;
                Vector2 d = currentPosition - targetPosition;
                if(d.sqrMagnitude > MIN_DISTANCE * MIN_DISTANCE)
                {
                    float speed = (float)behaviour.Properties[PROP_MOVE_SPEED].GetNumber();
                    agent.transform.position = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);
                    return NodeStatus.Running;
                } else return NodeStatus.Success;
            } else return NodeStatus.Failure;
        }
    }
}