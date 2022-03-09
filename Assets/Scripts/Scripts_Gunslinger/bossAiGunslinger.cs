using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using UnityEngine.UI;

public class bossAiGunslinger : MonoBehaviour
{
    public static bossAiGunslinger instance;

    //General
    [Header("General Boss Information")]
    public Animator bossAnimator;
    [Tooltip("The boss's health number")]
    public int bossHealth;
    [Tooltip("The current phase of the boss")]
    public int currentphase;
    [Tooltip("A bool that determines if the boss is dead")]
    public bool dead;
    [Tooltip("The boss's health slider")]
    public Slider bossHealthbar;
    [Tooltip("The movement speed of the boss during each phase")]
    public float bossMoveSpeedP1, bossMoveSpeedP2, bossMoveSpeedP3, bossMoveSpeedP4;
    [Tooltip("Checks if the boss is in the middle of an attack animation")]
    public bool bossIsAttacking;
    [Tooltip("The boss's capsule collider")]
    CapsuleCollider bossCapsuleCollider;
    [Tooltip("The boss's rigidbody")]
    Rigidbody bossRigidbody;

    //Information about projectiles
    [Header("Projectiles Information")]
    [SerializeField] Transform revolverBulletPosition;
    [SerializeField] GameObject revolverBullet;
    [SerializeField] float revolverBulletShootForce, revolverBulletUpwardForce;
    [SerializeField] float revolverBulletSpread;

    //Information regarding the player
    [Header("Player Information")]
    [Tooltip("The player")]
    public Transform player;
    [Tooltip("A target to allow the boss to aim at the player's center")]
    public Transform playerTarget;
    ModifiedTPC mtpc;

    //Information regarding the boss's AI
    [Header("Boss AI Information")]
    [Tooltip("The boss's nav mesh agent")]
    public NavMeshAgent bossNavAgent;
    [Tooltip("This references the area mask in the nav mesh")]
    LayerMask areaMask;
    [Tooltip("A random number generated for a random attack to be used")]
    public int randAttack;
    [Tooltip("How far the boss can see")]
    public float fovRadius, fovLookRadius;
    [Tooltip("How wide the boss can see")]
    [Range(0, 360)] public float fovAngle, fovLookAngle;
    [Tooltip("detect which layer the boss can detect/interact with")]
    [SerializeField] LayerMask bossTerrainDetector, bossPlayerDetector, bossObstacleDetector;
    [Tooltip("Detects if the boss is following the player")]
    [SerializeField] bool followingPlayer;
    [Tooltip("The range of how far the AI can be before detecting and attacking")]
    [SerializeField] float sightRange, attackRange;
    [Tooltip("Checks if the player is within range")]
    public bool playerInSightRange, playerInAttackRangeP1, playerInAttackRangeP2, playerInAttackRangeP3, playerInAttackRangeP4, playerInLos;
    [Tooltip("A bool used to force the boss to look at the player")]
    public bool playerTracking;
    [Tooltip("A list of all the jump links in the scene")]
    public List<GameObject> jumpLinks;
    [Space(10)]
    //Boss patroling
    [Tooltip("The walkpoint that the AI goes to")]
    public Vector3 walkPoint;
    [Tooltip("Checks if there is a walkpoint")]
    bool walkPointSet;
    [Tooltip("How far the AI can detect a flat surface to walk to")]
    [SerializeField] float walkPointRange;
    [Space(10)]
    //Boss attacking
    [Tooltip("the cooldown for attacks")]
    [SerializeField] float attackCooldown;
    [Tooltip("The minimum and maximum timer range between attacks")]
    [SerializeField] float attackCooldownMin, attackCooldownMax;
    [Tooltip("The boss's agent link mover")]
    private AgentLinkMoverGunslinger linkMover;
    [Tooltip("The waypoints that the boss travels")]
    public GameObject[] bossWaypoints;
    public int bossWaypointIndex;
    [Tooltip("Min = 0 or first waypoint, Max = Total number +1")]
    public int bossWaypointMin, bossWaypointMax;
    [Tooltip("The Navmesh links that allow the boss to jump")]
    public NavMeshLink navMeshLinkScript;
    [Tooltip("bool that tracks if the boss is using a jump attack")]
    public bool jumpAttack;
    [Tooltip("Checks if boss is grounded")]
    public bool bossGrounded;
    [Tooltip("the sphere radius of the boss's ground detection")]
    float bossGroundedRadius = 4f;
    [Tooltip("bool to check if the boss is changing phases")]
    public bool phaseChanging;

