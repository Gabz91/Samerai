using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScripts
{
    public class FireballState : StateMachineBehaviour
    {
        float deceleration;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            deceleration = 0.001f;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            deceleration += 0.001f;
            if (animator.transform.rotation.y >= 0)
                animator.transform.Translate(0.1f-deceleration, 0, 0);
            else
                animator.transform.transform.Translate(-0.1f+deceleration, 0, 0);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Destroy(animator.gameObject);
        }

        // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}
    }
}