using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class SetAnimParamNode : IBehaviourTreeNode
    {
        private const string PROP_PARAM_NAME = "parameter-name";
        private const string PROP_PARAM_TYPE = "parameter-type"; // TODO: make enum
        private const string PROP_VAL_SRC = "value-source";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_PARAM_NAME, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_PARAM_TYPE, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_VAL_SRC, new VariableProperty(VariableProperty.Type.String));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;
            Animator animator = ((Agent)obj).Animator;

            string paramName = behaviour.GetProperty(PROP_PARAM_NAME).GetString();
            string paramType = behaviour.GetProperty(PROP_PARAM_TYPE).GetString();
            string valSrc = behaviour.GetProperty(PROP_VAL_SRC).GetString();
            object val = obj.GetProperty(valSrc);
            switch (paramType)
            {
                case "Float":
                    double fVal = (double)val;
                    animator.SetFloat(paramName, (float)fVal);
                    break;
                case "Int":
                    double iVal = (double)val;
                    animator.SetInteger(paramName, (int)iVal);
                    break;
                case "Bool":
                    animator.SetBool(paramName, (bool)val);
                    break;
                case "Trigger":
                    animator.SetTrigger(paramName);
                    break;
                default:
                    Debug.LogWarning("Attempted to set invalid AnimParam type!");
                    break;
            }

            return NodeStatus.Success;
        }
    }
}