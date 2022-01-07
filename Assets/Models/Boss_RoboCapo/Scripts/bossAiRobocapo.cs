using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent), typeof(AgentLinkMoverRobocapo))]

public class bossAiRobocapo : MonoBehaviour
{

    public static bossAiRobocapo instance;
    //General
    public Animator bossAnimator;
    [Header("Boss Information")]
    [Tooltip("The boss's health")]
    public int bossHealth;
    [Tooltip("The current phase of the boss")]
    [SerializeField] int currentphase;
    [Tooltip("A bool that determines if the boss is dead")]
    [SerializeField] bool dead = false;
    [Tooltip("The boss's health slider")]
    [SerializeField] Slider bossHealthbar;
    [Tooltip("The movement speed of the boss during each phase")]
    public float bossMoveSpeedP1, bossMoveSpeedP2, bossMoveSpeedP3, bossMoveSpeedP4;
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
    [SerializeField] bool playerTracking = false;
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
    //Hitboxes
    [Header("Hitbox Information")]
    [Tooltip("A gameobject with a trigger that detects the player")]
    [SerializeField] GameObject attackTrigger;
    [Tooltip("Hitboxes for the boss's weapons during attacks")]
    [SerializeField] List<GameObject> attackHitboxesWeapons;
    [Tooltip("The boss's hurtbox to receive damage from the player")]
    public List<GameObject> hurtboxes;
    //Particles
    [Header("Particles and Effects")]
    [Tooltip("Particle Locations")]
    [SerializeField] Transform particlePlaceholderLocation1;
    [Tooltip("Particles")]
    [SerializeField] GameObject particlePlaceholder1;
    [Tooltip("This bool is true when RC is vulnerable to attack and false when he is capable of blocking")]
    public bool vulnurable;
    [Tooltip("This bool is true when RC is stunned")]
    public bool stunned;

    //Weapon Colliders
    [SerializeField] GameObject rightBayonet;
    [SerializeField] GameObject lefttBayonet;

    //Managers
    int bulletPoolManager = 0;
    
    


    void Awake()
    {
        instance = this;

        bossAnimator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bossNavAgent = GetComponent<NavMeshAgent>();
        bossCapsuleCollider = GetComponent<CapsuleCollider>();

        linkMover = GetComponent<AgentLinkMoverRobocapo>();

        bossHealthbar.maxValue = bossHealth;
        //linkMover.OnLinkStart += HandleLinkStart;
        //linkMover.OnLinkEnd += HandleLinkEnd;

        //Add to the hurtbox list
        attackHitboxesWeapons.Add(rightBayonet);

    }

    void Start()
    {
        //bossAnimator.SetTrigger("Introduction");
        StartCoroutine(Phase1Pattern()); //Starts phase 1
        currentphase = 1;
        Phase1Stats();
    }


    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, bossPlayerDetector); //checks for sight range
        playerInAttackRange = Physics.CheckSphere(attackTrigger.transform.position, attackRange, bossPlayerDetector); //checks for attack range
        PlayerTracking();
        PhaseAiStates();

        ///The below code is unessesary, just set the health bar = health when he takes damage
        //bossHealthbar.value = bossHealth; 

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
            if (currentphase == 1)
            {
                if (randAttack == 1)
                {
                    bossAnimator.SetTrigger("attack1");
                }
                if (randAttack == 2)
                {
                    bossAnimator.SetTrigger("attack2");
                }
                if (randAttack == 3)
                {
                    bossAnimator.SetTrigger("attack3");
                }
                if (randAttack == 4 && playerInAttackRange)
                {
                    randAttack = Random.Range(1, 2);
                }
            }
            if (currentphase == 2)
            {
                if (randAttack == 1)
                {
                    bossAnimator.SetTrigger("shoot1");
                }
                if (randAttack == 2)
                {
                    bossAnimator.SetTrigger("shoot2");
                }
                if (randAttack == 3)
                {
                    bossAnimator.SetTrigger("shoot3");
                }
                if (randAttack == 4 && playerInAttackRange)
                {
                    randAttack = Random.Range(1, 2);
                }
            }

        }
    }

    void PhaseAiStates()
    {
        if (currentphase == 1)
        {
            Phase1Pattern();
            if (!playerInSightRange && !playerInAttackRange) BossPatroling();
            if (playerInSightRange && !playerInAttackRange) BossChasePlayer();
            if (playerInSightRange && playerInAttackRange) BossAttackPlayer();
            if (bossHealth <= 750)
            {
                attackTrigger.SetActive(false);
                Phase2Transition();
                currentphase = 2;
            }
        }
        if (currentphase == 2)
        {
            Phase2Pattern();
            //Searching for the player tag within its FOV radius
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, fovRadius, bossPlayerDetector);
            if (!playerInSightRange && !playerInLos) BossPatroling(); //Patrol if the boss can't find the player
            if (playerInSightRange && !playerInLos) BossChasePlayer(); //Chase the player if they are in sight range
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
            if (playerInLos) BossAttackPlayer(); 
        }
        if (currentphase == 3)
        {

        }
        if (currentphase == 4)
        {

        }
        if (currentphase == 5)
        {
            dead = true;
        }
    }
    IEnumerator Phase1Pattern()
    {
        attackTrigger.SetActive(true);
        while (true && !dead)
        {
            yield return new WaitForSeconds(0);
            if (playerInAttackRange)
            {
                //Debug.Log("I hit you with imaginary tonfas");
                randAttack = Random.Range(1, 2);
                yield return new WaitForSeconds(timeBetweenAttacksP1);
            }
        }
    }
    void Phase1Stats()
    {
        //This code should be at the end of the introduction animation event state, but for now it is here so that the boss can move
        bossNavAgent.speed = bossMoveSpeedP1;
        bossNavAgent.angularSpeed = 200;
        bossNavAgent.acceleration = 10;
        bossNavAgent.stoppingDistance = 0;
        bossNavAgent.autoBraking = true;
    }
    void Phase2Transition()
    {
        StopAllCoroutines();
        bossAnimator.SetTrigger("phase2");
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
    void Phase2Stats()
    {
        //This code should be at the end of the introduction animation event state, but for now it is here so that the boss can move
        bossNavAgent.speed = bossMoveSpeedP2;
        bossNavAgent.angularSpeed = 250;
        bossNavAgent.acceleration = 20;
        bossNavAgent.stoppingDistance = 0;
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
        if (!dead)
        {
            bossNavAgent.SetDestination(player.position);
        }
    }
    void BossAttackPlayer()
    {
        bossNavAgent.SetDestination(transform.position); //This forces the boss to stay in place during attacks

    }
    void PlayerTracking() //Tracks the player's position so the boss faces in their direction
    {
        Vector3 targetPostition = new Vector3(player.position.x, this.transform.position.y, player.position.z); //The Y position uses the boss's Y position so that the boss does not rotate vertically based on the player's vertical position
        if (playerTracking)
        {
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
        if (vulnurable)
        {
            bossHealth -= damage;
            bossHealthbar.value = bossHealth;
            //bossAnimator.SetTrigger("stagger");
        }

        else
        {
            Debug.Log("the boss blocked the Attack");

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

    //Animation events
    
    void ActivateRightBayonet()
    {
        rightBayonet.SetActive(true);
    }

    void DisableRightBayonet()
    {
        rightBayonet.SetActive(true);
    }

}
