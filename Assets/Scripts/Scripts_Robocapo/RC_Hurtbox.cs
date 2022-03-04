using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RC_Hurtbox : MonoBehaviour
{
    [SerializeField]
    bossAiRobocapo bossAiReference;
    
    int hurtboxDamage = 25;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Weapon")
        {
            //Debug.Log("Ive Been Hit");
            bossAiReference.rcTakeDamage(hurtboxDamage);
        }
    }
}
