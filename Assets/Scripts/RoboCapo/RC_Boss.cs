using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RC_Boss : MonoBehaviour
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

    //A set of empty game objects for the boss to hop to when doing ranged attacks
    [SerializeField] List<Transform> BossDestinations;
    [SerializeField] int currentphase = 1;
    int destination;
    float Phase2timer = 0f;
    Vector3 distanceToWalkpoint;

    

    void Awake()
    {
        NavAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        phase2Hops();
    }

    void phase2Hops()
    {
        //time her? I barely knew her.

        
        
        Debug.Log(Phase2timer);
        if (Phase2timer > 0f)
        {
            Phase2timer -= Time.deltaTime;
        }
        else
        {
            NavAgent.isStopped = false;
            destination = Random.Range(0, BossDestinations.Count - 1);
            NavAgent.SetDestination(BossDestinations[destination].position);
            distanceToWalkpoint = transform.position - BossDestinations[destination].position;
            NavAgent.speed *= 50;

            if (distanceToWalkpoint.magnitude < 2f)
            {
                Debug.Log("Made It");
                NavAgent.speed /= 50;
                NavAgent.isStopped = true;
                Phase2timer = Random.Range(5f, 10f);
                //ani.SetTrigger("shoot");
            }


        }
    }
}
