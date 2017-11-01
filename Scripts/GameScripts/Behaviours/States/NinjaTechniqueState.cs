using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScripts
{
    public class NinjaTechniqueState : StateMachineBehaviour
    {
        float timer;
        bool fireballTrigger;
        GameObject fireball;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            timer = Time.time;
            fireballTrigger = true;
            PlayerController.isUsingTech = true;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.length - (Time.time - timer) <= 0.4f && fireballTrigger)
            {
                fireball = Instantiate(ResourceManager.Instance.fireballPrefab);
                fireball.GetComponent<Animator>().Play("fireball");
                fireball.name = "Fireball";
                fireball.transform.position = ResourceManager.Instance.playerTrans.position;
                fireball.transform.right = ResourceManager.Instance.playerTrans.right;
                fireballTrigger = false;
            }
        }


        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            PlayerController.isUsingTech = false;
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