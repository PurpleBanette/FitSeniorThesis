using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class firstBoss : MonoBehaviour
{
    Animator ani;
    int health = 100;
    int randAttack;
    int randBlock;

    bool dead;
    bool vulnurable;
    GameObject player;
    RaycastHit hit;
    Transform playerpos;
    public Collider leftweap;
    public Collider rightweap;
    bool tookHit;
    //public Damagetest playerweapon;
    public GameMaster gm;
    public GameObject waveProjectile;
    public GameObject waveOrigin;
    bool charge = false;
    //float speed = 10f;
    float chargeSpeed = 30f;
    [SerializeField]
    int currentphase = 1;
    [SerializeField]
    GameObject bullet;
    [SerializeField]
    GameObject LeftEmmiter;
    [SerializeField]
    GameObject RightEmmiter;
    Vector3 targetPosition;
    bool inAttackRange;
    [SerializeField]
    float attackRange = 10f;
    [SerializeField]
    GameObject inRangeTrigger;
    [SerializeField]
    LayerMask bossTerrainDetector, bossPlayerDetector;
    [SerializeField]
    NavMeshAgent NavAgent;
    float attackCooldown = 0f;
    [SerializeField]
    Slider bossHealth;

    void Awake()
    {
        ani = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerpos = player.GetComponent<Transform>();
        if(NavAgent == null)
        {
            NavAgent = GetComponent<NavMeshAgent>();
        }
        leftweap.enabled = false;
        rightweap.enabled = false;
    }

    private void Start()
    {
        bossHealth.value = health;
        //phaseChanger(currentphase);
    }


    void Update()
    {

        //Debug.DrawRay(transform.position, Vector3.forward);
        //prevents the boss from doing anything after he dies
        if (!dead)
        {
            targetPosition = new Vector3(playerpos.position.x, this.transform.position.y, playerpos.position.z);
            this.transform.LookAt(targetPosition);
            if (charge)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, chargeSpeed * Time.deltaTime);
            }
            /*
            //should change this if to a function so it doesnt have to run on update
            if (gm.firstTutComplete && currentphase == 1)
            {
                //startPhase2();
                currentphase = 2;
                phaseChanger(currentphase);

            }
            */
            //Check to see if player is in attack range, if so attack, if not, pursue, for phase 2 add an option to shoot while out of melee range
            inAttackRange = Physics.CheckSphere(inRangeTrigger.transform.position, attackRange, bossPlayerDetector);
            if (inAttackRange && attackCooldown <= 0f)
            {
                attack();
            }
            else if(!inAttackRange)
            {
                pursuit(targetPosition);
            }
            //if the attack cooldown is above 0, it will count down, if it is below 0, it will be set to 0.
            if(attackCooldown > 0f)
            {
                attackCooldown -= Time.deltaTime;
            }
            else if(attackCooldown < 0f)
            {
                attackCooldown = 0f;
            }
        }
    }

    /// <summary>
    /// Functions
    /// </summary>
    void attack()
    {
        ani.SetTrigger("basicAttack");
        //ani.SetTrigger("stagger");
        attackCooldown = Random.Range(3f, 5f);
    }

    void pursuit(Vector3 player)
    {
        if(NavAgent.destination != player)
        {
            NavAgent.SetDestination(player);
        }
        
    }
    void phaseChanger(int phase)
    {
        if (phase == 1)
        {
            StartCoroutine(phase1Pattern());
        }
        if (phase == 2)
        {
            StopAllCoroutines();
            StartCoroutine(phase2Pattern());
            Debug.Log("phase 2");
        }
        if (phase == 3)
        {
            StopAllCoroutines();
            StartCoroutine(phase3Pattern());
            Debug.Log("phase 3");
        }
        if (phase >= 4)
        {
            StopAllCoroutines();
            ani.SetTrigger("Dead");
            dead = true;
        }
    }
    
    public void startPhase2()
    {
        StopCoroutine(phase1Pattern());
        Debug.Log("Phase 2 was triggered");
        StartCoroutine(phase2Pattern());

    }

    /// <summary>
    /// Corutines
    /// </summary>

    IEnumerator testPattern()
    {
        while (true && !dead)
        {
            yield return new WaitForSeconds(4);
            ani.SetTrigger("charge");

        }
    }
    IEnumerator phase1Pattern()
    {
        while (true && !dead)
        {
            yield return new WaitForSeconds(4);
            randAttack = 1;
            if (randAttack == 1)
            {
                /*
                if (!gm.twoDmode)
                {
                    //transform.position = Vector3.Lerp(transform.position, playerpos.position, 0.8f);
                }
                */
                ani.SetTrigger("basicAttack");
            }


        }
    }

    IEnumerator phase2Pattern()
    {
        while (true && !dead)
        {
            yield return new WaitForSeconds(3);
            randAttack = Random.Range(0, 3);
            if (randAttack == 1)
                ani.SetTrigger("wave");
            if (randAttack == 2)
                ani.SetTrigger("charge");
        }
    }

    IEnumerator phase3Pattern()
    {
        while (true && !dead)
        {
            yield return new WaitForSeconds(2);
            ani.SetTrigger("shoot");

        }
    }


    /// <summary>
    /// OnTriggerEnter and Damage
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Weapon")
        {
            //If hit by the enemy's weapon the boss check for vulnurability
            //playerweapon.disableWeapon();
            TakeDamage();
        }

    }

    void TakeDamage()
    {
        //the boss will only take damage during vulnurability frames
        if (vulnurable && !dead)
        {
            health -= 5;
            bossHealth.value = health;
            //gm.attackTutorialIterate();
            //gm.comboAllowed = true;
            //gm.combowindow = gm.defaultcombowindow;
            //Debug.Log(health);
            /*
            if (gm.twoDmode)
            {
                gm.twoDknockback();
            }
            */
            if (health <= 0)
            {
                dead = true;
                Debug.Log("thebossisdead");
            }
        }
        else if (!dead)
        {

            //the boss has a random chance to do a counter attack if he blocks an attack
            Debug.Log("Blocked");
            gm.playerstagger = true;
            /*
            randBlock = Random.Range(0, 3);
            if(randBlock == 2)
            {
                ani.SetTrigger("counter");
            }
            else
            {
                ani.SetTrigger("block");
            }
            */
            ani.SetTrigger("counter");
        }


    }
    
    /*
    void AttackRaycast()
    {
        Debug.Log("cast");
        Ray attackRay = new Ray(transform.position, Vector3.forward);
        Debug.DrawRay(transform.position, Vector3.forward);
        if (Physics.Raycast(attackRay, out hit, 5f))
        {
            if (hit.collider.tag == "Player")
            {
                //player will take damage

                Debug.Log("Hit");
            }
        }
    }
    */

    /// <summary>
    /// ANIMATION EVENT FUNCTIONS
    /// </summary>
    void vulnurability()
    {
        vulnurable = true;
    }

    void endVulnurability()
    {
        vulnurable = false;
    }

    void disableLeftWeap()
    {
        leftweap.enabled = false;
    }
    void disableRightWeap()
    {
        rightweap.enabled = false;
    }
    void enableLeftWeap()
    {
        leftweap.enabled = true;
    }
    void enableRightWeap()
    {
        rightweap.enabled = true;
    }

    void spawnWave()
    {
        Instantiate(waveProjectile, waveOrigin.transform);
    }

    void chargeAttack()
    {
        charge = true;
    }

    void endCharge()
    {
        charge = false;
    }

    void leftShoot()
    {
        Instantiate(bullet, LeftEmmiter.transform);
    }

    void rightShoot()
    {
        Instantiate(bullet, RightEmmiter.transform);
    }

    
}
