using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace Behaviours
{
    // TODO: turn this into SetFactionNode later!
    public class SetEnemyNode : IBehaviourTreeNode
    {
        private const string PROP_NPC_INPUT = "npc-input";
        private const string PROP_IS_ENEMY = "is-enemy";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.AddInputProperty(PROP_NPC_INPUT);
            behaviour.AddProperty(PROP_IS_ENEMY, new VariableProperty(VariableProperty.Type.Boolean));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;

            // Get NPC
            string npcSrc = behaviour.GetProperty(instance, PROP_NPC_INPUT).GetString();
            GameObject npc = obj.GetProperty(npcSrc) as GameObject;

            // Set enemy status
            bool isEnemy = behaviour.GetProperty(instance, PROP_IS_ENEMY).GetBoolean();
            npc.GetComponent<NPC>().SetEnemy(isEnemy);

            Debug.Log("Enemy Set");

            return NodeStatus.Success;
        }
    }
}
