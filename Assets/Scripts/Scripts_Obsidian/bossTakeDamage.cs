using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossTakeDamage : MonoBehaviour
{
    [SerializeField] int hurtboxDamage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Weapon")
        {
            bossAiObsidian.instance.damageParticles.SetActive(true);
            bossAiObsidian.instance.bossHealth -= hurtboxDamage * 3;
            bossAiObsidian.instance.hitTick = true;
            hitPause.instance.INevarFreeze();
            ModifiedTPC.instance.disableWeapon();
            Debug.Log(GetComponentInParent<bossColorOverride>());
            GetComponentInParent<bossColorOverride>().colorFade = 1;
        }
    }

    private void Update()
    {
        /*if (hitTick && InvincibleFrameTimer >= 0 && !bossAiObsidian.instance.phaseChanging)
        {
            hitTick = true;
            InvincibleFrameTimer -= Time.deltaTime;
            foreach (var hurtbox in bossAiObsidian.instance.hurtboxes)
            {
                hurtbox.SetActive(false);
            }
        }
        if (hitTick && InvincibleFrameTimer <= 0 && !bossAiObsidian.instance.phaseChanging)
        {
            hitTick = false;
            InvincibleFrameTimer = 0.25f;
            foreach (var hurtbox in bossAiObsidian.instance.hurtboxes)
            {
                hurtbox.SetActive(true);
            }
        }*/
    }
}
