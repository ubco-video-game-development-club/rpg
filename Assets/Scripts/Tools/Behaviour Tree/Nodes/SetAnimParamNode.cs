using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public enum AnimationParamType
    {
        Int,
        Float,
        Bool,
        Trigger
    }

    public class SetAnimParamNode : IBehaviourTreeNode
    {
        private const string PROP_PARAM_NAME = "parameter-name";
        private const string PROP_PARAM_TYPE = "parameter-type";
        private const string PROP_VAL_SRC = "value-source";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_PARAM_NAME, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_PARAM_TYPE, new VariableProperty(VariableProperty.Type.Enum, typeof(AnimationParamType)));
            behaviour.Properties.Add(PROP_VAL_SRC, new VariableProperty(VariableProperty.Type.String));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;
            Animator animator = ((Agent)obj).Animator;

            string paramName = behaviour.GetProperty(PROP_PARAM_NAME).GetString();
            AnimationParamType paramType = behaviour.GetProperty(PROP_PARAM_TYPE).GetEnum<AnimationParamType>();
            string valSrc = behaviour.GetProperty(PROP_VAL_SRC).GetString();
            object val = obj.GetProperty(valSrc);
            switch (paramType)
            {
                case AnimationParamType.Float:
                    double fVal = (double)val;
                    animator.SetFloat(paramName, (float)fVal);
                    break;
                case AnimationParamType.Int:
                    int iVal = Mathf.RoundToInt((float)(double)val);
                    animator.SetInteger(paramName, iVal);
                    break;
                case AnimationParamType.Bool:
                    animator.SetBool(paramName, (bool)val);
                    break;
                case AnimationParamType.Trigger:
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