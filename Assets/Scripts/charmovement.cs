using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charmovement : MonoBehaviour
{
    Vector3 movement;
    //Vector3 turning;
    public float turnSmoothing = 5f;
    const float moveSpeed = 10f;
    //float evadeTime = 0.5f; //how long the evade takes
    //float evadespeed = 50f; //how far to evade
    bool evading;
    Rigidbody rb;
    Animator ani;
    GameObject boss;
    GameObject weaponobj;
    Collider weapon;
    Animator aniweap;
    Animator bossAni;
    float xMovement;
    float zMovement;
    public GameObject blinktarget;
    Transform blinkpos;
    public bool blocking;
    Collider cl;
    public GameMaster gm;
    public twoDshift td;
    
    

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        cl = GetComponent<Collider>();
        boss = GameObject.FindGameObjectWithTag("Boss");
        bossAni = boss.GetComponent<Animator>();
        weaponobj = GameObject.FindGameObjectWithTag("Weapon");
        weapon = weaponobj.GetComponent<Collider>();
        aniweap = weaponobj.GetComponent<Animator>();
        //gm = GetComponent<GameMaster>();
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && gm.playerstagger == false)
        {
            blocking = true;
            aniweap.SetBool("block", true);
        }
        else
        {
            blocking = false;
            aniweap.SetBool("block", false);
        }

        Move(xMovement, zMovement);


        //if (Input.GetMouseButtonDown(0))
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (gm.playerstagger == false)
            {
                weapon.enabled = true;

                aniweap.SetTrigger("swing");

            }
            else
            {
                Debug.Log("you are staggered");
            }

        }


        if (Input.GetMouseButtonDown(1))
        {
            if (gm.playerstagger == false)
            {
                weapon.enabled = true;
                aniweap.SetTrigger("attack");
            }
            else
            {
                Debug.Log("you are staggered");
            }

        }



    }
    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector3(rb.velocity.x, 10f, rb.velocity.z);
            //rb.AddForce(Vector3.up * 100f);
            Debug.Log("jump");
        }

        if (!gm.playerstagger && !gm.twoDmode)
        {
            xMovement = Input.GetAxisRaw("Horizontal") * moveSpeed;
            zMovement = Input.GetAxisRaw("Vertical") * moveSpeed;
            evading = Input.GetKeyDown(KeyCode.LeftControl);
        }
        else if (!gm.playerstagger && gm.twoDmode)
        {
            zMovement = Input.GetAxisRaw("Vertical") * moveSpeed;
        }
        else
        {
            xMovement = 0;
            zMovement = 0;
            evading = false;
        }

        if (evading)
        {
            Evade();
        }

        //Uncomment this when walking animations are added
        /*if(xMovement > 0 || zMovement > 0)
        {
            ani.SetBool("isWalking", true);
        }
        else
        {
            ani.SetBool("isWalking", false);
        }
        */


    }
    void Move(float x, float z)
    {
        //rb.velocity = new Vector3(xMovement, 0f, zMovement);
        movement.Set(x, 0f, z);
        movement = movement.normalized * moveSpeed * Time.deltaTime;

        rb.MovePosition(transform.position + movement);

        if (x!=0 || z!=0)
        {
            Rotation(x,z);
        }
    }
    void Rotation(float x, float z)
    {
        Vector3 targetDirection = new Vector3(x, 0f, z);
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

        Quaternion newRotation = Quaternion.Lerp(rb.rotation,targetRotation,turnSmoothing * Time.deltaTime);
        rb.MoveRotation(newRotation);
    }
    void Evade()
    {
        blinkpos = blinktarget.transform;
        transform.position = blinkpos.position;    
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.transform.tag == "Enemy")
        {
            if (!blocking)
            {
                Debug.Log("Ive Been hit");
                gm.playerDamaged(10);
                
            }
            else if(blocking)
            {
                bossAni.SetTrigger("stagger");
                gm.blockTutorialIterate();
                Debug.Log("blockedIt");

            }
        }
        else if (other.transform.tag == "unblockable")
        {
            Debug.Log("cant blockthis");
            gm.playerDamaged(10);
        }
    }

    
}
