using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class GetBehaviourObjectNode : IBehaviourTreeNode
    {
        private const string PROP_GAMEOBJECT_SRC = "gameobject-source";
        private const string PROP_BEHAVIOUR_OBJ_DEST = "behaviour-object-destination";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_GAMEOBJECT_SRC, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_BEHAVIOUR_OBJ_DEST, new VariableProperty(VariableProperty.Type.String));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;

            string src = behaviour.GetProperty(PROP_GAMEOBJECT_SRC).GetString();
            if (obj.HasProperty(src))
            {
                string dest = behaviour.GetProperty(PROP_BEHAVIOUR_OBJ_DEST).GetString();
                GameObject target = obj.GetProperty(src) as GameObject;
                BehaviourObject behaviourObject = target.GetComponent<BehaviourObject>();
                obj.SetProperty(dest, behaviourObject);
                return NodeStatus.Success;
            }
            return NodeStatus.Failure;
        }
    }
}