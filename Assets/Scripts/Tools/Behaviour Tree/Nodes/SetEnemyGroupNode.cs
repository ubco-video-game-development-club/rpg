using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace Behaviours
{
    public class SetEnemyGroupNode : IBehaviourTreeNode
    {
        private const string PROP_GROUP_TAG = "group-tag";
        private const string PROP_IS_ENEMY = "is-enemy";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.AddProperty(PROP_GROUP_TAG, new VariableProperty(VariableProperty.Type.String));
            behaviour.AddProperty(PROP_IS_ENEMY, new VariableProperty(VariableProperty.Type.Boolean));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;

            // Get NPC group
            string groupTag = behaviour.GetProperty(instance, PROP_GROUP_TAG).GetString();
            bool isEnemy = behaviour.GetProperty(instance, PROP_IS_ENEMY).GetBoolean();

            foreach (GameObject npcObj in GameObject.FindGameObjectsWithTag(groupTag))
            {
                // Set enemy status
                if (npcObj.TryGetComponent<NPC>(out NPC npc))
                {
                    npc.SetEnemy(isEnemy);
                }
            }

            return NodeStatus.Success;
        }
    }
}
