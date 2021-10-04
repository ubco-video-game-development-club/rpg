using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class SimpleMoveNode : IBehaviourTreeNode
    {
        private const string PROP_POSITION_SRC = "position-source";
        private const string PROP_MIN_DISTANCE = "min-distance";
        private const string PROP_MOVE_SPEED = "move-speed";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_POSITION_SRC, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_MIN_DISTANCE, new VariableProperty(VariableProperty.Type.Number));
            behaviour.Properties.Add(PROP_MOVE_SPEED, new VariableProperty(VariableProperty.Type.Number));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;

            string src = behaviour.Properties[PROP_POSITION_SRC].GetString();
            if (obj.HasProperty(src))
            {
                Vector2 targetPosition = (Vector2)obj.GetProperty(src);
                Vector2 currentPosition = obj.transform.position;
                Vector2 d = currentPosition - targetPosition;

                float minDist = (float)behaviour.Properties[PROP_MIN_DISTANCE].GetNumber();
                if (d.sqrMagnitude > minDist * minDist)
                {
                    float speed = (float)behaviour.Properties[PROP_MOVE_SPEED].GetNumber();
                    obj.transform.position = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);
                    return NodeStatus.Running;
                }
                return NodeStatus.Success;
            }
            return NodeStatus.Failure;
        }
    }
}