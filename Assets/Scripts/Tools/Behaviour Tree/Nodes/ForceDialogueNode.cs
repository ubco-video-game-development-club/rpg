using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    public class ForceDialogueNode : IBehaviourTreeNode
    {
        private const string PROP_GAMEOBJECT_SRC = "gameobject-source";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_GAMEOBJECT_SRC, new VariableProperty(VariableProperty.Type.String));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;

            GameObject target = obj.gameObject;
            string src = behaviour.GetProperty(PROP_GAMEOBJECT_SRC).GetString();
            if (obj.HasProperty(src))
            {
                target = obj.GetProperty(src) as GameObject;
            }

            if (target.TryGetComponent<NPC>(out NPC npc))
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
