using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obsidianDeath : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAiObsidian bossReference = animator.GetComponent<bossAiObsidian>();
        bossWeaponLocator.instance.weaponMarker.enabled = true;
        bossReference.bossNavAgent.speed = 0;
        bossReference.bossNavAgent.acceleration = 1000;
        bossReference.phaseChanging = true;
        bossReference.bossIsAttacking = false;
        bossReference.playerTracking = false;

        bossReference.chargeHitboxPhase3.SetActive(false);
        bossReference.jumpHitboxPhase3.SetActive(false);

        animator.ResetTrigger("gauntletAttack1");
        animator.ResetTrigger("gauntletAttack2");
        animator.ResetTrigger("gauntletAttack3");
        animator.ResetTrigger("gauntletAttack4");
        animator.ResetTrigger("gauntletAttack5");
        animator.ResetTrigger("gauntletAttack6");

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
        bossReference.bossNavAgent.speed = 0;
    }
}