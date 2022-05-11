using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RC_Hurtbox : MonoBehaviour
{
    int hurtboxDamage = 34;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Weapon" && !bossAiRobocapoRemake.instance.GuardUp)
        {
            bossAiRobocapoRemake.instance.damageParticles.SetActive(true);
            hitPause.instance.INevarFreeze();
            ModifiedTPC.instance.disableWeapon();
            bossAiRobocapoRemake.instance.TakeDamage();
            bossAiRobocapoRemake.instance.hitTick = true;
            GetComponentInParent<bossColorOverride>().colorFade = 1;
        }
        else if (other.transform.tag == "Weapon" && bossAiRobocapoRemake.instance.GuardUp)
        {
            Debug.Log("Blocked Hit");
        }
    }
}
