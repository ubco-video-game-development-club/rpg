using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace Behaviours
{
    public class SetLevelPropertyNode : IBehaviourTreeNode
    {
        private const string PROP_INPUT = "behaviour-property-input";
        private const string LEVEL_PROP = "level-property-name";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.AddInputProperty(PROP_INPUT);
            behaviour.AddProperty(LEVEL_PROP, new VariableProperty(VariableProperty.Type.String));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;

            string propInput = behaviour.GetProperty(instance, PROP_INPUT).GetString();
            object propValue = obj.GetProperty(propInput);

            string levelProp = behaviour.GetProperty(instance, LEVEL_PROP).GetString();
            LevelManager.SetLevelProperty(levelProp, propValue);

            return NodeStatus.Success;
        }
    }
}
