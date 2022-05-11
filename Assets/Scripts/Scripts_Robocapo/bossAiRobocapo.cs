using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent), typeof(AgentLinkMoverRobocapo))]

public class bossAiRobocapo : MonoBehaviour
{

    //public static bossAiRobocapo instance;
    //General
    public Animator bossAnimator;
    [Header("Boss Information")]
    [Tooltip("The boss's health")]
    public int bossHealth = 1000;
    [Tooltip("The current phase of the boss")]
    public int currentphase;
    [Tooltip("A bool that determines if the boss is dead")]
    [SerializeField] bool dead = false;
    [Tooltip("The boss's health slider")]
    [SerializeField] Slider bossHealthbar;
    [Tooltip("The movement speed of the boss during each phase")]
    public float bossMoveSpeedP1, bossMoveSpeedP2, bossMoveSpeedP3, bossMoveSpeedP4;
    public float dashSpeed;
    [Tooltip("Checks if the boss is in the middle of an attack animation")]
    public bool bossIsAttacking;
    [Tooltip("The boss's capsule collider")]
    CapsuleCollider bossCapsuleCollider;
    [Tooltip("The boss's rigidbody")]
    Rigidbody bossRigidbody;
    public bool phaseChanging;
    //Projectiles
    [Header("Projectiles Information")]
    [SerializeField] Transform bulletPositionPlaceholder;
    [SerializeField] GameObject bulletPlaceholder;
    [SerializeField] float bulletShootForce, bulletUpwardForce;
    [SerializeField] float bulletSpread;
    [Tooltip("Checks if rapid fire is on/off")]
    [SerializeField] bool bulletRapidfireTrigger;
    //Player
    [Header("Player Information")]
    [Tooltip("The player")]
    public Transform player;
    [Tooltip("A target to allow the boss to aim at the player's center")]
    public Transform playerTarget;
    //AI
    [Header("Boss AI Information")]
    [Tooltip("The boss's nav mesh agent")]
    public NavMeshAgent bossNavAgent;
    [Tooltip("This references the area mask in the nav mesh")]
    LayerMask areaMask;
    [Tooltip("A random number generated for a random attack to be used")]
    public int randAttack;
    [Tooltip("A random number generated to randomize the boss' pathfinding")]
    int randPath;
    [Tooltip("How far the boss can see")]
    public float fovRadius;
    [Tooltip("How wide the boss can see")]
    [Range(0, 360)] public float fovAngle;
    [Tooltip("detect which layer the boss can detect/interact with")]
    [SerializeField] LayerMask bossTerrainDetector, bossPlayerDetector, bossObstacleDetector;
    [Tooltip("Detects if the boss is following the player")]
    [SerializeField] bool followingPlayer = true;
    [Tooltip("The range of how far the AI can be before detecting and attacking")]
    [SerializeField] float sightRange, attackRange;
    [Tooltip("Checks if the player is within range")]
    public bool playerInSightRange, playerInAttackRange, playerInLos;
    [Tooltip("A bool used to force the boss to look at the player")]
    public bool playerTracking = false;
    [Tooltip("Checks if boss is grounded")]
    public bool bossGrounded;
    float bossGroundedRadius = 5f;
    [Space(10)]
    //Patroling
    [Tooltip("The walkpoint that the AI goes to")]
    public Vector3 walkPoint;
    [Tooltip("Checks if there is a walkpoint")]
    bool walkPointSet;
    [Tooltip("How far the AI can detect a flat surface to walk to")]
    [SerializeField] float walkPointRange;
    [Space(10)]
    //Attacking
    [Tooltip("The time between each attack for each phase")]
    [SerializeField] float timeBetweenAttacksP1, timeBetweenAttacksP2, timeBetweenAttacksP3, timeBetweenAttacksP4;
    //[Tooltip("the cooldown for attacks")]
    //[SerializeField] float attackCooldown;
    //[Tooltip("The minimum and maximum timer range between attacks")]
    //[SerializeField] float attackCooldownMin, attackCooldownMax;
    private AgentLinkMoverRobocapo linkMover;
    [Tooltip("The waypoints that the boss travels to")]
    public GameObject[] bossWaypoints;
    public int bossWaypointIndex;
    [Tooltip("The minimum and maximum waypoints. The first number should always be 0 and the last number should by +1 more than the total number of waypoints. If there are 16 waypoints, the number for the maximum must be 17")]
    public int bossWaypointMin, bossWaypointMax;
    [SerializeField] float randomJumpTimer = 10f;
    public bool shotLoopChecker;
    //Hitboxes
    [Header("Hitbox Information")]
    [Tooltip("A gameobject with a trigger that detects the player")]
    [SerializeField] GameObject attackTrigger;
    [Tooltip("Hitboxes for the boss's weapons during attacks")]
    [SerializeField] List<GameObject> attackHitboxesWeapons;
    [Tooltip("The boss's hurtbox to receive damage from the player")]
    public List<GameObject> hurtboxes;
    [Tooltip("Invincibility frames")]
    public float InvincibleFrameTimer = 0.25f;
    public bool hitTick = false;
    //Particles
    [Header("Particles and Effects")]
    [SerializeField] GameObject muzzleParticleEffect;


