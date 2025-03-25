using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using MinD;
using UnityEngine;

namespace MinD.Runtime.Entity {

public class ResetEnemyFlags : StateMachineBehaviour {

    [Header("[ Flags Set Value ]")]
    [SerializeField] private bool applyRootMotion = true;

    [SerializeField] private bool isPerformingAction = false;


    [HideInInspector] public Enemy owner;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        if (owner == null)
            owner = animator.GetComponent<Enemy>();

        animator.applyRootMotion = applyRootMotion;

        owner.isPerformingAction = isPerformingAction;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}

}