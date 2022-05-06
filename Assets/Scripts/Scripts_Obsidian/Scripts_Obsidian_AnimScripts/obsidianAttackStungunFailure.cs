using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obsidianAttackStungunFailure : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAiObsidian.instance.damageParticles.SetActive(true);
        bossAiObsidian.instance.bossHealth -= 25;
        bossAiObsidian.instance.hitTick = true;
        hitPause.instance.INevarFreeze();
        bossAiObsidian.instance.obsidianLaserOff();
        bossAiObsidian.instance.Phase2RapidFireStop();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAiObsidian bossReference = animator.GetComponent<bossAiObsidian>();
        bossReference.bossNavAgent.speed = bossAiObsidian.instance.bossMoveSpeedP2;
        bossReference.bossIsAttacking = false;
    }
}
