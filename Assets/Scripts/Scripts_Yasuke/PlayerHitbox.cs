using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    
    void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "EnemyProjectile" && ModifiedTPC.instance.blocking == false || other.transform.tag == "EnemyAttackTriggers" && ModifiedTPC.instance.blocking == false)
        {
            ModifiedTPC.instance.imHit = true;
            ModifiedTPC.instance.playerTakeDamage();
            ModifiedTPC.instance.playerHitTick = true;
        }
        else if(other.transform.tag == "EnemyProjectile" && ModifiedTPC.instance.blocking == true || other.transform.tag == "EnemyAttackTriggers" && ModifiedTPC.instance.blocking == true)
        {
            Debug.Log("successful Block");
            ModifiedTPC.instance.blockParticle.SetActive(true);
            if (GameObject.Find("Robocapo"))
            {
                bossAiRobocapoRemake.instance.bossAnimator.SetTrigger("Stunned");
            }
            if (GameObject.Find("M_BossObsidian"))
            {
                bossAiObsidian.instance.bossAnimator.SetTrigger("attack2flinchP1");
            }
        }
    }
}
