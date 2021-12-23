using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public GameObject player;
    GameObject boss;
    public static GameMaster gm;
    /*
    public Text healthUi;
    public Text staggeringUi;
    public Text staggerTimeUi;
    public Text blockTutorialUi;
    public Text attackTutorialUi;
    */
    //int blocksPerformed = 0;
    //int attacksPerformed = 0;
    //int health = 100;
    public bool playerstagger;
    float staggertime = 1f;
    //public bool twoDmode = true;
    bool attackTutComplete;
    bool blockTutComplete;
    public bool firstTutComplete;
    public Transform playerPosition;

    [HideInInspector] public Transform playerDodgePosition;
    [SerializeField] float slowMoTimeScale = 0.2f;
    [SerializeField] float slowMoDuration = 0.3f;
    float sloMoTimer;
    bool slowMotion = false;


    void Awake()
    {
        gm = this;

        player = GameObject.FindGameObjectWithTag("Player");
        boss = GameObject.FindGameObjectWithTag("Boss");
        /*
        FB = boss.GetComponent<firstBoss>();
        if(FB != null)
        {
            Debug.Log("ScriptFound");
        }
        else
        {
            Debug.LogWarning("Boss Script NOT DETECTED");
        }
        */

        /*
        healthUi.text = "Health online";
        staggeringUi.text = "Ready";
        staggerTimeUi.text = staggertime.ToString();
        blockTutorialUi.text = "Attacks Blocked - 0/3";
        attackTutorialUi.text = "Attacks - 0/3";
        */

        sloMoTimer = slowMoDuration;
    }

    private void Update()
    {

        //The stagger mechanic can be reIntroduced later

        /* 
        if (playerstagger)
        {
            staggeringUi.text = "Staggering";
            staggertime -= Time.deltaTime;
            staggerTimeUi.text = staggertime.ToString();
            if (staggertime <= 0)
            {
                playerstagger = false;
                staggertime = 1f;
            }
        }
        else
        {
            staggeringUi.text = "Ready";
            staggerTimeUi.text = staggertime.ToString();
        }
        */
        if (slowMotion)
        {
            sloMoTimer -= Time.deltaTime;
            Debug.Log(sloMoTimer);
            if(sloMoTimer <= 0)
            {
                Time.timeScale = 1;
                sloMoTimer = slowMoDuration;
                slowMotion = false;
            }
            
        }

    }

    //This is handled in ModifiedTPC
    /*
    public void playerDamaged(int damageval)
    {
        health = health - damageval;
        healthUi.text = "Health: " + health;

        if (health <= 0)
        {
            healthUi.text = "dead";
        }
    }
    */

    //We can expirament with 2d mode later
    /*
    public void twoDknockback()
    {

        boss.transform.position += transform.forward * 5f;
        player.transform.position += transform.forward * 5f;

    }
    */

    
    //Commenting out Tutorial Scripts for now
    
    /*
    public void blockTutorialIterate()
    {
        blocksPerformed += 1;
        if (blocksPerformed >= 3)
        {
            blockTutorialUi.text = "Completed - 3/3";
            blockTutComplete = true;
            checkBlockAttackTutorial();
        }
        else
        {
            blockTutorialUi.text = "Attacks Blocked - " + blocksPerformed.ToString() + "/3";
        }


    }

    public void attackTutorialIterate()
    {
        attacksPerformed += 1;
        if (attacksPerformed < 3)
        {
            attackTutorialUi.text = "Attacks Landed - " + attacksPerformed.ToString() + "/3";
            attackTutComplete = true;
            checkBlockAttackTutorial();
        }
        else
        {
            attackTutorialUi.text = "Completed - 3/3";
        }

    }

    public void checkBlockAttackTutorial()
    {
        if (attackTutComplete && blockTutComplete)
        {
            //Start Unblockables Tutorial
            //FB.startPhase2();
            firstTutComplete = true;

        }
        else
        {
            Debug.Log("Tutorial Not finished yet");
        }
    }
    */

    public void activateSlowMo()
    {
        Time.timeScale = slowMoTimeScale;
        slowMotion = true;
    }

    public IEnumerator SlowMoCo()
    {
        Time.timeScale = slowMoTimeScale;
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1;
        //StopCoroutine(activateSlowMo());
    }
}