    public GameObject damageParticles;
    [Tooltip("This bool is true when RC is vulnerable to attack and false when he is capable of blocking")]
    public bool vulnurable;
    [Tooltip("This bool is true when RC is attacking while still capable of blocking")]
    public bool protAttack;
    [Tooltip("This bool is true when RC is stunned")]
    public bool stunned;
    public bool charging;
    public GameObject jumpTrail;

    //Weapons and Weapon Colliders
    [SerializeField] GameObject rightTonfa;
    [SerializeField] GameObject leftTonfa;

    [SerializeField] GameObject SweeperSphere;

    [SerializeField] GameObject rightBayonet;
    [SerializeField] GameObject leftBayonet;

    [SerializeField] GameObject bulletEmmitterLeft;
    [SerializeField] GameObject bulletEmmitterRight;

    public GameObject chargeHitbox;

    //Debug Ui
    [SerializeField] Text behaviorTracker;
    [SerializeField] Text randAttackTracker;


    //Managers
    int bulletPoolManager = 0;

    float buckshotForce = 100f;
    GameObject currentBullet;
    bool rapidFire;
    float randAttackTime;
    float minAtkCooldown = 3.5f;
    float maxAtkCooldown = 6f;
    public bool attackSet = true;
    

    //DEBUG Variables

    //Use this to disable the vulnurability mechanic
    bool Debug_InfiniteVulnurability = true;
    [SerializeField] Text TxtDebug_CurrentPhase;


    void Awake()
    {
        //instance = this;

        bossAnimator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bossNavAgent = GetComponent<NavMeshAgent>();
        bossCapsuleCollider = GetComponent<CapsuleCollider>();

        randAttackTime = Random.Range(minAtkCooldown, maxAtkCooldown);

        linkMover = GetComponent<AgentLinkMoverRobocapo>();

        bossHealthbar.maxValue = bossHealth;
        bossHealthbar.value = bossHealth;
        //linkMover.OnLinkStart += HandleLinkStart;
        //linkMover.OnLinkEnd += HandleLinkEnd;

        //Add to the hurtbox list
        attackHitboxesWeapons.Add(rightBayonet);
        attackHitboxesWeapons.Add(leftBayonet);
        attackHitboxesWeapons.Add(chargeHitbox);
        attackHitboxesWeapons.Add(SweeperSphere);
        attackHitboxesWeapons.Add(rightTonfa);
        attackHitboxesWeapons.Add(leftTonfa);

        disableWeaponHitboxes();
    }

    void Start()
    {
        //bossAnimator.SetTrigger("Introduction");
        //StartCoroutine(Phase1Pattern()); //Starts phase 1
        currentphase = 1;
        BossIdle();
        
        //Phase1Stats();
    }

    void Update()
    {
       

        TxtDebug_CurrentPhase.text = currentphase.ToString(); 

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, bossPlayerDetector); //checks for sight range
        playerInAttackRange = Physics.CheckSphere(attackTrigger.transform.position, attackRange, bossPlayerDetector); //checks for attack range
        PlayerTracking();

