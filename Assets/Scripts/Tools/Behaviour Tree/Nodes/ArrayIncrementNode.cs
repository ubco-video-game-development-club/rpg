using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    public class ArrayIncrementNode : IBehaviourTreeNode
    {
        // TODO: implement Loop, PingPong, etc. increment wrap types?
        private const string PROP_INDEX_SRC = "index-source";
        private const string PROP_ARR_LEN = "array-length";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_INDEX_SRC, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_ARR_LEN, new VariableProperty(VariableProperty.Type.Number));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;

            // 1. Get the current index from the source
            int index = 0;
            string indexSrc = behaviour.GetProperty(instance, PROP_INDEX_SRC).GetString();
            if (obj.HasProperty(indexSrc))
            {
                index = (int)obj.GetProperty(indexSrc);
            }

            // 2. Increment or loop the index and 
            int arrLen = (int)behaviour.GetProperty(instance, PROP_ARR_LEN).GetNumber();
            index = (index + 1) % arrLen;

            // 3. Save the new index back to the agent
            obj.SetProperty(indexSrc, index);

            return NodeStatus.Success;
        }
    }
}
