using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScripts
{
    public class Attack1State : StateMachineBehaviour
    {

        // This will be called when the animator first transitions to this state.
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            AudioManager.Instance.PlaySound("attack");
        }


        // This will be called once the animator has transitioned out of the state.
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            PlayerController.isAttacking = false;
            if (PlayerController.noOfClicks == 2)
            {
                animator.Play("attack2");
            }
            else
            {
                PlayerController.noOfClicks = 0;
            }
        }

        // This will be called every frame whilst in the state.
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // OnStateExit may be called before the last OnStateUpdate so we need to check the particles haven't been destroyed.
        }

    }
}

