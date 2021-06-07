using UnityEngine;

/*
    Used in conjunction with the Player script to clear the current override
    controller when exiting a temporary animation state machine.
*/
namespace RPG
{
    public class RestoreParameters : StateMachineBehaviour
    {
        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            animator.GetComponent<Player>().ClearAnimationOverrides();
        }
    }
}
