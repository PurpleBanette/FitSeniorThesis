using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using UnityEngine.UI;

public class bossAi : MonoBehaviour
{
    Animator bossAni;
    public int health; //Remember to figure out how to damage the player
    int randAttack;
    int randPath;
    public int currentphase = 1;
    public bool dead;
    public Transform player;
    public Slider bossHealthbar;
    public GameObject obsidianLaser;

    CapsuleCollider bossCC;
    Rigidbody bossRB;
    [Tooltip("The AI's nav mesh agent")] public NavMeshAgent bossNavAgent; //References the nav mesh agent attached to the AI
    LayerMask areaMask; //References the area mask in the nav mesh
    [Tooltip("detect which layer the boss can detect")] public LayerMask bossTerrainDetector, bossPlayerDetector;
    [Tooltip("Detects if the sidekick is following the player")] public bool followingPlayer = false;

    //Patroling
    [Tooltip("The walkpoint that the AI goes to")] public Vector3 walkPoint;
    [Tooltip("Checks if there is a walkpoint")] bool walkPointSet;
    [Tooltip("How far the AI can detect a flat surface to walk to")] public float walkPointRange;
    //Attacking
    [Tooltip("The time between each attack for each phase")] public float timeBetweenAttacks;
    [Tooltip("Checks if the AI already attacked")] public bool alreadyAttacked;
    //States
    [Tooltip("The range of how far the AI can be before detecting and attacking")] public float sightRange, attackRange;
    [Tooltip("Checks if the player is within range")] public bool playerInSightRange, playerInAttackRangeP1, playerInAttackRangeP2, playerInAttackRangeP3;

    //Hitbox management stuff
    [Tooltip("Trigger GameObject for Phase 1 Attacks")] public GameObject triggerP1;
    public List<GameObject> attackHitboxesPhase1;
    [Tooltip("Trigger GameObject for Phase 2 Attacks")] public GameObject triggerP2;
    public List<GameObject> attackHitboxesPhase2;
    [Tooltip("Trigger GameObject for Phase 3 Attacks")] public GameObject triggerP3;
    public List<GameObject> attackHitboxesPhase3;
    public GameObject chargeHitboxPhase3;

    public List<GameObject> hurtboxes;

    float phase3WalkTimer = 0f;

    //phase 2
    bool playerTracking = false;


    void Awake()
    {
        bossAni = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bossNavAgent = GetComponent<NavMeshAgent>();

    }

    private void Start()
    {
        StartCoroutine(phase1Pattern());
        currentphase = 1;
        bossNavAgent.speed = 5f;
    }

    // Update is called once per frame
    void Update()
    {

        //checks for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, bossPlayerDetector);
        //playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, bossPlayerDetector);
        bossHealthbar.value = health;

        //Phase 1
        if (currentphase == 1)
        {
            playerInAttackRangeP1 = Physics.CheckSphere(triggerP1.transform.position, attackRange, bossPlayerDetector);
            if (!playerInSightRange && !playerInAttackRangeP1) BossPatroling();
            if (playerInSightRange && !playerInAttackRangeP1) BossChasePlayer();
            if (playerInAttackRangeP1 && playerInSightRange) BossAttackPlayer();
        }


        //Phase 2
        //playerInAttackRangeP2 = Physics.CheckSphere(triggerP2.transform.position, attackRange, bossPlayerDetector);
        if (currentphase == 2)
        {
            Vector3 bossFwd = transform.TransformDirection(Vector3.forward);
            playerInAttackRangeP2 = Physics.Raycast(transform.position, bossFwd, Mathf.Infinity, bossPlayerDetector);
            if (!playerInSightRange && !playerInAttackRangeP2) BossPatroling();
            if (playerInSightRange && !playerInAttackRangeP2) BossChasePlayer();
            if (playerInAttackRangeP2 && playerInSightRange) BossAttackPlayer();
        }

        //Phase 3
        if (currentphase == 3)
        {
            Vector3 bossFwd = transform.TransformDirection(Vector3.forward);
            playerInAttackRangeP3 = Physics.CheckSphere(triggerP3.transform.position, attackRange, bossPlayerDetector);
            if (!playerInSightRange && !playerInAttackRangeP3) BossPatroling();
            if (playerInSightRange && !playerInAttackRangeP3) BossChasePlayer();
            if (playerInAttackRangeP3 && playerInSightRange) BossAttackPlayer();
            phase3WalkTimer += Time.deltaTime;
            if (phase3WalkTimer >= 10f)
            {
                randPath = Random.Range(0, 2);
                phase3WalkTimer = 0f;
            }
        }
        //prevents the boss from doing anything after he dies
        if (!dead)
        {
            if (currentphase == 1 && health <= 600)
            {
                StopAllCoroutines();
                bossNavAgent.speed = 0f;
                bossAni.SetTrigger("phase2");
                currentphase = 2;
                StartCoroutine(phase2Pattern());
                triggerP1.SetActive(false);

            }
            if (currentphase == 2 && health <= 300)
            {
                StopAllCoroutines();
                bossNavAgent.speed = 0f;
                bossAni.SetTrigger("phase3");
                currentphase = 3;
                StartCoroutine(phase3Pattern());
                triggerP2.SetActive(false);
            }
            if (currentphase == 3 && health <= 0)
            {
                triggerP3.SetActive(false);
                StopAllCoroutines();
                currentphase = 4;
                bossAni.SetTrigger("dead");
                dead = true;
            }
        }
        Vector3 targetPostition = new Vector3(player.position.x, this.transform.position.y, player.position.z);
        if (playerTracking)
        {
            transform.LookAt(targetPostition);
        }



    }

