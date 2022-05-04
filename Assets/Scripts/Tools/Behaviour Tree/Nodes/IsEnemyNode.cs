using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace Behaviours
{
    // TODO: turn this into HasFactionNode later!
    public class IsEnemyNode : IBehaviourTreeNode
    {
        private const string PROP_NPC_SRC = "source-npc";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_NPC_SRC, new VariableProperty(VariableProperty.Type.String));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;
            string npcSrc = behaviour.GetProperty(instance, PROP_NPC_SRC).GetString();
            GameObject npc = obj.GetProperty(npcSrc) as GameObject;
            return npc.GetComponent<NPC>().IsEnemy() ? NodeStatus.Success : NodeStatus.Failure;
        }
    }
}
