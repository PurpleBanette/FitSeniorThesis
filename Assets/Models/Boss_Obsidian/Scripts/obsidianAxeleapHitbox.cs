using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obsidianAxeleapHitbox : MonoBehaviour
{
    public static obsidianAxeleapHitbox instance;
    public bool playerTooFarAway;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            bossAiObsidian.instance.bossAnimator.SetBool("playerTooFarP1", false);
            playerTooFarAway = false;
            bossAiObsidian.instance.BossLeapAtPlayerP1CostModifier();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            bossAiObsidian.instance.bossAnimator.SetBool("playerTooFarP1", true);
            playerTooFarAway = true;
            bossAiObsidian.instance.BossLeapAtPlayerP1CostModifier();
        }
    }
}
