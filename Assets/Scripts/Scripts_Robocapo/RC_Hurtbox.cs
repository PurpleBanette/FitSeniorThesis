using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RC_Hurtbox : MonoBehaviour
{
    int hurtboxDamage = 25;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Weapon")
        {
            Debug.Log("Ive Been Hit");
            bossAiRobocapoRemake.instance.damageParticles.SetActive(true);
            hitPause.instance.INevarFreeze();
            ModifiedTPC.instance.disableWeapon();
            bossAiRobocapoRemake.instance.TakeDamage();
            bossAiRobocapoRemake.instance.hitTick = true;
            GetComponentInParent<bossColorOverride>().colorFade = 1;
        }
    }
}
