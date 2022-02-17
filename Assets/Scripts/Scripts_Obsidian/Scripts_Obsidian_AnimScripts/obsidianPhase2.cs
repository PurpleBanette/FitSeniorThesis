using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obsidianPhase2 : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAiObsidian bossReference = animator.GetComponent<bossAiObsidian>();
        bossReference.bossNavAgent.speed = 0;
        animator.ResetTrigger("axeAttack1");
        animator.ResetTrigger("axeAttack2");
        animator.ResetTrigger("axeAttack3");

        bossReference.HitboxDeactivatedPhase1();
        bossReference.HitboxDeactivatedPhase1Shockwave();
        bossReference.p1WeaponTrail.SetActive(false);
        bossReference.shockwaveP1Hitbox.SetActive(false);
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
        bossReference.randAttack = 4;
        bossReference.phaseChanging = false;
        foreach (var hurtbox in bossReference.hurtboxes)
        {
            hurtbox.SetActive(true);
        }
        bossReference.bossNavAgent.speed = bossReference.bossMoveSpeedP2;
    }
}