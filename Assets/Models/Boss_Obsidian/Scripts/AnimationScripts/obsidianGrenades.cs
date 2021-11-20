using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obsidianGrenades : MonoBehaviour
{
    ModifiedTPC charCtrl;
    GameObject player;
    Rigidbody rb;
    TrailRenderer tr;

    public GameObject explosion;
    public int explosionDamage;
    public float explosionRange;
    public float maxLifetime;
    public bool useGravity;
    //public bool explodeOnTouch = true;
    int collisions;
    public LayerMask playerDetector;
    public float explosionForce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        charCtrl = player.GetComponent<ModifiedTPC>();
        GameObject[] terrain = GameObject.FindGameObjectsWithTag("Terrain");
        tr = GetComponent<TrailRenderer>();
    }

    private void Start()
    {
        Setup();
    }

    private void OnEnable()
    {
        tr.emitting = true;
    }

    private void OnDisable()
    {
        tr.emitting = false;
    }

    private void Update()
    {
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0)
        {
            Explode();
        }
    }

    private void Setup()
    {
        rb.useGravity = useGravity;
    }

    private void Explode()
    {
        //Check if an explosion is assigned
        if (explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);

        //Damage player if nearby
        Collider[] playerCollider = Physics.OverlapSphere(transform.position, explosionRange, playerDetector);
        for (int i = 0; i < playerCollider.Length; i++)
        {
            charCtrl.playerTakeDamage();
            if (charCtrl.GetComponent<Rigidbody>())
            {
                charCtrl.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);
            }
        }

        Invoke("Delay", 0.05f);
    }

    private void Delay()
    {
        gameObject.SetActive(false);
        rb.velocity = Vector3.zero;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }

    void OnTriggerEnter(Collider other)
    {
        Explode();
    }
}
