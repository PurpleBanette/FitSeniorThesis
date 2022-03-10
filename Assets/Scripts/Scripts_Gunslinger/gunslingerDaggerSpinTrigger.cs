using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunslingerDaggerSpinTrigger : MonoBehaviour
{
    public static gunslingerDaggerSpinTrigger instance;
    public bool playerTooClose;
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
            playerTooClose = true;
            bossAiGunslinger.instance.bossAnimator.SetBool("rangeDaggerSpin", true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            playerTooClose = false;
            bossAiGunslinger.instance.bossAnimator.SetBool("rangeDaggerSpin", false);
        }
    }
}
