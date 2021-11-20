using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;
    //firstBoss FB;
    GameObject player;
    GameObject boss;
    public Text healthUi;
    public Text staggeringUi;
    public Text staggerTimeUi;
    public Text blockTutorialUi;
    public Text attackTutorialUi;
    int blocksPerformed = 0;
    int attacksPerformed = 0;
    int health = 100;
    public bool playerstagger;
    float staggertime = 1f;
    public float defaultcombowindow = 2f;
    public float combowindow;
    public bool twoDmode = true;
    public bool comboAllowed;
    bool attackTutComplete;
    bool blockTutComplete;
    public bool firstTutComplete;


    void Awake()
    {
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
        combowindow = defaultcombowindow;
        healthUi.text = "Health online";
        staggeringUi.text = "Ready";
        staggerTimeUi.text = staggertime.ToString();
        blockTutorialUi.text = "Attacks Blocked - 0/3";
        attackTutorialUi.text = "Attacks - 0/3";
    }

    private void Update()
    {
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


        if (comboAllowed)
        {
            combowindow -= Time.deltaTime;
            if (combowindow <= 0)
            {
                comboAllowed = false;
                combowindow = defaultcombowindow;
            }
        }

    }
    public void playerDamaged(int damageval)
    {
        health = health - damageval;
        healthUi.text = "Health: " + health;

        if (health <= 0)
        {
            healthUi.text = "dead";
        }
    }

    public void twoDknockback()
    {

        boss.transform.position += transform.forward * 5f;
        player.transform.position += transform.forward * 5f;

    }

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

}