        //This cooldown is to prevent RC from constantly triggering an attack
        if (attackSet)
        {
            randAttackTime -= Time.deltaTime;
            if(randAttackTime <= 0)
            {
                randAttackTime = Random.Range(minAtkCooldown, maxAtkCooldown);
                attackSet = false;
            }
        }

        BossGroundCheck();
        JumpDeltaTimer();
        PhaseAiStates();
        //Debug.Log(randAttackTime);
        randAttackTracker.text = randAttack.ToString();
        InvincibilityDetection();

        //Phase Setter
        if (bossHealth <= 750 && currentphase == 1)
        {
            attackTrigger.SetActive(false);
            currentphase = 2;
            //Phase2Transition();
        }

        if (bossHealth <= 500 && currentphase == 2)
        {
            attackTrigger.SetActive(false);
            currentphase = 3;

        }

        if (bossHealth <= 250 && currentphase == 3)
        {
            attackTrigger.SetActive(false);
            currentphase = 4;
        }

        if (bossHealth <= 0)
        {
            attackTrigger.SetActive(false);
            currentphase = 5;
            dead = true;
            bossAnimator.SetTrigger("Death");
        }

    }
    void FixedUpdate()
    {
        BossAnimations();
    }
    private void HandleLinkStart() //Controls animations when the boss interacts with jumpable navmesh links
    {
        if (!dead)
        {
            bossAnimator.SetTrigger("jump");
        }
    }

    private void HandleLinkEnd() //Controls animations when the boss interacts with jumpable navmesh links
    {
        if (!dead)
        {
            bossAnimator.SetTrigger("land");
        }
    }
    void BossAnimations()
    {
        if (!dead)
        {
            bossAnimator.SetFloat("isWalking", Mathf.Abs(bossNavAgent.speed));    
        }
    }

    void PhaseAiStates()
    {
        if (currentphase == 1 || currentphase == 3 || currentphase == 4)
        {
            //Phase1Pattern();
            
            if(!attackSet && !playerInAttackRange && !charging)
            {
                DashToPlayer();
                behaviorTracker.text = "Dashing";
            }
            else if(!attackSet && playerInAttackRange && !charging) 
            {
                BossAttackPlayer();
                behaviorTracker.text = "Attacking";
            }
            else if(charging)
            {
                behaviorTracker.text = "Charging";
            }
            else
            {
                BossIdle();
                behaviorTracker.text = "Idle";
            }
            
            /*
            else if (!playerInSightRange && !playerInAttackRange)
            {
                BossPatroling();
            }
            */

            /*
            if (playerInSightRange && !playerInAttackRange) BossChasePlayer();
            if (playerInSightRange && playerInAttackRange) BossAttackPlayer();
            */

            
        }
        
        
        if (currentphase == 2)
        {
            //Phase2Pattern();
            //Searching for the player tag within its FOV radius
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, fovRadius, bossPlayerDetector);
            //if (!playerInSightRange && !playerInLos) BossPatroling(); //Patrol if the boss can't find the player
            //if (playerInSightRange && !playerInLos) BossChasePlayer(); //Chase the player if they are in sight range
            if (rangeChecks.Length != 0) //Attack the player if they are within line of sight and in range
            {
                Transform fovTarget = rangeChecks[0].transform; //Checks for colliders
                Vector3 directionToTarget = (player.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < fovAngle / 2) //If the player is within line of sight range
                {
                    float distanceToTarget = Vector3.Distance(transform.position, player.position);

                    //If there is no obstacle in the way, the player is detected
                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, bossObstacleDetector))
                    {
                        playerInLos = true;
                    }
                    else
                    {
                        playerInLos = false;
                    }
                }
                else
                {
                    playerInLos = false;
                }
            }
            else if (playerInLos)
            {
                playerInLos = false;
            }
            
            //if (playerInLos) BossAttackPlayer();

            ///ALTERNATE SETUP
            if (!attackSet && !playerInAttackRange && playerInLos)
            {
                //RangedAttacks
                Phase2RangedAttackList();
                behaviorTracker.text = "Ranged Attacks";
            }
            else if (!attackSet && playerInAttackRange)
            {
                BossAttackPlayer();
                behaviorTracker.text = "Melee Attacking";
            }
            
            else
            {
                BossIdle();
                behaviorTracker.text = "Idle";
            }
        }

    }


    void BossIdle()
    {
        bossNavAgent.speed = 0f;
    }
    
    void BossPatroling() //The boss moving in random directions
    {
        bossNavAgent.speed = bossMoveSpeedP1;

        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            bossNavAgent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //If the boss reaches its destination
        if (distanceToWalkPoint.magnitude < 2f)
        {
            walkPointSet = false;
            bossWaypointIndex = Random.Range(bossWaypointMin, bossWaypointMax);
        }
    }
    void SearchWalkPoint()
    {
        walkPoint = bossWaypoints[bossWaypointIndex].transform.position;
        walkPointSet = true;
    }
    void BossChasePlayer()
    {
        if (!dead)
        {
            bossNavAgent.SetDestination(player.position);
            bossNavAgent.speed = bossMoveSpeedP1;
        }
    }

    void DashToPlayer()
    {
        bossNavAgent.SetDestination(player.position);
        bossNavAgent.speed = dashSpeed;
    }

    void Phase2RangedAttackList()
    {
        if (!attackSet)
        {
            randAttack = Random.Range(7, 9);
            if (currentphase == 1 || currentphase == 2)
            {
                if (randAttack == 7)
                {
                    bossAnimator.SetTrigger("Gunspin");
                }
                if (randAttack == 8)
                {
                    bossAnimator.SetBool("ShotLoop", true);
                    shotLoopChecker = true;
                }
                attackSet = true;
            }
            /*if (randAttack <= 5)
            {
                bossAnimator.SetBool("ShotLoop", true);
            }
            else
            {
                bossAnimator.SetTrigger("Gunspin");
            }*/

            
        }



    }

    void BossAttackPlayer()
    {
        bossNavAgent.SetDestination(transform.position); //This forces the boss to stay in place during attacks
        bossNavAgent.speed = 0f;

        //Debug.Log("attackTick");
        if (!attackSet && !robocapoPlayerRangeDetectorSphere.instance.playerTooFarAway)
        {
            randAttack = Random.Range(1, 7);
            if (currentphase == 1 || currentphase == 2)
            {
                if (randAttack == 1)
                {
                    bossAnimator.SetBool("TripleStab", true);
                }
                if (randAttack == 2)
                {
                    bossAnimator.SetTrigger("DoubleWind");
                }
                if (randAttack == 3)
                {
                    bossAnimator.SetBool("3Hit", true);
                }
                if (randAttack == 4)
                {
                    bossAnimator.SetBool("WindUpOverhead", true);
                }
                if (randAttack == 5)
                {
                    bossAnimator.SetTrigger("DoubleWind");
                }
                if (randAttack == 6)
                {
                    bossAnimator.SetBool("basicAttack", true);
                }
                attackSet = true;
            }
            /*randAttack = Random.Range(1, 8);

            if(currentphase == 1 || currentphase == 2)
            {
                if (randAttack <= 2 || randAttack == 3)
                {
                    bossAnimator.SetBool("3Hit", true);
                }
                /*
                if (randAttack == 4 || randAttack == 7)
                {
                    bossAnimator.SetTrigger("WindmillCharge");
                }
                if (randAttack == 5 || randAttack == 6)
                {
                    bossAnimator.SetBool("WindUpOverhead", true);
                }
                
                else
                {
                    bossAnimator.SetBool("WindUpOverhead", true);
                }

            }*/



            /*
            else if (currentphase == 2)
            {
                if(randAttack <= 5)
                {
                    bossAnimator.SetBool("ShotLoop", true);
                }
                else
                {
                    bossAnimator.SetTrigger("Gunspin");
                }
            }
            */
            /*else if(currentphase == 3)
            {
                if(randAttack <= 3)
                {
                    bossAnimator.SetBool("basicAttack", true);
                }
                else
                {
                    bossAnimator.SetTrigger("DoubleWind");
                }
            }
            attackSet = true;*/
        }

        //resets the cooldown for attacks
        //attackSet = true;
    }
    void PlayerTracking() //Tracks the player's position so the boss faces in their direction
    {
         
        if (playerTracking)
        {
            //The Y position uses the boss's Y position so that the boss does not rotate vertically based on the player's vertical position
            Vector3 targetPostition = new Vector3(player.position.x, this.transform.position.y, player.position.z);
            transform.LookAt(targetPostition);
        }
    }

    void PlayerTrackFalse()
    {
        playerTracking = false;
    }

    void PlayerTrackTrue()
    {
        playerTracking = true;
    }

    public void rcTakeDamage(int damage)
    {
        if (vulnurable || Debug_InfiniteVulnurability)
        {
            bossHealth -= damage;
            bossHealthbar.value = bossHealth;
            //bossAnimator.SetTrigger("stagger");
        }

        else
        {
            Debug.Log("the boss blocked the Attack");
            
            if (!protAttack)
            {
                bossAnimator.SetTrigger("block");
            }
            
            ModifiedTPC.instance.weapon.enabled = false;
        }
    }

    public void disableWeaponHitboxes()
    {
        foreach (GameObject weapon in attackHitboxesWeapons)
        {
            if(weapon.activeInHierarchy)
            {
                weapon.SetActive(false);
            }
            
        }
    }

    //Stat Changes

    void DashStats()
    {
        bossNavAgent.speed = dashSpeed;
        bossNavAgent.angularSpeed = 600;
        bossNavAgent.acceleration = 500;
        bossNavAgent.stoppingDistance = 10;
        bossNavAgent.autoBraking = true;
    }

    /*
    void Phase1Stats()
    {
        //This code should be at the end of the introduction animation event state, but for now it is here so that the boss can move
        bossNavAgent.speed = bossMoveSpeedP1;
        bossNavAgent.angularSpeed = 600;
        bossNavAgent.acceleration = 100;
        bossNavAgent.stoppingDistance = 0;
        bossNavAgent.autoBraking = true;
    }

    void Phase2Stats()
    {
        //This code should be at the end of the introduction animation event state, but for now it is here so that the boss can move
        bossNavAgent.speed = bossMoveSpeedP2;
        bossNavAgent.angularSpeed = 750;
        bossNavAgent.acceleration = 75;
        bossNavAgent.stoppingDistance = 0;
        bossNavAgent.autoBraking = true;
    }
    

    //Attack Patterns
    IEnumerator Phase1Pattern()
    {
        attackTrigger.SetActive(true);
        while (true && !dead)
        {
            yield return new WaitForSeconds(0);
            if (playerInAttackRange)
            {
                
                randAttack = Random.Range(1, 8);
                yield return new WaitForSeconds(timeBetweenAttacksP1);
            }
        }
    }
    

    void Phase2Transition()
    {
        StopAllCoroutines();
        //bossAnimator.SetTrigger("phase2");
    }
    
    IEnumerator Phase2Pattern()
    {
        attackTrigger.SetActive(true);
        while (true && !dead)
        {
            yield return new WaitForSeconds(0);
            if (playerInLos)
            {
                Debug.Log("I shot you with imaginary bullets");
                randAttack = Random.Range(1, 2);
                yield return new WaitForSeconds(timeBetweenAttacksP2);
            }
        }
    }
    */

    //Animation events

    void ActivateRightBayonet()
    {
        rightBayonet.SetActive(true);
    }

    void DisableRightBayonet()
    {
        rightBayonet.SetActive(false);
    }

    void ActivateLeftBayonet()
    {
        leftBayonet.SetActive(true);
    }

    void DisableLeftBayonet()
    {
        leftBayonet.SetActive(false);
    }

    void ActivateRightTonfa()
    {
        leftTonfa.SetActive(true);
    }

    void DisableRightTonfa()
    {
        leftTonfa.SetActive(false);
    }

    void ActivateLeftTonfa()
    {
        rightTonfa.SetActive(true);
    }

    void DisableLeftTonfa()
    {
        rightTonfa.SetActive(false);
    }

    void leftBuckshot()
    {
        //currentBullet = RC_ObjectPool.instance.GetPooledObject(RC_ObjectPool.instance.pooledBuckshot, true);
        //RC_ObjectPool.instance.LaunchObject(currentBullet, playerTarget, bulletEmmitterLeft.transform, 0f, null, buckshotForce);
        bulletPoolManager = RC_ObjectPool.instance.GetPooledObjectManaged(RC_ObjectPool.instance.pooledBuckshot, bulletPoolManager,playerTarget, bulletEmmitterLeft.transform, 0f, RC_ObjectPool.instance.pooledMuzzleFlashes, buckshotForce);
    }

    void rightBuckshot()
    {
        //currentBullet = RC_ObjectPool.instance.GetPooledObject(RC_ObjectPool.instance.pooledBuckshot, true);
        //RC_ObjectPool.instance.LaunchObject(currentBullet, playerTarget, bulletEmmitterRight.transform, 0f, null, buckshotForce);
        bulletPoolManager = RC_ObjectPool.instance.GetPooledObjectManaged(RC_ObjectPool.instance.pooledBuckshot, bulletPoolManager, playerTarget, bulletEmmitterRight.transform, 0f, RC_ObjectPool.instance.pooledMuzzleFlashes, buckshotForce);
    }

    void startRapidFire()
    {
        StartCoroutine("RapidFire");
    }

    IEnumerator RapidFire()
    {
        rapidFire = true;
        while (rapidFire == true)
        {
            bulletPoolManager = RC_ObjectPool.instance.GetPooledObjectManaged(RC_ObjectPool.instance.pooledBullets, bulletPoolManager, RC_ObjectPool.instance.L_defaultTarget.transform, bulletEmmitterLeft.transform, 0f, RC_ObjectPool.instance.pooledMuzzleFlashes, buckshotForce);
            bulletPoolManager = RC_ObjectPool.instance.GetPooledObjectManaged(RC_ObjectPool.instance.pooledBullets, bulletPoolManager, RC_ObjectPool.instance.R_defaultTarget.transform, bulletEmmitterRight.transform, 0f, RC_ObjectPool.instance.pooledMuzzleFlashes, buckshotForce);
            yield return new WaitForSeconds(0.05f);
        }
    }

    void endRapidFire()
    {
        StopCoroutine("RapidFire");
    }

    void makeVulnurable()
    {
        vulnurable = true;
    }
    void endVulnurability()
    {
        vulnurable = false;
    }

    void Charge() //The boss's charge attack
    {
        charging = true;
        playerTracking = false;
        chargeHitbox.SetActive(true);
        walkPoint = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        bossNavAgent.SetDestination(player.transform.position);
        
        bossNavAgent.speed = 150f;
        bossNavAgent.acceleration = 1000f;
        bossNavAgent.angularSpeed = 20f;
    }

    void endCharge()
    {
        chargeHitbox.SetActive(false);
        charging = false;
    }

    void endTracking()
    {
        playerTracking = false;
    }

    void activateSweepingAttack()
    {

    }
    void disableSweepingAttack()
    {

    }
    void JumpDeltaTimer()
    {
        if (shotLoopChecker)
        {
            randomJumpTimer -= Time.deltaTime * 2;
            if (randomJumpTimer <= 0)
            {
                bossAnimator.SetBool("ShotLoop", false);
                bossAnimator.SetTrigger("jump");
                randomJumpTimer = 10f;
            }
        }
        else
        {
            randomJumpTimer = 10f;
        }
    }
    void BossGroundCheck()
    {
        bossGrounded = Physics.CheckSphere(transform.position, bossGroundedRadius, bossTerrainDetector);
        if (bossGrounded)
        {
            bossGrounded = true;
            bossAnimator.SetBool("bossGrounded", true);
        }
        else
        {
            bossGrounded = false;
            bossAnimator.SetBool("bossGrounded", false);
        }
    }
    void InvincibilityDetection()
    {
        if (hitTick && InvincibleFrameTimer > 0 && !phaseChanging)
        {
            InvincibleFrameTimer -= Time.deltaTime;
            foreach (var hurtbox in hurtboxes)
            {
                hurtbox.SetActive(false);
            }
        }
        if (hitTick && InvincibleFrameTimer <= 0 && !phaseChanging)
        {
            hitTick = false;
            InvincibleFrameTimer = 0.25f;
            foreach (var hurtbox in hurtboxes)
            {
                hurtbox.SetActive(true);
            }
            damageParticles.SetActive(false);
        }
    }
}
