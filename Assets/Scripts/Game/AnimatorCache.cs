using UnityEngine;

/*
    Unity's animation system clears parameters when changing between override controllers.
    This script enables parameter state caching for seamless controller transitions.
*/
namespace RPG
{
    public class AnimatorCache : MonoBehaviour
    {
        private AnimatorParameter[] parameterCache;
        private Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void CacheParameters()
        {
            parameterCache = new AnimatorParameter[animator.parameters.Length];
            for (int i = 0; i < animator.parameters.Length; i++)
            {
                AnimatorControllerParameter paramInfo = animator.parameters[i];
                parameterCache[i] = new AnimatorParameter(animator, paramInfo.type, paramInfo.name);
            }
        }

        public void RestoreParameters()
        {
            foreach (AnimatorParameter param in parameterCache)
            {
                switch (param.ParamType)
                {
                    case AnimatorControllerParameterType.Int:
                        animator.SetInteger(param.Name, (int)param.Value);
                        break;
                    case AnimatorControllerParameterType.Float:
                        animator.SetFloat(param.Name, (float)param.Value);
                        break;
                    case AnimatorControllerParameterType.Bool:
                        animator.SetBool(param.Name, (bool)param.Value);
                        break;
                }
            }
        }

        private class AnimatorParameter
        {
            public AnimatorControllerParameterType ParamType { get; private set; }
            public string Name { get; private set; }
            public object Value { get; private set; }

            public AnimatorParameter(Animator animator, AnimatorControllerParameterType paramType, string name)
            {
                ParamType = paramType;
                Name = name;
                switch (paramType)
                {
                    case AnimatorControllerParameterType.Int:
                        Value = (int)animator.GetInteger(name);
                        break;
                    case AnimatorControllerParameterType.Float:
                        Value = (float)animator.GetFloat(name);
                        break;
                    case AnimatorControllerParameterType.Bool:
                        Value = (bool)animator.GetBool(name);
                        break;
                }
            }
        }
    }
}
