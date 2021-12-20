using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent), typeof(AgentLinkMoverObsidian))]
public class bossAiObsidian : MonoBehaviour
{
    //General information about the Boss
    Animator bossAnimator;
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
    public float bossMoveSpeedP1, bossMoveSpeedP2, bossMoveSpeedP3;
    [Tooltip("Checks if the boss is in the middle of an attack animation")]
    public bool bossIsAttacking;
    [Tooltip("The boss's capsule collider")] 
    CapsuleCollider bossCapsuleCollider;
    [Tooltip("The boss's rigidbody")] 
    Rigidbody bossRigidbody;
    [Tooltip("The target gameobject that the boss will charge into when using the charge attack")]
    [SerializeField] GameObject chargeTarget;
    [SerializeField] Transform phase2LaserDirection;
    public bool phaseChanging;

    //Information about projectiles
    [Header("Projectiles Information")]
    //Phase2
    [Tooltip("The laser gameobject that the boss shoots during phase 2")]
    public GameObject phase2Laser;
    [SerializeField] Transform p2BulletPosition;
    [SerializeField] GameObject p2Bullet;
    [SerializeField] float p2ShootForce, p2UpwardForce;
    [SerializeField] float p2BulletSpread;
    [Tooltip("Checks if rapid fire is on/off during phase 2 attack 2")]
    [SerializeField] bool p2rpTrigger;
    [Space(10)]
    //Phase3 Weapon
    [SerializeField] GameObject obsidianWeapon;
    [SerializeField] Transform p3WeaponPosition;
    [SerializeField] Transform p3BossOrigin;
    [SerializeField] float p3WeaponShootForce;
    [Space(10)]
    //Phase3 Blaster
    [SerializeField] Transform p3BlasterPosition;
    [SerializeField] GameObject p3BlasterBullet;
    [SerializeField] float p3BlasterShootForce, p3BlasterUpwardForce;
    [SerializeField] float p3BlasterBulletSpread;
    [Tooltip("Checks if rapid fire is on/off during phase 3 attack 1")]
    [SerializeField] bool p3BlasterTrigger;
    [Space(10)]
    //Phase3 Meteor
    [SerializeField] Transform p3MeteorPositionL;
    [SerializeField] Transform p3MeteorPositionR;
    [SerializeField] GameObject p3MeteorBullet;
    [SerializeField] float p3MeteorShootForce, p3MeteorUpwardForce;
    [SerializeField] float p3MeteorBulletSpread;
    [Tooltip("The target direction for the meteors to travel to")]
    [SerializeField] Transform p3MeteorTarget;
    [Space(10)]
    //Phase3 Grenades
    [SerializeField] Transform p3GrenadePosition;
    [SerializeField] GameObject p3GrenadeBullet;
    [SerializeField] float p3GrenadeShootForce, p3GrenadeUpwardForce;
    [SerializeField] float p3GrenadeBulletSpread;
    [Space(10)]
    //Phase3 Barrage
    [SerializeField] Transform p3BarragePositionL;
    [SerializeField] Transform p3BarragePositionR;
    [SerializeField] GameObject p3BarrageBullet;
    [SerializeField] float p3BarrageShootForce, p3BarrageUpwardForce;
    [SerializeField] float p3BarrageBulletSpread;

    //Information regarding the player
    [Header("Player Information")]
    [Tooltip("The player")] 
    public Transform player;
    [Tooltip("A target to allow the boss to aim at the player's center")] 
    public Transform playerTarget;

