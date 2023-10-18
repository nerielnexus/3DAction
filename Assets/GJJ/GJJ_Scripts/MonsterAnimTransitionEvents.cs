using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJJ
{
    public class MonsterAnimTransitionEvents : StateMachineBehaviour
    {
        private void AfterShootGuns(Animator animator)
        {
            animator.SetBool("AnimGun", false);
            animator.SetBool("AnimIdle", true);
        }

        private void AfterRun(Animator animator)
        {
            animator.SetBool("AnimRun", false);
            animator.SetBool("AnimIdle", true);
        }

        private void EnterRun(Animator animator)
        {
            Debug.LogWarning("AnimRun Entered");
            
        }
        // ====================================================================

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.IsName("Run"))
                EnterRun(animator);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
        }

        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
        }

        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.IsName("shoots gun_2"))
                AfterShootGuns(animator);
            else
                Debug.LogWarning("shoot gun name wrong");

            /*
            if (stateInfo.IsName("Run"))
                AfterRun(animator);
            else
                Debug.LogWarning("run name wrong");
            */
        }
    }

}