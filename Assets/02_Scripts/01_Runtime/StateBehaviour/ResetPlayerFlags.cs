using MinD.Utility;
using UnityEngine;


namespace MinD.Runtime.Entity {

public class ResetPlayerFlags : StateMachineBehaviour {

    [Header("[ Flags Set Value ]")]
    [SerializeField] private bool isPerformingAction = false;

    [SerializeField] private bool applyRootMotion = false;
    [SerializeField] private bool canRotate = true;
    [SerializeField] private bool canMove = true;
    
    [SerializeField] private bool colliderEnabled = true;
    
    [Header("[ Other Flags ]")]
    [SerializeField] private bool resetJumpingFlag = false;

    private Player owner;
    
    

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        if (owner == null) {
            owner = animator.GetComponent<Player>();
        }

        animator.applyRootMotion = applyRootMotion;

        owner.isPerformingAction = isPerformingAction;
        owner.canRotate = canRotate;
        owner.canMove = canMove;


        if (resetJumpingFlag) {
            owner.locomotion.isJumping = false;
        }
        
        PhysicUtility.SetActiveChildrenColliders(owner.transform, colliderEnabled, WorldUtility.damageableLayerMask);
    }
}

}