    //Information regarding the boss's AI
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
    //[SerializeField] float smoothRotationSpeed;
    //private Coroutine SmoothLookCoroutine;
    [Tooltip("detect which layer the boss can detect/interact with")]
    [SerializeField] LayerMask bossTerrainDetector, bossPlayerDetector, bossObstacleDetector;
    [Tooltip("Detects if the boss is following the player")] 
    [SerializeField] bool followingPlayer = true;
    [Tooltip("The range of how far the AI can be before detecting and attacking")] 
    [SerializeField] float sightRange, attackRange;
    [Tooltip("Checks if the player is within range")]
    public bool playerInSightRange, playerInAttackRangeP1, playerInAttackRangeP2, playerInLos;//, playerInAttackRangeP3;
    [Tooltip("A bool used to force the boss to look at the player")]
    public bool playerTracking = false;
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
    [Tooltip("The time between each attack for each phase")] 
    [SerializeField] float timeBetweenAttacks;
    [Tooltip("the cooldown for phase 3 attacks")]
    [SerializeField] float phase3Cooldown;
    [Tooltip("The minimum and maximum timer range between attacks during phase 3")]
    [SerializeField] float phase3CooldownMin, phase3CooldownMax;
    [Tooltip("How strong the velocity of the boss is while charging")]
    [SerializeField] Vector3 bossChargeVelocity;
    public bool obsidianIsLeaping;
    public bool obsidianIsCharging;
    private AgentLinkMoverObsidian linkMover;
    [Tooltip("The waypoints that the boss travels to during phase 3")]
    public GameObject[] bossWaypoints;
    public int bossWaypointIndex;
    [Tooltip("Min = 0 or first waypoint, Max = Total number +1")]
    public int bossWaypointMin, bossWaypointMax;
    

    [Header("Hitbox Information")]
    [Tooltip("Trigger GameObject for Phase 1 Attacks")] 
    [SerializeField] GameObject triggerP1;
    [Tooltip("Hitboxes for phase 1")]
    [SerializeField] List<GameObject> attackHitboxesPhase1;
    [Tooltip("Hitbox for phase 1 shockwave")]
    public GameObject shockwaveP1Hitbox;
    [Tooltip("Hitboxes for phase 2")]
    [SerializeField] List<GameObject> attackHitboxesPhase2;
    [Tooltip("Hitboxes for phase 3")]
    [SerializeField] List<GameObject> attackHitboxesPhase3;
    [Tooltip("Hitboxes for the boss's charge attack during phase 3")]
    public GameObject chargeHitboxPhase3;
    [Tooltip("Hitboxes for the boss's Jump Strike attack during phase 3")]
    public GameObject jumpHitboxPhase3;
    [Tooltip("The boss's hurtbox to receive damage from the player")]
    public List<GameObject> hurtboxes;

    [Header("Particles and Effects")]
    [Tooltip("Phase 1 attack 3 particle location")]
    [SerializeField] Transform p1Attack3Location;
    [Tooltip("Phase 1 attack 3 particle")]
    [SerializeField] GameObject p1Attack3Particle;
    [Tooltip("Phase 1 weapon trails")]
    public GameObject p1WeaponTrail;
    [Tooltip("Phase 1 attack 2 particle location")]
    [SerializeField] Transform p1Attack2Location;
    [Tooltip("Phase 1 attack 2 particle")]
    [SerializeField] GameObject p1Attack2Particle;
    [Tooltip("Phase 2 attack 2 particle location")]
    [SerializeField] Transform p2Attack2Location;
    [Tooltip("Phase 2 attack 2 particle location")]
    [SerializeField] GameObject p2Attack2Particle;
    [Tooltip("Phase 3 attack 1 particle")]
    [SerializeField] GameObject p3Attack1Particle;
    [Tooltip("Phase 3 attack 2 muzzle particle")]
    [SerializeField] GameObject p3Attack2Particle;
    [Tooltip("Phase 3 attack 4 muzzle particle")]
    [SerializeField] GameObject p3Attack4Particle;
    [Tooltip("Phase 3 attack 4 charge effect")]
    [SerializeField] GameObject p3Attack4ChargeEffect;
    [Tooltip("Phase 3 attack 5 muzzle particle")]
    [SerializeField] GameObject p3Attack5Particle;
    [Tooltip("Phase 3 attack 6 muzzle particle")]
    [SerializeField] GameObject p3Attack6Particle;
    [Tooltip("Phase 3 attack 6 charge effect")]
    [SerializeField] GameObject p3Attack6ChargeEffect;
    [Tooltip("Obsidian's Shield")]
    [SerializeField] GameObject bossShieldObsidian;

    int bulletPoolManager = 0;
    int blasterPoolManager = 0;
    int grenadePoolManager = 0;
    int LbarrageManager = 0;
    int RbarrageManager = 0;


