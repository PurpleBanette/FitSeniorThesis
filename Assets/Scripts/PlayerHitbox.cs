using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    [SerializeField]
    ModifiedTPC charCtrl;
    void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "EnemyProjectile" && charCtrl.blocking == false)
        {
            charCtrl.playerTakeDamage();
        }
        else if(other.transform.tag == "EnemyProjectile" && charCtrl.blocking == true)
        {
            Debug.Log("successful Block");
            bossAiRobocapo.instance.bossAnimator.SetTrigger("stun");
        }
        
    }
        
}
