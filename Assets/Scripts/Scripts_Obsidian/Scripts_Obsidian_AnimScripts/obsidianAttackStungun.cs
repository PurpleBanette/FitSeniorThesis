using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obsidianAttackStungun : StateMachineBehaviour
{

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAiObsidian bossReference = animator.GetComponent<bossAiObsidian>();
        bossReference.bossNavAgent.speed = 0;
        bossReference.bossIsAttacking = true;
        bossReference.Phase2RapidFireStop();
        bossReference.HitboxDeactivatedPhase2();
        bossReference.playerTracking = false;
        bossReference.obsidianLaserOff();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
