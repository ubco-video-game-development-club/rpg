using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    // TODO: turn this into SetFactionNode later!
    public class SetEnemyNode : IBehaviourTreeNode
    {
        private const string PROP_NPC_SRC = "source-npc";
        private const string PROP_IS_ENEMY = "is-enemy";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_NPC_SRC, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_IS_ENEMY, new VariableProperty(VariableProperty.Type.Boolean));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;

            Debug.Log("SETTING ENEMY!");

            // Get NPC
            string npcSrc = behaviour.GetProperty(PROP_NPC_SRC).GetString();
            GameObject npc = obj.GetProperty(npcSrc) as GameObject;

            // Set enemy status
            bool isEnemy = behaviour.GetProperty(PROP_IS_ENEMY).GetBoolean();
            npc.GetComponent<NPC>().SetEnemy(isEnemy);

            return NodeStatus.Success;
        }
    }
}
