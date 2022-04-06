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
            charCtrl.playerTakeDamage();
            ModifiedTPC.instance.playerHitTick = true;
        }
        else if(other.transform.tag == "EnemyProjectile" && charCtrl.blocking == true || other.transform.tag == "EnemyAttackTriggers" && charCtrl.blocking == true)
        {
            Debug.Log("successful Block");
            charCtrl.blockParticle.SetActive(true);
            bossAiRobocapo.instance.bossAnimator.SetTrigger("stun");
        }
    }
}
