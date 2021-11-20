using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossTakeDamage : MonoBehaviour
{
    [SerializeField] 
    bossAiObsidian bossAiReference;
    [SerializeField]
    int hurtboxDamage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Weapon")
        {
            bossAiReference.bossHealth -= hurtboxDamage;
        }
    }

}
