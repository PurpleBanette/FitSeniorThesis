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
        
    }
        
}
