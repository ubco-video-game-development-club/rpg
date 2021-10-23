using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Animation
{
    [RequireComponent(typeof(Animator))]
    public class Animator2D : MonoBehaviour
    {
        private AnimatorOverrideController controller;
        private bool isLocked;

        private void Awake()
        {
            Animator animator = GetComponent<Animator>();
            controller = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = controller;
        }

        public void PlayAnimation(AnimationClip animation, bool allowInterrupt)
        {
            if (!isLocked)
            {
                controller["Main"] = animation;
                if (!allowInterrupt) StartCoroutine(LockAnimation(animation));
            }
        }

        private IEnumerator LockAnimation(AnimationClip animation)
        {
            isLocked = true;
            yield return new WaitForSeconds(animation.length);
            isLocked = false;
        }
    }
}
