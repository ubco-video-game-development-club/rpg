using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class VectorIteratorNode : IBehaviourTreeNode
    {
        private const string PROP_ARRAY = "vector-array";
        private const string PROP_INDEX_SRC = "index-source";
        private const string PROP_VECTOR_DEST = "vector-destination";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_ARRAY, new VariableProperty(VariableProperty.Type.Array, VariableProperty.Type.Vector));
            behaviour.Properties.Add(PROP_INDEX_SRC, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_VECTOR_DEST, new VariableProperty(VariableProperty.Type.String));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;

            // 1. read the current index from the agent (or set to zero if not exists)
            int index = 0;
            string indexSrc = behaviour.GetProperty(PROP_INDEX_SRC).GetString();
            if (obj.HasProperty(indexSrc))
            {
                index = (int)obj.GetProperty(indexSrc);
            }

            // 2. get the vector at the current index
            object[] arr = behaviour.GetProperty(PROP_ARRAY).GetArray();
            Vector2 vec = (Vector2)arr[index];

            // 3. store the resulting vector on the agent
            string vectorDest = behaviour.GetProperty(PROP_VECTOR_DEST).GetString();
            obj.SetProperty(vectorDest, vec);

            return NodeStatus.Success;
        }
    }
}