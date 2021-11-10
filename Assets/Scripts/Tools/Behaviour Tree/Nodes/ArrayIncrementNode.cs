using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    // TODO: split into LoopIncrement and PingPongIncrement nodes?
    public class ArrayIncrementNode : IBehaviourTreeNode
    {
        private const string PROP_INDEX_SRC = "index-source";
        private const string PROP_MAX_INDEX = "max-index";

        public void Init(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_INDEX_SRC, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_MAX_INDEX, new VariableProperty(VariableProperty.Type.Number));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;

            // 1. Get the current index from the source
            string indexSrc = behaviour.GetProperty(PROP_INDEX_SRC).GetString();
            int index = (int)obj.GetProperty(indexSrc);

            // 2. Increment or loop the index and 
            int maxIndex = (int)behaviour.GetProperty(PROP_MAX_INDEX).GetNumber();
            index = (index + 1) % maxIndex;

            // 3. Save the new index back to the agent
            obj.SetProperty(indexSrc, index);

            return NodeStatus.Success;
        }
    }
}
