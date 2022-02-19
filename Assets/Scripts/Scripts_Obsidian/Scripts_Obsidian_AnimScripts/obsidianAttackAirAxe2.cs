using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obsidianAttackAirAxe2 : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAiObsidian bossReference = animator.GetComponent<bossAiObsidian>();
        /*bossReference.bossNavAgent.speed = bossReference.bossMoveSpeedP1;
        Vector3 bossPosition = bossReference.transform.position;
        Vector3 playerPosition = bossReference.player.transform.position;
        bossReference.navMeshLinkScript.startPoint = bossPosition;
        bossReference.navMeshLinkScript.endPoint = playerPosition;
        bossReference.bossNavAgent.speed = bossReference.bossMoveSpeedP1;
        bossReference.jumpAttack = true;*/
        bossReference.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        bossReference.GetComponent<Rigidbody>().isKinematic = false;
        bossReference.playerTracking = false;
        Vector3 horizontalDirection = bossReference.playerTarget.transform.position - bossReference.transform.position;
        bossReference.GetComponent<Rigidbody>().AddForce(horizontalDirection.normalized * 75, ForceMode.Impulse);
        bossReference.GetComponent<Rigidbody>().AddForce(0, 3500, 0);

        
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

}