    [Header("Hitbox Information")]
    public List<GameObject> hurtboxes;
    public float InvincibleFrameTimer = 0.25f;
    public bool hitTick = false;
    public int damageDaggerSpin;
    [Tooltip("the hitbox for gunslinger's dagger spin")]
    public List<GameObject> daggerSpinHitboxes;

    [Header("Fixed Damage Attacks")] 
    public int placeholder1;

    [Header("Particles and Effects")]
    public int placeholder2;

    int revolverBulletPoolManager = 0;


    void Awake()
    {
        instance = this;

        bossAnimator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bossNavAgent = GetComponent<NavMeshAgent>();
        bossCapsuleCollider = GetComponent<CapsuleCollider>();

        linkMover = GetComponent<AgentLinkMoverGunslinger>();
        linkMover.OnLinkStart += HandleLinkStart;
        linkMover.OnLinkEnd += HandleLinkEnd;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentphase = 1;
        Phase1Stats();
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, bossPlayerDetector); //checks for sight range
        //bossHealthbar.value = bossHealth; //Updates the boss's health each frame
        PlayerTracking();
        PhaseStates();
        InvincibilityDetection();
        FinisherCheck();
        BossGroundCheck();
    }
    void FixedUpdate()
    {
        BossAnimations();
    }

    void BossAnimations()
    {
        if (currentphase == 1)
        {
            bossAnimator.SetFloat("isWalking", Mathf.Abs(bossNavAgent.speed));
        }
    }

    private void HandleLinkStart() //Controls animations when the boss interacts with jumpable navmesh links
    {
        if (currentphase == 1)
        {
            bossAnimator.SetTrigger("jumpP1");
        }
    }

    private void HandleLinkEnd() //Controls animations when the boss interacts with jumpable navmesh links
    {

    }

    public void BossAttackPlayer()
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

    void CooldownTimer()
    {
        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0)
        {
            attackCooldown = Random.Range(attackCooldownMin, attackCooldownMax);
        }
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
        }
    }

    void FinisherCheck()
    {
        Vector3 bossPlayerDistance = transform.position - player.transform.position;
        //If the player reaches the end, grabs the weapon, and approaches the boss
        if (currentphase == 4 && ModifiedTPC.instance.playerHasBossWeapon == true)
        {
            if (bossPlayerDistance.magnitude < 5f)
            {
                Debug.Log("Boss is dead");
                //Placeholder for the finishing cutscene
                Destroy(gameObject);
            }
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

    void PhaseStates()
    {
        if (currentphase == 1)
        {
            CooldownTimer();
            Phase1Pattern(); //Phase 1
            playerInAttackRangeP1 = Physics.CheckSphere(transform.position, attackRange, bossPlayerDetector);
        }
    }

    void Phase1Pattern()
    {
        BossPatroling();
        /*if (attackCooldown <= 0.05)
        {
            //When the boss's cooldown reaches 0, generate a number to choose which attack to hit the player with
            randAttack = Random.Range(1, 7);
            BossAttackPlayer();
        }*/
    }
    void Phase1Stats()
    {
        attackCooldown = 7f;
        bossNavAgent.speed = bossMoveSpeedP1;
        attackRange = 5;
        sightRange = 1000;
        bossNavAgent.angularSpeed = 500;
        bossNavAgent.acceleration = 150;
        bossNavAgent.stoppingDistance = 2;
        bossNavAgent.autoBraking = true;
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
        if (currentphase == 1)
        {
            bossNavAgent.SetDestination(player.position);
        }
    }
    void AttackRevolverShoot()
    {
        revolverBulletPoolManager = objectPoolGunslinger.instance.GetPooledObjectManaged(objectPoolGunslinger.instance.pooledBullets, revolverBulletPoolManager, playerTarget, revolverBulletPosition, revolverBulletSpread, null, revolverBulletShootForce);
    }
    void ActivateHitboxDaggerSpin()
    {
        foreach (var hitboxp in daggerSpinHitboxes)
        {
            hitboxp.SetActive(true);
        }
    }
    void DeactivateHitboxDaggerSpin()
    {
        foreach (var hitboxp in daggerSpinHitboxes)
        {
            hitboxp.SetActive(false);
        }
    }
}
