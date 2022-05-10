using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using UnityEngine.UI;

//This connects the current script to the agent link mover script (for jumping from 1 navmesh link to another)
[RequireComponent(typeof(NavMeshAgent), typeof(AgentLinkMoverRobocapo))]

public class bossAiRobocapoRemake : MonoBehaviour
{
    public static bossAiRobocapoRemake instance;

    //General Boss Information
    [Header("Boss Information")]
    [Tooltip("The boss's animator")]
    public Animator bossAnimator;
    [Tooltip("The boss's health value")]
    public int bossHealth = 1000;
    [Tooltip("The current phase of the boss")]
    public int currentphase;
    [Tooltip("Bool that checks if the boss is dead")]
    [SerializeField] bool dead;
    [Tooltip("The boss's healthbar slider")]
    [SerializeField] Slider bossHealthSlider;
    [Tooltip("The movement speed of the boss during each phase")]
    public float bossMoveSpeedP1, bossMoveSpeedP2, bossMoveSpeedP3, bossMoveSpeedP4;
    [Tooltip("A float used to add on to the boss's current speed: MoveSpeed * DashMultiplyer")]
    public float dashMultiplyer;
    [Tooltip("Checks if the boss is in the middle of an attack animation")]
    public bool bossIsAttacking;
    [Tooltip("The boss's capsule collider")]
    CapsuleCollider bossCapsuleCollider;
    [Tooltip("The boss's rigidbody")]
    Rigidbody bossRigidbody;
    public bool phaseChanging;
    private AgentLinkMoverRobocapo linkMover;
    [Tooltip("The boss's nav mesh agent")]
    public NavMeshAgent bossNavAgent;
    public GameObject bulletShotTargetL, bulletShotTargetR;
    public float shotLoopTimer = 5;

    //Projectiles
    [Header("Projectiles Information")]
    [Tooltip("The bullet prefab that will be pooled")]
    [SerializeField] GameObject bulletPrefab;
    [Tooltip("The positions that bullets will be emitted from")]
    [SerializeField] Transform bulletPositionL, bulletPositionR;
    [SerializeField] float bulletShootForce, bulletUpwardForce;
    [SerializeField] float bulletSpread;
    [Tooltip("Checks if rapid fire is on/off")]
    [SerializeField] bool bulletRapidfireTrigger;

    //Player Information
    [Header("Player Information")]
    [Tooltip("The player")]
    public Transform player;
    [Tooltip("A target to allow the boss to aim at the player's center")]
    public Transform playerTarget;
    ModifiedTPC mtpc;

    //AI Information
    [Header ("AI Information")]
    [Tooltip("This references the area mask in the nav mesh")]
    LayerMask areaMask;
    [Tooltip("A random number generated for a random attack to be used")]
    public int randAttack;
    [Tooltip("A random number generated to randomize the boss' pathfinding")]
    int randPath;
    [Tooltip("How far the boss can see")]
    public float fovRadius, fovLookRadius;
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
    [SerializeField] float attackCooldown;
    [Tooltip("The waypoints that the boss travels to during phase 3")]
    public GameObject[] bossWaypoints;
    public int bossWaypointIndex;
    [Tooltip("Min = 0 or first waypoint, Max = Total number +1")]
    public int bossWaypointMin, bossWaypointMax;

    //Hitbox Information
    [Header("Hitbox Information")]
    [Tooltip("The boss's hurtbox to receive damage from the player")]
    public List<GameObject> hurtboxes;
    [Tooltip("This is used to time when the boss's hitboxes should dissapear")]
    public float InvincibleFrameTimer = 0.25f;
    [Tooltip("Checks if the boss is hit by the player")]
    public bool hitTick = false;
    public bool GuardUp;

    //Melee Hitboxes
    [Tooltip("Trigger GameObject for Melee Attacks")]
    [SerializeField] GameObject meleeTriggerSphere;
    [Tooltip("Hitboxes for phase 1")]
    public List<GameObject> attackHitboxesMelee;

    //Particles
    [Header("Particles and Effects")]
    [SerializeField] GameObject muzzleParticleEffect;
    [Tooltip("Particles when the boss takes damage")]
    public GameObject damageParticles;
    [Tooltip("Particles when the boss jumps")]
    public GameObject jumpTrail;
    public AudioSource soundBullet;
    public AudioSource soundDamaged;
    public GameObject ShieldParticle;

    //Managers
    int bulletPoolManager = 0;


