using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Animation
{
    [RequireComponent(typeof(Animator))]
    public class Animator2D : MonoBehaviour
    {
        [SerializeField] private AnimationClip defaultAnimation;

        private AnimatorOverrideController controller;
        private bool isLocked;
        private AnimationClip prev;

        private void Awake()
        {
            Animator animator = GetComponent<Animator>();
            controller = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = controller;
            if (defaultAnimation != null) PlayAnimation(defaultAnimation, true);
        }

        public void PlayAnimation(AnimationClip animation, bool looping, bool reset = false)
        {
            if (isLocked && looping) return;

            if (!looping)
            {
                StartCoroutine(LockAnimation(animation, reset));
            }
            else
            {
                prev = animation;
            }
            
            controller["Main"] = animation;
        }

        private IEnumerator LockAnimation(AnimationClip animation, bool reset)
        {
            isLocked = true;
            yield return new WaitForSeconds(animation.length);
            if (reset) controller["Main"] = prev;
            isLocked = false;
        }
    }
}
