using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace Behaviours
{
    public class FindObjectByIdNode : IBehaviourTreeNode
    {
        private const string PROP_UNIQUE_ID = "unique-id";
        private const string PROP_OBJECT_OUTPUT = "object-output";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.AddProperty(PROP_UNIQUE_ID, new VariableProperty(VariableProperty.Type.String));
            behaviour.AddOutputProperty(PROP_OBJECT_OUTPUT);
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;

            // Find object by unique ID
            string id = behaviour.GetProperty(instance, PROP_UNIQUE_ID).GetString();
            GameObject target = UniqueID.Get(id);
            if (target == null)
            {
                Debug.LogError("Failed to find GameObject with ID: " + id);
                return NodeStatus.Failure;
            }

            // Save target object to destination property
            string dest = behaviour.GetProperty(instance, PROP_OBJECT_OUTPUT).GetString();
            obj.SetProperty(dest, target);
            return NodeStatus.Success;
        }
    }
}