    void Awake()
    {
        instance = this;
        bossAnimator = GetComponent<Animator>();
        bossCapsuleCollider = GetComponent<CapsuleCollider>();
        bossRigidbody = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bossNavAgent = GetComponent<NavMeshAgent>();
        linkMover = GetComponent<AgentLinkMoverRobocapo>();
        linkMover.OnLinkStart += HandleLinkStart;
        linkMover.OnLinkEnd += HandleLinkEnd;
    }

    void Start()
    {
        //Starts phase 1
        currentphase = 1;
        StartCoroutine(Phase1Pattern());
        Phase1Stats();
    }

    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, bossPlayerDetector); //checks for sight range
        playerInAttackRange = Physics.CheckSphere(meleeTriggerSphere.transform.position, attackRange, bossPlayerDetector); //checks for attack
        bossHealthSlider.value = bossHealth; //Updates the boss's health each frame
        PlayerTracking();
        PhaseStates();
        InvincibilityDetection();
        FinisherCheck();
        BossGroundCheck();
        RenderCheck();
    }

    void FixedUpdate()
    {
        BossAnimations();
    }

    void PhaseStates()
    {
        if (currentphase == 1)
        {
            Phase1Pattern();
            if (!playerInSightRange && !playerInAttackRange) BossPatroling();
            if (playerInSightRange && !playerInAttackRange) BossChasePlayer();
            if (playerInAttackRange && playerInSightRange) BossAttackPlayer();
        }
        if (currentphase == 1 && bossHealth <= 750) //Transition to phase 2
        {
            playerInAttackRange = false;
            Phase2Transition();
            currentphase = 2;
        }
        if (currentphase == 2)
        {
            Phase2Pattern();
            Phase2Stats();
        }
        if (currentphase == 2 && bossHealth <= 500)
        {
            playerInAttackRange = false;
            Phase3Transition();
            Phase3Stats();
            currentphase = 3;
        }
        if (currentphase == 3)
        {
            Phase3Pattern();
            if (!playerInSightRange && !playerInAttackRange) BossPatroling();
            if (playerInSightRange && !playerInAttackRange) BossChasePlayer();
            if (playerInAttackRange && playerInSightRange) BossAttackPlayer();
        }
        if (currentphase == 3 && bossHealth <= 250)
        {
            playerInAttackRange = false;
            Phase4Transition();
            Phase4Stats();
            currentphase = 4;
        }
        if (currentphase == 4)
        {
            Phase4Pattern();
            if (!playerInSightRange && !playerInAttackRange) BossPatroling();
            if (playerInSightRange && !playerInAttackRange) BossChasePlayer();
            if (playerInAttackRange && playerInSightRange) BossAttackPlayer();
        }
        if (bossHealth <= 0)
        {
            DeathTransition();
            currentphase = 5;
            dead = true;
            bossNavAgent.speed = 0;
            //Code for transition to next scene
        }
    }

    private void HandleLinkStart()
    {

    }
    private void HandleLinkEnd()
    {

    }

    void BossAnimations()
    {
        bossAnimator.SetFloat("isWalking", Mathf.Abs(bossNavAgent.speed));

        if(currentphase == 1)
        {
            if (playerInAttackRange == true)
            {
                if (randAttack == 1)
                {
                    bossAnimator.SetTrigger("3HitCombo");
                }
                if (randAttack == 2)
                {
                    bossAnimator.SetTrigger("3HitCombo");
                }
                if (randAttack == 3)
                {
                    bossAnimator.SetTrigger("3HitCombo");
                }
                if (randAttack == 4)
                {
                    bossAnimator.SetTrigger("WindUpOverhead");
                }
                if (randAttack == 5)
                {
                    bossAnimator.SetTrigger("WindUpOverhead");
                }
                if (randAttack >= 6)
                {
                    randAttack = Random.Range(1, 6);
                }
            } 
        }
        if(currentphase == 2)
        {
            if (randAttack == 1)
            {
                bossAnimator.SetTrigger("Gunspin");
            }
            if (randAttack == 2)
            {
                bossAnimator.SetTrigger("RangedShot");
            }
            if (randAttack >= 3)
            {
                randAttack = Random.Range(1, 3);
            }
        }
        if(currentphase == 3)
        {
            if(playerInAttackRange == true)
            {
                if (randAttack == 1)
                {
                    bossAnimator.SetTrigger("TripleStab");
                }
                if (randAttack == 2)
                {
                    bossAnimator.SetTrigger("DoubleWind");
                }
                if (randAttack == 3)
                {
                    bossAnimator.SetTrigger("StabDash");
                }
                if (randAttack >= 4)
                {
                    randAttack = Random.Range(1, 4);
                }
            }
        }
        if(currentphase == 4)
        {
            if(playerInAttackRange == true)
            {
                if(randAttack >= 1 && randAttack <= 3)
                {
                    bossAnimator.SetTrigger("3HitCombo");
                }
                if(randAttack == 4)
                {
                    bossAnimator.SetTrigger("WindUpOverhead");
                }
                if(randAttack == 5)
                {
                    bossAnimator.SetTrigger("TripleStab");
                }
                if(randAttack == 6)
                {
                    bossAnimator.SetTrigger("DoubleWind");
                }
                if(randAttack == 7)
                {
                    bossAnimator.SetTrigger("StabDash");
                }
            }
            else
            {
                if(randAttack >= 1 && randAttack <= 4)
                {
                    bossAnimator.SetTrigger("RangedShot");
                }
                if(randAttack >= 5 && randAttack <= 7)
                {
                    bossAnimator.SetTrigger("Gunspin");
                }
            }
            if(randAttack >= 8)
            {
                randAttack = Random.Range(1, 8);
            }
        }
    }

    IEnumerator Phase1Pattern()
    {
        while (true && !dead && currentphase == 1)
        {
            yield return new WaitForSeconds(0); //I don't know why this works, but it crashes Unity if i don't put this here
            if (playerInAttackRange)
            {
                randAttack = Random.Range(1, 6);
                yield return new WaitForSeconds(attackCooldown);
            }
        }
    }

    void Phase1Stats()
    {
        attackCooldown = 6f;
        bossNavAgent.speed = bossMoveSpeedP1;
        bossNavAgent.angularSpeed = 200;
        bossNavAgent.acceleration = 50;
        bossNavAgent.stoppingDistance = 0;
        bossNavAgent.autoBraking = true;
        bossAnimator.SetFloat("runMultiplyer", 1f);
    }

    void Phase2Transition()
    {
        StopAllCoroutines();
        //bossAnimator.SetTrigger("phase2");
    }

    IEnumerator Phase2Pattern()
    {
        while (true && !dead && currentphase == 2)
        {
            yield return new WaitForSeconds(0); //I don't know why this works, but it crashes Unity if i don't put this here
            if (playerInLos)
            {
                randAttack = Random.Range(1, 3);
                yield return new WaitForSeconds(attackCooldown);
            }
        }
    }

    void Phase2Stats()
    {
        attackCooldown = 5f;
        bossNavAgent.speed = bossMoveSpeedP2;
        bossNavAgent.angularSpeed = 225;
        bossNavAgent.acceleration = 50;
        bossNavAgent.stoppingDistance = 0;
        bossNavAgent.autoBraking = true;
        bossAnimator.SetFloat("runMultiplyer", 1.2f);
    }

    void Phase3Transition()
    {
        StopAllCoroutines();
    }

    IEnumerator Phase3Pattern()
    {
        while (true && !dead && currentphase == 3)
        {
            yield return new WaitForSeconds(0); //I don't know why this works, but it crashes Unity if i don't put this here
            if (playerInAttackRange)
            {
                randAttack = Random.Range(1, 4);
                yield return new WaitForSeconds(attackCooldown);
            }
        }
    }
    void Phase3Stats()
    {
        attackCooldown = 4f;
        bossNavAgent.speed = bossMoveSpeedP3;
        bossNavAgent.angularSpeed = 250;
        bossNavAgent.acceleration = 75;
        bossNavAgent.stoppingDistance = 0;
        bossNavAgent.autoBraking = true;
        bossAnimator.SetFloat("runMultiplyer", 1.5f);
    }

    IEnumerator Phase4Pattern()
    {
        while (true && !dead && currentphase == 4)
        {
            yield return new WaitForSeconds(0); //I don't know why this works, but it crashes Unity if i don't put this here
            if (playerInAttackRange)
            {
                randAttack = Random.Range(1, 8);
                yield return new WaitForSeconds(attackCooldown);
            }
        }
    }

    void Phase4Transition()
    {
        StopAllCoroutines();
    }

    void Phase4Stats()
    {
        attackCooldown = 3f;
        bossNavAgent.speed = bossMoveSpeedP4;
        bossNavAgent.angularSpeed = 300;
        bossNavAgent.acceleration = 80;
        bossNavAgent.stoppingDistance = 0;
        bossNavAgent.autoBraking = true;
        bossAnimator.SetFloat("runMultiplyer", 2f);
    }

    void DeathTransition()
    {
        StopAllCoroutines();
    }

    void BossPatroling() //The boss moving in random directions
    {
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
        if (distanceToWalkPoint.magnitude < 3f && currentphase == 3)
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
        bossNavAgent.SetDestination(player.position);
    }

    void BossAttackPlayer()
    {
        bossNavAgent.SetDestination(transform.position); //This forces the boss to stay in place during attacks
    }

    void PlayerTracking() //Tracks the player's position so the boss faces in their direction
    {
        if (playerTracking)
        {
            Vector3 targetPostition = new Vector3(player.position.x, this.transform.position.y, player.position.z); //The Y position uses the boss's Y position so that the boss does not rotate vertically based on the player's vertical position
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

    public void MeleeHitboxOn()
    {
        foreach (var hitboxMelee in attackHitboxesMelee)
        {
            hitboxMelee.SetActive(true);
        }
    }

    public void MeleeHitboxOff()
    {
        foreach (var hitboxMelee in attackHitboxesMelee)
        {
            hitboxMelee.SetActive(false);
        }
    }

    IEnumerator RapidFire()
    {
        bulletRapidfireTrigger = true;
        while (bulletRapidfireTrigger == true)
        {
            bulletPoolManager = RC_ObjectPool.instance.GetPooledObjectManaged(RC_ObjectPool.instance.pooledBullets, bulletPoolManager, bulletShotTargetL.transform, bulletPositionL, .5f, RC_ObjectPool.instance.pooledMuzzleFlashes, bulletShootForce/3);
            bulletPoolManager = RC_ObjectPool.instance.GetPooledObjectManaged(RC_ObjectPool.instance.pooledBullets, bulletPoolManager, bulletShotTargetR.transform, bulletPositionR, .5f, RC_ObjectPool.instance.pooledMuzzleFlashes, bulletShootForce/3);
            soundBullet.Play();
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void RapidFireStop()
    {
        bulletRapidfireTrigger = false;
        StopCoroutine(RapidFire());
    }

    void FireBulletsL()
    {
        bulletPoolManager = RC_ObjectPool.instance.GetPooledObjectManaged(RC_ObjectPool.instance.pooledBullets, bulletPoolManager, playerTarget.transform, bulletPositionL, 0f, RC_ObjectPool.instance.pooledMuzzleFlashes, bulletShootForce);
        soundBullet.Play();
    }
    void FireBulletsR()
    {
        bulletPoolManager = RC_ObjectPool.instance.GetPooledObjectManaged(RC_ObjectPool.instance.pooledBullets, bulletPoolManager, playerTarget.transform, bulletPositionR, 0f, RC_ObjectPool.instance.pooledMuzzleFlashes, bulletShootForce);
        soundBullet.Play();
    }

    void InvincibilityDetection()
    {
        //If the boss takes damage, start the invincibility timer, disable hurtbox
        if (hitTick && InvincibleFrameTimer > 0 && !phaseChanging)
        {
            InvincibleFrameTimer -= Time.deltaTime;
            foreach (var hurtbox in hurtboxes)
            {
                hurtbox.SetActive(false);
            }
            soundDamaged.Play();
        }
        //enable hurtboxes after the timer is done
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

    void BossGroundCheck()
    {
        bossGrounded = Physics.CheckSphere(transform.position, bossGroundedRadius, bossTerrainDetector);
        if (bossGrounded)
        {
            bossAnimator.SetBool("bossGrounded", true);
        }
        else
        {
            bossAnimator.SetBool("bossGrounded", false);
        }
    }

    void FinisherCheck()
    {
        Vector3 bossPlayerDistance = transform.position - player.transform.position;
        //If the player reaches the end, grabs the weapon, and approaches the boss
        if (bossHealth <= 0)
        {
            if (bossPlayerDistance.magnitude < 5f)
            {
                Debug.Log("Boss is dead");

                //Placeholder for the finishing cutscene
                //Destroy(gameObject);
            }
        }
    }

    public void TakeDamage()
    {
        bossHealth -= 25;
        bossHealthSlider.value = bossHealth;
    }

    public void ShotLoopTimer()
    {
        shotLoopTimer -= Time.deltaTime;
        if (shotLoopTimer <= 0)
        {
            shotLoopTimer = 5;
            bossAnimator.SetTrigger("jumpTimer");
        }
    }
    
    public void ActivateGuard()
    {
        ShieldParticle.SetActive(true);
        GuardUp = true;
    }

    public void DeActivateGuard()
    {
        ShieldParticle.SetActive(false);
        GuardUp = false;
    }

    public void RenderCheck()
    {
        if (GetComponent<Renderer>().isVisible)
        {
            bossLocator.instance.bossMarker.enabled = false;
        }
        else
        {
            bossLocator.instance.bossMarker.enabled = true;
        }
    }
}