    IEnumerator phase1Pattern()
    {
        triggerP1.SetActive(true);
        bossAni.SetFloat("isWalkingAxe", Mathf.Abs(bossNavAgent.speed));
        timeBetweenAttacks = 4f;
        while (true && !dead)
        {
            yield return new WaitForSeconds(0);
            attackRange = 2f;
            sightRange = 100f;
            if (playerInAttackRangeP1)
            {
                randAttack = Random.Range(1, 4);
                if (randAttack == 1)
                {
                    bossAni.SetTrigger("axeAttack1");
                    bossNavAgent.speed = 0f;
                    yield return new WaitForSeconds(4);
                    bossAni.ResetTrigger("axeAttack1");
                }
                if (randAttack == 2)
                {
                    bossAni.SetTrigger("axeAttack2");
                    bossNavAgent.speed = 0f;
                    yield return new WaitForSeconds(4);
                    bossAni.ResetTrigger("axeAttack2");
                }
                if (randAttack == 3)
                {
                    bossAni.SetTrigger("axeAttack3");
                    bossNavAgent.speed = 0f;
                    yield return new WaitForSeconds(4);
                    bossAni.ResetTrigger("axeAttack3");
                }
            }
        }
    }

    IEnumerator phase2Pattern()
    {
        triggerP2.SetActive(true);
        bossAni.SetFloat("isWalkingRailgun", Mathf.Abs(bossNavAgent.speed));
        timeBetweenAttacks = 5f;
        Debug.DrawRay(transform.position, Vector3.forward, Color.red);
        while (true && !dead)
        {
            yield return new WaitForSeconds(0);
            attackRange = 2f;
            sightRange = 100f;

            if (playerInAttackRangeP2)
            {
                randAttack = Random.Range(1, 4);
                if (randAttack == 1)
                {
                    bossAni.SetTrigger("railgunAttack1");
                    bossNavAgent.speed = 0f;
                    yield return new WaitForSeconds(5);
                    bossAni.ResetTrigger("railgunAttack1");
                }
                if (randAttack == 2)
                {
                    bossAni.SetTrigger("railgunAttack2");
                    bossNavAgent.speed = 0f;
                    yield return new WaitForSeconds(5);
                    bossAni.ResetTrigger("railgunAttack2");
                }
                if (randAttack == 3)
                {
                    bossAni.SetTrigger("railgunAttack3");
                    bossNavAgent.speed = 0f;
                    yield return new WaitForSeconds(5);
                    bossAni.ResetTrigger("railgunAttack3");
                }
            }

        }
    }

