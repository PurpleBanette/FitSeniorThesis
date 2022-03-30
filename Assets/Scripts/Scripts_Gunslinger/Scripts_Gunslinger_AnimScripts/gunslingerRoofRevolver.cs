using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunslingerRoofRevolver : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAiGunslinger bossReference = animator.GetComponent<bossAiGunslinger>();
        bossReference.bossNavAgent.speed = 0;
        bossReference.bossIsAttacking = true;
        bossReference.BossAttackPlayer();
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAiGunslinger bossReference = animator.GetComponent<bossAiGunslinger>();
        bossReference.bossNavAgent.speed = bossReference.bossMoveSpeedP1;
        bossReference.bossIsAttacking = false;
        bossReference.bossAnimator.SetBool("roofRevolverBool", false);
    }
}
