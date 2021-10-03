using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class VectorIteratorNode : IBehaviourTreeNode
    {
        private const string PROP_ARRAY = "source-array";
        private const string PROP_INDEX_SRC = "index-source";
        private const string PROP_VECTOR_DEST = "vector-destination";

        public void Init(Behaviour behaviour)
        {
            // TODO: have an array property to drive these values
            behaviour.Properties.Add(PROP_ARRAY, new VariableProperty(VariableProperty.Type.String)); // todo: change type to array (of vectors)
            behaviour.Properties.Add(PROP_INDEX_SRC, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_VECTOR_DEST, new VariableProperty(VariableProperty.Type.String));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;

            Vector2[] tempArr = {
                new Vector2(-3, -3),
                new Vector2(-5, -5),
                new Vector2(-3, -5)
            };

            // we need to output two things: index, and current vector
            // these need to be stored in specific properties (defined in the inspector)

            // 1. read the current index from the agent (or set to zero if not exists)
            int index = 0;
            string indexSrc = behaviour.GetProperty(PROP_INDEX_SRC).GetString();
            if (obj.HasProperty(indexSrc))
            {
                index = (int)obj.GetProperty(indexSrc);
            }

            // 2. get the vector at the current index
            Vector2 vec = tempArr[index];

            // 3. store the resulting vector on the agent
            obj.SetProperty(PROP_VECTOR_DEST, vec);

            return NodeStatus.Success;
        }
    }
}