    IEnumerator phase3Pattern()
    {
        triggerP3.SetActive(true);
        bossAni.SetFloat("isWalkingGauntlet", Mathf.Abs(bossNavAgent.speed));
        timeBetweenAttacks = 4f;
        Debug.DrawRay(transform.position, Vector3.forward, Color.red);
        while (true && !dead)
        {
            yield return new WaitForSeconds(5);
            attackRange = 100f;
            sightRange = 1000f;

            if (playerInAttackRangeP3)
            {
                randAttack = Random.Range(1, 7);
                if (randAttack == 1)
                {
                    bossAni.SetTrigger("gauntletAttack1");
                    bossNavAgent.speed = 0f;
                    yield return new WaitForSeconds(5);
                    bossAni.ResetTrigger("gauntletAttack1");
                }
                if (randAttack == 2)
                {
                    bossAni.SetTrigger("gauntletAttack2");
                    bossNavAgent.speed = 0f;
                    yield return new WaitForSeconds(5);
                    bossAni.ResetTrigger("gauntletAttack2");
                }
                if (randAttack == 3)
                {
                    bossAni.SetTrigger("gauntletAttack3");
                    bossNavAgent.speed = 0f;
                    yield return new WaitForSeconds(5);
                    bossAni.ResetTrigger("gauntletAttack3");
                }
                if (randAttack == 4)
                {
                    bossAni.SetTrigger("gauntletAttack4");
                    bossNavAgent.speed = 0f;
                    yield return new WaitForSeconds(5);
                    bossAni.ResetTrigger("gauntletAttack4");
                }
                if (randAttack == 5)
                {
                    bossAni.SetTrigger("gauntletAttack5");
                    bossNavAgent.speed = 0f;
                    yield return new WaitForSeconds(5);
                    bossAni.ResetTrigger("gauntletAttack5");
                }
                if (randAttack == 6)
                {
                    bossAni.SetTrigger("gauntletAttack6");
                    //bossNavAgent.speed = 0f;
                    yield return new WaitForSeconds(5);
                    bossAni.ResetTrigger("gauntletAttack6");
                }
                /*if (bossNavAgent.speed >= 1)
                {
                    bossNavAgent.speed = 5f;
                    yield return new WaitForSeconds(1);
                    bossNavAgent.speed = 0f;
                }*/
            }
        }
    }

    void FixedUpdate()
    {
        BossAnimations();
    }

    void BossAnimations()
    {
        if (currentphase == 1)
        {
            bossAni.SetFloat("isWalkingAxe", Mathf.Abs(bossNavAgent.speed));
        }

        if (currentphase == 2)
        {
            bossAni.SetFloat("isWalkingAxe", 0f);
            bossAni.SetFloat("isWalkingRailgun", Mathf.Abs(bossNavAgent.speed));
        }
        if (currentphase == 3)
        {
            bossAni.SetFloat("isWalkingAxe", 0f);
            bossAni.SetFloat("isWalkingRailgun", 0f);
            bossAni.SetFloat("isWalkingGauntlet", Mathf.Abs(bossNavAgent.speed));
        }
    }

    void BossPatroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            bossNavAgent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    void SearchWalkPoint()
    {
        //calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        //check if walkpoint is actually on the ground and not off the map
        if (Physics.Raycast(walkPoint, -transform.up, 2f, bossTerrainDetector))
        {
            walkPointSet = true;
        }
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
        bossNavAgent.SetDestination(transform.position);



        //Toggles the alreadyAttacked function with a delay in seconds, using timeBetweenAttacks
        if (!alreadyAttacked)
        {
            //Attacks
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;

        if (currentphase == 1)
        {
            bossNavAgent.speed = 5f;
        }
        if (currentphase == 2)
        {
            bossNavAgent.speed = 7f;
        }
        if (currentphase == 3)
        {
            bossNavAgent.speed = 10f;
        }
    }

    void HitboxActivatedPhase1()
    {
        foreach (var hitboxp1 in attackHitboxesPhase1)
        {
            hitboxp1.SetActive(true);
        }
    }
    void HitboxDeactivatedPhase1()
    {
        foreach (var hitboxp1 in attackHitboxesPhase1)
        {
            hitboxp1.SetActive(false);
        }
    }

    void HitboxActivatedPhase2()
    {
        foreach (var hitboxp2 in attackHitboxesPhase2)
        {
            hitboxp2.SetActive(true);
        }
    }
    void HitboxDeactivatedPhase2()
    {
        foreach (var hitboxp2 in attackHitboxesPhase2)
        {
            hitboxp2.SetActive(false);
        }
    }

    void HitboxActivatedPhase3()
    {
        foreach (var hitboxp3 in attackHitboxesPhase3)
        {
            hitboxp3.SetActive(true);
        }
    }
    void HitboxDeactivatedPhase3()
    {
        foreach (var hitboxp3 in attackHitboxesPhase3)
        {
            hitboxp3.SetActive(false);
        }
    }

    void NotMoving()
    {
        bossNavAgent.speed = 0f;
    }
    void Moving()
    {
        if (currentphase == 1)
        {
            bossNavAgent.speed = 5f;
        }
        if (currentphase == 2)
        {
            bossNavAgent.speed = 7f;
        }
        if (currentphase == 3)
        {
            bossNavAgent.speed = 10f;
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

    void obsidianLaserOn()
    {
        obsidianLaser.SetActive(true);
    }
    void obsidianLaserOff()
    {
        obsidianLaser.SetActive(false);
    }

    void ObsidianCharge()
    {
        chargeHitboxPhase3.SetActive(true);
        bossNavAgent.speed = 100f;
    }
    void ObsidianChargeEnd()
    {
        chargeHitboxPhase3.SetActive(false);
    }
}

