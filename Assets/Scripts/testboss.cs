using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testboss : MonoBehaviour
{
    Animator ani;
    int health;
    int randAttack;
    int currentphase = 1;
    bool dead;
    GameObject player;
    void Awake()
    {
        ani = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        StartCoroutine(phase1Pattern());
    }

    // Update is called once per frame
    void Update()
    {
        //prevents the boss from doing anything after he dies
        if (!dead)
        {
            transform.LookAt(player.transform);
            //this changes phases whenever you press the X key
            //change to a health threshold or some other event for actual bosses
            if (Input.GetKeyDown(KeyCode.X))
            {
                currentphase++;
                if(currentphase == 2)
                {
                    StopAllCoroutines();
                    StartCoroutine(phase2Pattern());
                    Debug.Log("phase 2");
                }
                if(currentphase == 3)
                {
                    StopAllCoroutines();
                    StartCoroutine(phase3Pattern());
                    Debug.Log("phase 3");
                }
                if(currentphase >= 4)
                {
                    StopAllCoroutines();
                    ani.SetTrigger("Dead");
                    dead = true;
                }
        
            }    
        }
    }


    IEnumerator phase1Pattern()
    {
        while (true && !dead)
        {
            yield return new WaitForSeconds(2);
            randAttack = Random.Range(0, 3);
            if (randAttack == 1)
                ani.SetTrigger("SpinAttack");
            if (randAttack == 2)
                ani.SetTrigger("Stab");
        }
    }

    IEnumerator phase2Pattern()
    {
        while (true && !dead)
        {
            yield return new WaitForSeconds(2);
            randAttack = Random.Range(0, 3);
            if (randAttack == 1)
                ani.SetTrigger("DoubleShot");
            if (randAttack == 2)
                ani.SetTrigger("Shoot");
        }
    }

    IEnumerator phase3Pattern()
    {
        while (true && !dead)
        {
            yield return new WaitForSeconds(2);
            randAttack = Random.Range(0, 5);
            if (randAttack == 1)
                ani.SetTrigger("DoubleShot");
            if (randAttack == 2)
                ani.SetTrigger("SpinAttack");
            if (randAttack == 3)
                ani.SetTrigger("Shoot");
            if (randAttack == 4)
                ani.SetTrigger("Stab");
        }
    }
}