    void Awake()
    {
        bossAnimator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bossNavAgent = GetComponent<NavMeshAgent>();
        bossCapsuleCollider = GetComponent<CapsuleCollider>();

        linkMover = GetComponent<AgentLinkMoverObsidian>();
        linkMover.OnLinkStart += HandleLinkStart;
        linkMover.OnLinkEnd += HandleLinkEnd;
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
        bossHealthbar.value = bossHealth; //Updates the boss's health each frame
        PlayerTracking();
        PhaseStates();
    }

    void FixedUpdate()
    {
        BossAnimations();
    }

    void PhaseStates()
    {
        if (currentphase == 1)
        {
            Phase1Pattern(); //Phase 1
            playerInAttackRangeP1 = Physics.CheckSphere(triggerP1.transform.position, attackRange, bossPlayerDetector);
            if (!playerInSightRange && !playerInAttackRangeP1) BossPatroling(); //Patrol if the boss can't find the player
            if (playerInSightRange && !playerInAttackRangeP1) BossChasePlayer(); //Chase the player if they are in sight range
            if (playerInAttackRangeP1 && playerInSightRange) BossAttackPlayer(); //Attack the player if they are in attack range
        }
        if (currentphase == 1 && bossHealth <= 600) //Transitions into phase 2
        {
            triggerP1.SetActive(false);
            playerInAttackRangeP1 = false;
            Phase2Transition();
            currentphase = 2;
        }
        if (currentphase == 2)
        {
            Phase2Pattern(); // Phase 2

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
        if (currentphase == 2 && bossHealth <= 300)
        {
            playerInLos = false;
            Phase3Transition();
            currentphase = 3;
        }
        if (currentphase == 3)
        {
            if (playerInSightRange) BossPatroling();
            Phase3Pattern();
            Phase3Timer();

        }
        if (currentphase == 3 && bossHealth <= 0)
        {
            Phase4Transition();
            currentphase = 4;
        }
        if (obsidianIsLeaping)
        {
            //If the boss is using its leap attacks, this disables the collision of the capsule collider until the boss lands on the player. This also slows down the leap if its destination is reached
            bossCapsuleCollider.isTrigger = true;
            Vector3 distanceToWalkPoint = transform.position - walkPoint;
            if (distanceToWalkPoint.magnitude < 1f)
            {
                bossNavAgent.speed = 5f;
                bossNavAgent.acceleration = 100f;
            }
            if (distanceToWalkPoint.magnitude > 1f)
            {
                bossNavAgent.SetDestination(player.transform.position);
            }
        }
    }

    private void HandleLinkStart() //Controls animations when the boss interacts with jumpable navmesh links
    {
        if (currentphase == 1)
        {
            bossAnimator.SetTrigger("jumpP1");
        }
        if (currentphase == 2)
        {
            bossAnimator.SetTrigger("jumpP2");
        }
        if (currentphase == 3 && !obsidianIsLeaping)
        {
            bossAnimator.SetTrigger("jumpP3");
            bossNavAgent.speed = bossMoveSpeedP3;
            bossNavAgent.acceleration = 100f;
            bossNavAgent.angularSpeed = 200f;
        }
    }

    private void HandleLinkEnd() //Controls animations when the boss interacts with jumpable navmesh links
    {
        if (currentphase == 1)
        {
            bossAnimator.SetTrigger("landP1");
        }
        if (currentphase == 2)
        {
            bossAnimator.SetTrigger("landP2");
        }
        if (currentphase == 3 && !obsidianIsLeaping)
        {
            bossAnimator.SetTrigger("landP3");
            bossNavAgent.speed = bossMoveSpeedP3;
            bossNavAgent.acceleration = 100f;
            bossNavAgent.angularSpeed = 200f;
        }
    }

    void BossAnimations()
    {
        if (currentphase == 1)
        {
            bossAnimator.SetFloat("isWalkingAxe", Mathf.Abs(bossNavAgent.speed));
            if (randAttack == 1)
            {
                bossAnimator.SetTrigger("axeAttack1");
            }
            if (randAttack == 2)
            {
                bossAnimator.SetTrigger("axeAttack2");
            }
            if (randAttack == 3)
            {
                bossAnimator.SetTrigger("axeAttack3");
            }
            if (randAttack == 4 && playerInAttackRangeP1)
            {
                randAttack = Random.Range(1, 4);
            }
        }
        if (currentphase == 2)
        {
            bossAnimator.SetFloat("isWalkingRailgun", Mathf.Abs(bossNavAgent.speed));
            if (randAttack == 1)
            {
                bossAnimator.SetTrigger("railgunAttack1");
            }
            if (randAttack == 2)
            {
                bossAnimator.SetTrigger("railgunAttack2");
            }
            if (randAttack == 3)
            {
                bossAnimator.SetTrigger("railgunAttack3");
            }
            if (randAttack == 4 && playerInLos)
            {
                randAttack = Random.Range(1, 4);
            }
        }
        if (currentphase == 3)
        {
            bossAnimator.SetFloat("isWalkingGauntlet", Mathf.Abs(bossNavAgent.speed));
            if (phase3Cooldown <= 0.05)
            {
                if (randAttack == 1)
                {
                    bossAnimator.SetTrigger("gauntletAttack1");
                }
                if (randAttack == 2)
                {
                    bossAnimator.SetTrigger("gauntletAttack2");
                }
                if (randAttack == 3)
                {
                    bossAnimator.SetTrigger("gauntletAttack3");
                }
                if (randAttack == 4)
                {
                    bossAnimator.SetTrigger("gauntletAttack4");
                }
                if (randAttack == 5)
                {
                    bossAnimator.SetTrigger("gauntletAttack5");
                }
                if (randAttack == 6)
                {
                    bossAnimator.SetTrigger("gauntletAttack6");
                }
                if (randAttack == 7)
                {
                    randAttack = Random.Range(1, 7);
                }
            }
        }
    }    

    IEnumerator Phase1Pattern()
    {
        triggerP1.SetActive(true);
        while (true && !dead)
        {
            //If the player is in attack range, generate a number to choose which attack to hit the player with
            yield return new WaitForSeconds(0);
            if (playerInAttackRangeP1)
            {
                randAttack = Random.Range(1, 4);
                yield return new WaitForSeconds(timeBetweenAttacks);
            }   
        }
    }

    void Phase1Stats()
    {
        timeBetweenAttacks = 5f;
        bossNavAgent.speed = bossMoveSpeedP1;
        attackRange = 9;
        sightRange = 1000;
        bossNavAgent.angularSpeed = 200;
        bossNavAgent.acceleration = 10;
        bossNavAgent.stoppingDistance = 0;
        bossNavAgent.autoBraking = true;
    }

    void Phase2Transition()
    {
        StopAllCoroutines();
        bossAnimator.SetTrigger("phase2");
        attackRange = 10;
        sightRange = 1000;
        timeBetweenAttacks = 6f;
    }

    IEnumerator Phase2Pattern()
    {
        while (true && !dead)
        {
            //If the player is in attack range, generate a number to choose which attack to hit the player with
            yield return new WaitForSeconds(0);
            if (playerInLos)
            {
                randAttack = Random.Range(1, 4);
                yield return new WaitForSeconds(timeBetweenAttacks);
            }
        }
    }

    void Phase3Transition()
    {
        StopAllCoroutines();
        bossAnimator.SetTrigger("phase3");
        attackRange = 100;
        bossNavAgent.acceleration = 100;
    }

    void Phase3Pattern()
    {
        if (phase3Cooldown <= 0.05)
        {
            //When the boss's cooldown reaches 0, generate a number to choose which attack to hit the player with
            randAttack = Random.Range(1, 7);
            BossAttackPlayer();
        }
    }

    void Phase3Timer() //The timer for the boss's cooldown before attacking
    {
        phase3Cooldown -= Time.deltaTime;
        if (phase3Cooldown <= 0)
        {
            phase3Cooldown = Random.Range(phase3CooldownMin, phase3CooldownMax);
        }
    }

    void Phase4Transition()
    {
        StopAllCoroutines();
        bossNavAgent.speed = 0f;
        bossNavAgent.acceleration = 1000;
        attackRange = 0f;
        sightRange = 0f;
        walkPoint = transform.position;
        bossAnimator.SetTrigger("phase4");
        dead = true;
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
        if (distanceToWalkPoint.magnitude < 2f && currentphase == 3)
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
        if (currentphase == 1 || currentphase == 2 || currentphase == 3)
        {
            bossNavAgent.SetDestination(player.position);
        }
    }

    void BossAttackPlayer()
    {
        bossNavAgent.SetDestination(transform.position); //This forces the boss to stay in place during attacks

    }

    void HitboxActivatedPhase1() //Activates the boss's hitboxes
    {
        foreach (var hitboxp1 in attackHitboxesPhase1)
        {
            hitboxp1.SetActive(true);
        }
    }

    public void HitboxDeactivatedPhase1() //Deactivates the boss's hitboxes
    {
        foreach (var hitboxp1 in attackHitboxesPhase1)
        {
            hitboxp1.SetActive(false);
        }
    }

    void HitboxActivatedPhase1Shockwave() //Activates the hitbox for the boss's shockwave attack
    {
        shockwaveP1Hitbox.SetActive(true);
    }

    public void HitboxDeactivatedPhase1Shockwave() //Deactivates the hitbox for the boss's shockwave attack
    {
        shockwaveP1Hitbox.SetActive(false);
    }

    void HitboxActivatedPhase2() //Activates the boss's hitboxes
    {
        foreach (var hitboxp2 in attackHitboxesPhase2)
        {
            hitboxp2.SetActive(true);
        }
    }

    public void HitboxDeactivatedPhase2() //Deactivates the boss's hitboxes
    {
        foreach (var hitboxp2 in attackHitboxesPhase2)
        {
            hitboxp2.SetActive(false);
        }
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

    void Phase1Attack2Effects() //Particle effects
    {
        Instantiate(p1Attack2Particle, p1Attack2Location.position, Quaternion.identity);
    }

    void Phase1Attack3Effects() //Particle effects
    {
        Instantiate(p1Attack3Particle, p1Attack3Location.position, Quaternion.identity);
    }

    void Phase1WeaponTrailOn() //Particle effects
    {
        p1WeaponTrail.SetActive(true);
    }

    void Phase1WeaponTrailOff() //Particle effects
    {
        p1WeaponTrail.SetActive(false);
    }

    void obsidianLaserOn() //Particle effects
    {
        phase2Laser.SetActive(true);
    }
    void obsidianLaserOff() //Particle effects
    {
        phase2Laser.SetActive(false);
    }

    void Phase2LaserLookAtPlayer() //Forces the laser to aim at the player
    {
        phase2Laser.transform.LookAt(playerTarget.transform);
    }

    IEnumerator Phase2RapidFire()
    {
        p2rpTrigger = true;
        while (p2rpTrigger == true)
        {
            
            bulletPoolManager = ObjectPool.instance.GetPooledObjectManaged(ObjectPool.instance.pooledBullets, bulletPoolManager, playerTarget, p2BulletPosition, p2BulletSpread, ObjectPool.instance.pooledmuzzleFlash, p2ShootForce);
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Phase2RapidFireStop() //Forces the rapid fire to stop
    {
        p2rpTrigger = false;
        StopCoroutine(Phase2RapidFire());
    }

    void Phase2LaserStraighten()
    {
        phase2Laser.transform.LookAt(phase2LaserDirection.position);
    }

    void Phase3WeaponToss()
    {
        //calculate direction from weapon to player
        Vector3 bulletDirection = p3WeaponPosition.transform.position - p3BossOrigin.transform.position;

        //instantiate the projectile/bullet
        GameObject p3CurrentWeapon = Instantiate(obsidianWeapon, p3WeaponPosition.position, p3WeaponPosition.rotation * Quaternion.Euler(50f, -20f, 0f));

        //add forces to bullet/projectile
        p3CurrentWeapon.GetComponent<Rigidbody>().AddForce(bulletDirection.normalized * p3WeaponShootForce, ForceMode.Impulse);
    }

    IEnumerator Phase3Blaster()
    {
        p3BlasterTrigger = true;
        while (p3BlasterTrigger == true)
        {
            blasterPoolManager = ObjectPool.instance.GetPooledObjectManaged(ObjectPool.instance.pooledBlasterBullets, blasterPoolManager, playerTarget, p3BlasterPosition, p3BlasterBulletSpread, ObjectPool.instance.pooledblastermuzzleFlash, p3BlasterShootForce);
            yield return new WaitForSeconds(0.05f);
        }
    }

    void Phase3BlasterStop() //Forces the rapid fire to stop
    {
        p3BlasterTrigger = false;
        StopCoroutine(Phase3Blaster());
    }

    void Phase3MeteorL()
    {
        //calculate direction from weapon to player
        Vector3 bulletDirection = p3MeteorTarget.transform.position - p3MeteorPositionL.transform.position;

        //calculate spread
        float bulletspreadX = Random.Range(-p3MeteorBulletSpread, p3MeteorBulletSpread);
        float bulletspreadY = Random.Range(-p3MeteorBulletSpread, p3MeteorBulletSpread);

        //calculate new direction with spread
        Vector3 bulletDirectionSpread = bulletDirection + new Vector3(bulletspreadX, bulletspreadY, 0);

        //instantiate the projectile/bullet
        GameObject p3CurrentMeteorBullet = ObjectPool.instance.MeteorL;
        p3CurrentMeteorBullet.transform.position = p3MeteorPositionL.position;
        p3CurrentMeteorBullet.SetActive(true);

        //Instantiate(p3Attack2Particle, p3MeteorPositionL.position, Quaternion.identity);
        GameObject leftMeteorPart = ObjectPool.instance.GetPooledObject(ObjectPool.instance.pooledmeteorVfxL, true);
        leftMeteorPart.transform.position = p3MeteorPositionL.position;
        leftMeteorPart.transform.rotation = p3MeteorPositionL.rotation;
        leftMeteorPart.SetActive(true);

        //rotate bullet/projectile to shoot direction
        p3CurrentMeteorBullet.transform.forward = bulletDirectionSpread.normalized;

        //add forces to bullet/projectile
        p3CurrentMeteorBullet.GetComponent<Rigidbody>().AddForce(bulletDirectionSpread.normalized * p3MeteorShootForce, ForceMode.Impulse);
    }
    void Phase3MeteorR()
    {
        //calculate direction from weapon to player
        Vector3 bulletDirection = p3MeteorTarget.transform.position - p3MeteorPositionR.transform.position;

        //calculate spread
        float bulletspreadX = Random.Range(-p3MeteorBulletSpread, p3MeteorBulletSpread);
        float bulletspreadY = Random.Range(-p3MeteorBulletSpread, p3MeteorBulletSpread);

        //calculate new direction with spread
        Vector3 bulletDirectionSpread = bulletDirection + new Vector3(bulletspreadX, bulletspreadY, 0);

        //instantiate the projectile/bullet
        GameObject p3CurrentMeteorBullet = ObjectPool.instance.MeteorR;
        p3CurrentMeteorBullet.transform.position = p3MeteorPositionR.position;
        p3CurrentMeteorBullet.SetActive(true);

        //Instantiate(p3Attack2Particle, p3MeteorPositionR.position, Quaternion.identity);
        GameObject rightMeteorPart = ObjectPool.instance.GetPooledObject(ObjectPool.instance.pooledmeteorVfxR, true);
        rightMeteorPart.transform.position = p3MeteorPositionR.position;
        rightMeteorPart.transform.rotation = p3MeteorPositionR.rotation;
        rightMeteorPart.SetActive(true);

        //rotate bullet/projectile to shoot direction
        p3CurrentMeteorBullet.transform.forward = bulletDirectionSpread.normalized;

        //add forces to bullet/projectile
        p3CurrentMeteorBullet.GetComponent<Rigidbody>().AddForce(bulletDirectionSpread.normalized * p3MeteorShootForce, ForceMode.Impulse);
    }


    void Phase3GrenadeLauncher()
    {
        grenadePoolManager = ObjectPool.instance.GetPooledObjectManaged(ObjectPool.instance.pooledGrenades, grenadePoolManager, playerTarget, p3GrenadePosition, p3GrenadeBulletSpread, null, p3GrenadeShootForce);
    }

    void Phase3BarrageL()
    {
        LbarrageManager = ObjectPool.instance.GetPooledObjectManaged(ObjectPool.instance.pooledBarrageL, LbarrageManager, playerTarget, p3BarragePositionL, p3BarrageBulletSpread, ObjectPool.instance.pooledbarragemuzzleFlashL, p3BarrageShootForce);
    }

    void Phase3BarrageR()
    {
        RbarrageManager = ObjectPool.instance.GetPooledObjectManaged(ObjectPool.instance.pooledBarrageR, RbarrageManager, playerTarget, p3BarragePositionR, p3BarrageBulletSpread, ObjectPool.instance.pooledbarragemuzzleFlashR, p3BarrageShootForce);
    }

    void ObsidianCharge() //The boss's charge attack
    {
        obsidianIsCharging = true;
        chargeHitboxPhase3.SetActive(true);
        walkPoint = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        bossNavAgent.SetDestination(player.transform.position);
        //bossNavAgent.ResetPath();
        //bossNavAgent.isStopped = true;
        //bossNavAgent.velocity = (transform.position - player.transform.position).normalized * 10;

        //obsidianIsCharging = true;
        //chargeHitboxPhase3.SetActive(true);
        bossNavAgent.speed = 150f;
        bossNavAgent.acceleration = 10000f;
        bossNavAgent.angularSpeed = 0f;
        //walkPoint = new Vector3(chargeTarget.transform.position.x, transform.position.y, chargeTarget.transform.position.z);
        
    }

    void ObsidianChargeMuzzle() //Particle effects
    {
        GameObject LeftFlash = ObjectPool.instance.chargeMuzzleflashes[0];
        GameObject RightFlash = ObjectPool.instance.chargeMuzzleflashes[1];

        LeftFlash.transform.position = p3MeteorPositionL.position;
        RightFlash.transform.position = p3MeteorPositionR.position;
        LeftFlash.transform.rotation = p3MeteorPositionL.rotation;
        RightFlash.transform.rotation = p3MeteorPositionR.rotation;

        LeftFlash.SetActive(true);
        RightFlash.SetActive(true);

        //Instantiate(p3Attack4Particle, p3MeteorPositionL.position, Quaternion.identity);
        //Instantiate(p3Attack4Particle, p3MeteorPositionR.position, Quaternion.identity);
    }

    void ObsidianChargeEnd() //The end of the boss's charge attack
    {
        obsidianIsCharging = false;
        chargeHitboxPhase3.SetActive(false);
        bossWaypointIndex = Random.Range(bossWaypointMin, bossWaypointMax);
        walkPoint = bossWaypoints[bossWaypointIndex].transform.position;
        //bossNavAgent.isStopped = false;

        //obsidianIsCharging = false;
        //chargeHitboxPhase3.SetActive(false);
        bossNavAgent.speed = bossMoveSpeedP3;
        bossNavAgent.acceleration = 100f;
        bossNavAgent.angularSpeed = 200f;
    }

    void ObsidianLeapOn() //The boss's leap attack
    {
        obsidianIsLeaping = true;
        jumpHitboxPhase3.SetActive(true);
        bossNavAgent.speed = 50f;
        bossNavAgent.acceleration = 1000f;
        walkPoint = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        bossNavAgent.SetDestination(player.transform.position);
        bossNavAgent.autoBraking = true;
    }

    void ObsidianLeapMuzzle() //Particle effects
    {
        GameObject LeftFlash = ObjectPool.instance.jumpMuzzleflashes[0];
        GameObject RightFlash = ObjectPool.instance.jumpMuzzleflashes[1];
        
        LeftFlash.transform.position = p3MeteorPositionL.position;
        RightFlash.transform.position = p3MeteorPositionR.position;
        LeftFlash.transform.rotation = p3MeteorPositionL.rotation;
        RightFlash.transform.rotation = p3MeteorPositionR.rotation;

        LeftFlash.SetActive(true);
        RightFlash.SetActive(true);
        //Instantiate(p3Attack6Particle, p3MeteorPositionL.position, Quaternion.identity);
        //Instantiate(p3Attack6Particle, p3MeteorPositionR.position, Quaternion.identity);
    }

    void ObsidianLeapOff() //The end of the boss's leap attack
    {
        bossCapsuleCollider.isTrigger = false;
        obsidianIsLeaping = false;
        jumpHitboxPhase3.SetActive(false);
        bossNavAgent.speed = 0f;
        bossNavAgent.acceleration = 100f;
        bossNavAgent.autoBraking = false;
        bossWaypointIndex = Random.Range(bossWaypointMin, bossWaypointMax);
        walkPoint = bossWaypoints[bossWaypointIndex].transform.position;
    }

    void ShieldParticleOn() //Particle effects
    {
        //bossShieldObsidian.SetActive(true);
    }

    void ShieldParticleOff() //Particle effects
    {
        //bossShieldObsidian.SetActive(false);
    }
}

