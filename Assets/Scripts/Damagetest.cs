using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagetest : MonoBehaviour
{
    Collider weapon;
    GameObject player;
    Animator ani;
    public GameMaster gm;
    ModifiedTPC charCtrl;



    private void Start()
    {
        ani = GetComponent<Animator>();
        weapon = GetComponent<Collider>();
        player = GameObject.FindGameObjectWithTag("Player");
        charCtrl = player.GetComponent<ModifiedTPC>();
        disableWeapon();
    }
    /*
    void FixedUpdate()
    {
        
        
        if (Input.GetMouseButtonDown(1))
        {
            if(gm.playerstagger == false)
            {
                weapon.enabled = true;
                ani.SetTrigger("attack");
            }
            else
            {
                Debug.Log("you are staggered");
            }
            
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (gm.playerstagger == false)
            {
                ani.SetBool("block", true);
            }
            else
            {
                Debug.Log("cant block while staggered");
            }
                
            
        }
        else
        {
            
            ani.SetBool("block", false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            weapon.enabled = true;
            ani.SetTrigger("swing");
        }
        

}
    */


    public void disableWeapon()
    {
        if (weapon.enabled)
        {
            weapon.enabled = false;
        }

    }
    void blockEnable()
    {
        charCtrl.blocking = true;
    }
    void blockDisable()
    {
        charCtrl.blocking = false;
    }

}
