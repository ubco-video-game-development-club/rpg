using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace Behaviours
{
    public class ForceDialogueNode : IBehaviourTreeNode
    {
        public void Serialize(Behaviour behaviour) { }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            if (obj.TryGetComponent<NPC>(out NPC npc))
            {
                npc.Interact(GameManager.Player);
            }
            else
            {
                throw new MissingComponentException("Current agent does not have the required NPC component!");
            }
            return NodeStatus.Success;
        }
    }
}
