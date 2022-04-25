using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    [SerializeField]
    ModifiedTPC charCtrl;
    void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "EnemyProjectile" && charCtrl.blocking == false || other.transform.tag == "EnemyAttackTriggers" && charCtrl.blocking == false)
        {
            charCtrl.imHit = true;
            charCtrl.playerTakeDamage();
            ModifiedTPC.instance.playerHitTick = true;
        }
        else if(other.transform.tag == "EnemyProjectile" && charCtrl.blocking == true || other.transform.tag == "EnemyAttackTriggers" && charCtrl.blocking == true)
        {
            Debug.Log("successful Block");
            charCtrl.blockParticle.SetActive(true);
            if (GameObject.Find("Robocapo"))
            {
                bossAiRobocapoRemake.instance.bossAnimator.SetTrigger("Stunned");
            }
            if (GameObject.Find("Obsidian"))
            {
                bossAiObsidian.instance.bossAnimator.SetTrigger("attack2flinchP1");
            }
        }
    }
}
