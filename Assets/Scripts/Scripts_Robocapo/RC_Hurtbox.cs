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
            bossAiRobocapo.instance.damageParticles.SetActive(true);
            hitPause.instance.INevarFreeze();
            ModifiedTPC.instance.disableWeapon();
            bossAiRobocapo.instance.rcTakeDamage(hurtboxDamage);
            bossAiRobocapo.instance.hitTick = true;
            GetComponentInParent<bossColorOverride>().colorFade = 1;
        }
    }
}
