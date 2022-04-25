using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obsidianAxeAttack2Flinch : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAiObsidian bossReference = animator.GetComponent<bossAiObsidian>();
        bossReference.bossNavAgent.speed = 0;
        bossReference.bossIsAttacking = true;
        bossReference.HitboxDeactivatedPhase1();
        animator.ResetTrigger("axeAttack2");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAiObsidian bossReference = animator.GetComponent<bossAiObsidian>();
        bossReference.bossNavAgent.speed = bossReference.bossMoveSpeedP1;
        bossReference.randAttack = 4;
        bossReference.bossIsAttacking = false;
        animator.ResetTrigger("attack2flinchP1");
    }

}
