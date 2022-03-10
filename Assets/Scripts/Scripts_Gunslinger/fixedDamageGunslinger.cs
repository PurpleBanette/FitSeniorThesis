using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fixedDamageGunslinger : MonoBehaviour
{
    public static fixedDamageGunslinger instance;

    void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && name == "hitbox_daggerSpin")
        {
            ModifiedTPC.instance.health -= bossAiGunslinger.instance.damageDaggerSpin;
            ModifiedTPC.instance.FixedHealthUpdate();
        }
    }
}
