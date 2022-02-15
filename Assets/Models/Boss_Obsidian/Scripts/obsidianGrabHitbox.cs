using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obsidianGrabHitbox : MonoBehaviour
{
    public static obsidianGrabHitbox instance;
    void Awake()
    {
        instance = this;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            bossAiObsidian.instance.bossAnimator.SetTrigger("grabP1");
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            bossAiObsidian.instance.bossAnimator.SetBool("playerInGrabRange", true);
            bossAiObsidian.instance.bossAnimator.ResetTrigger("axeAttack1");
            bossAiObsidian.instance.bossAnimator.ResetTrigger("axeAttack2");
            bossAiObsidian.instance.bossAnimator.ResetTrigger("axeAttack3");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            bossAiObsidian.instance.bossAnimator.SetBool("playerInGrabRange", false);
            bossAiObsidian.instance.bossAnimator.ResetTrigger("grabP1");
        }
    }
}
