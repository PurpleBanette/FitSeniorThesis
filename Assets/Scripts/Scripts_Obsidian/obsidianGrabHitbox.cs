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
    
    void OnTriggerEnter(Collider other) // if the player is standing on the boss's head, trigger an animation in the Animator
    {
        if (other.transform.tag == "Player")
        {
            bossAiObsidian.instance.bossAnimator.SetTrigger("grabP1");
        }
    }
    void OnTriggerStay(Collider other) // if the player stays on the boss's head while the animation is playing, trigger a second animation in the Animator
    {
        if (other.transform.tag == "Player" && bossAiObsidian.instance.currentphase == 1)
        {
            bossAiObsidian.instance.bossAnimator.SetBool("playerInGrabRangeP1", true);
            bossAiObsidian.instance.bossAnimator.ResetTrigger("axeAttack1");
            bossAiObsidian.instance.bossAnimator.ResetTrigger("axeAttack2");
            bossAiObsidian.instance.bossAnimator.ResetTrigger("axeAttack3");
        }
        if (other.transform.tag == "Player" && bossAiObsidian.instance.currentphase == 2)
        {
            bossAiObsidian.instance.bossAnimator.SetBool("playerInGrabRangeP1", true);
            bossAiObsidian.instance.bossAnimator.ResetTrigger("railgunAttack1");
            bossAiObsidian.instance.bossAnimator.ResetTrigger("railgunAttack2");
            bossAiObsidian.instance.bossAnimator.ResetTrigger("railgunAttack3");
        }
    }
    void OnTriggerExit(Collider other) // if the player leaves boss's head while the animation is playing, trigger a second animation in the Animator
    {
        if (other.transform.tag == "Player")
        {
            bossAiObsidian.instance.bossAnimator.SetBool("playerInGrabRangeP1", false);
            bossAiObsidian.instance.bossAnimator.ResetTrigger("grabP1");
        }
    }
}
