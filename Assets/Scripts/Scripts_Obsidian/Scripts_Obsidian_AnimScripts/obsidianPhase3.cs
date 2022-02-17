using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obsidianPhase3 : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAiObsidian bossReference = animator.GetComponent<bossAiObsidian>();
        bossReference.bossNavAgent.speed = 0;
        animator.ResetTrigger("railgunAttack1");
        animator.ResetTrigger("railgunAttack2");
        animator.ResetTrigger("railgunAttack3");

        bossReference.HitboxDeactivatedPhase2();
        bossReference.phase2Laser.SetActive(false);
        bossReference.playerTracking = false;
        bossReference.bossIsAttacking = false;
        bossReference.phaseChanging = true;
        foreach (var hurtbox in bossReference.hurtboxes)
        {
            hurtbox.SetActive(false);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAiObsidian bossReference = animator.GetComponent<bossAiObsidian>();
        bossReference.bossNavAgent.speed = 0;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAiObsidian bossReference = animator.GetComponent<bossAiObsidian>();
        bossReference.phaseChanging = false;
        foreach (var hurtbox in bossReference.hurtboxes)
        {
            hurtbox.SetActive(true);
        }
        animator.ResetTrigger("gauntletAttack1");
        animator.ResetTrigger("gauntletAttack2");
        animator.ResetTrigger("gauntletAttack3");
        animator.ResetTrigger("gauntletAttack4");
        animator.ResetTrigger("gauntletAttack5");
        animator.ResetTrigger("gauntletAttack6");
        bossReference.bossNavAgent.speed = bossReference.bossMoveSpeedP3;
    }
    
}
