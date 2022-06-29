using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    public class GetMoveDirectionNode : IBehaviourTreeNode
    {
        private const string PROP_DIRECTION_OUTPUT = "direction-output";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.AddOutputProperty(PROP_DIRECTION_OUTPUT);
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;
            Rigidbody2D rigidbody2D = ((Agent)obj).Rigidbody2D;

            Vector2 dir = rigidbody2D.velocity.normalized;
            string dest = behaviour.GetProperty(instance, PROP_DIRECTION_OUTPUT).GetString();
            obj.SetProperty(dest, dir);
            
            return NodeStatus.Success;
        }
    }
}
