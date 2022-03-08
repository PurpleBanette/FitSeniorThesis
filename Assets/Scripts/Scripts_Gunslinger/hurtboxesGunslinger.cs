using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hurtboxesGunslinger : MonoBehaviour
{
    [SerializeField] int hurtboxDamage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Weapon")
        {
            bossAiGunslinger.instance.bossHealth -= hurtboxDamage;
            bossAiGunslinger.instance.hitTick = true;
            bossAiGunslinger.instance.bossHealthbar.value = bossAiGunslinger.instance.bossHealth;
        }
    }
}
