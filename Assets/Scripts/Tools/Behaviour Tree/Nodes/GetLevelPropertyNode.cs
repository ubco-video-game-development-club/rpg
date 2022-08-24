using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace Behaviours
{
    public class GetLevelPropertyNode : IBehaviourTreeNode
    {
        private const string LEVEL_PROP = "level-property-name";
        private const string PROP_OUTPUT = "behaviour-property-output";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.AddProperty(LEVEL_PROP, new VariableProperty(VariableProperty.Type.String));
            behaviour.AddOutputProperty(PROP_OUTPUT);
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;

            string levelProp = behaviour.GetProperty(instance, LEVEL_PROP).GetString();
            object levelValue = LevelManager.GetLevelProperty(levelProp);

            if (levelValue != null)
            {
                string propOutput = behaviour.GetProperty(instance, PROP_OUTPUT).GetString();
                obj.SetProperty(propOutput, levelValue);
            }

            return NodeStatus.Success;
        }
    }
}
