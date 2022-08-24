using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace Behaviours
{
    // TODO: turn this into HasFactionNode later!
    public class IsEnemyNode : IBehaviourTreeNode
    {
        private const string PROP_NPC_INPUT = "npc-input";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.AddInputProperty(PROP_NPC_INPUT);
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;
            string npcSrc = behaviour.GetProperty(instance, PROP_NPC_INPUT).GetString();
            GameObject npc = obj.GetProperty(npcSrc) as GameObject;

            if (npc.GetComponent<NPC>().IsEnemy())
            {
                Debug.Log("IsEnemy True");
            }

            return npc.GetComponent<NPC>().IsEnemy() ? NodeStatus.Success : NodeStatus.Failure;
        }
    }
}
