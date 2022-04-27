using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace Behaviours
{
    public class FindObjectByIdNode : IBehaviourTreeNode
    {
        private const string PROP_UNIQUE_ID = "unique-id";
        private const string PROP_OBJECT_DEST = "object-destination";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_UNIQUE_ID, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_OBJECT_DEST, new VariableProperty(VariableProperty.Type.String));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;

            // Find object by unique ID
            string id = behaviour.GetProperty(PROP_UNIQUE_ID).GetString();
            GameObject target = UniqueID.Get(id);
            if (target == null)
            {
                Debug.LogError("Failed to find GameObject with ID: " + id);
                return NodeStatus.Failure;
            }

            // Save target object to destination property
            string dest = behaviour.GetProperty(PROP_OBJECT_DEST).GetString();
            obj.SetProperty(dest, target);
            return NodeStatus.Success;
        }
    }
}