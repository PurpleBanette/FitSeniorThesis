using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obsidianMeteorBullets : MonoBehaviour
{
    ModifiedTPC charCtrl;
    GameObject player;
    Rigidbody rb;
    List<GameObject> terrain;
    bossAiObsidian bossScript;
    obsidianMeteorFragments meteorProjectiles;

    public GameObject explosion;
    public int explosionDamage;
    public float explosionRange;
    [SerializeField] float maxLifetime;
    float resetLifetime;
    public LayerMask playerDetector;
    public float explosionForce;
    public GameObject fragmentsPrefab;
    //public GameObject[] fragments;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        charCtrl = player.GetComponent<ModifiedTPC>();
        meteorProjectiles = GetComponent<obsidianMeteorFragments>();
        resetLifetime = maxLifetime;
    }

    private void Start()
    {

    }

    private void Update()
    {
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0) ExplodeIntoFragments();
        //Invoke("ExplodeIntoFragments", maxLifetime);
    }

    private void ExplodeIntoFragments()
    {
        GameObject currentFragment;

        //Check if an explosion is assigned
        if (explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);

        Collider[] playerCollider = Physics.OverlapSphere(transform.position, explosionRange, playerDetector);
        for (int i = 0; i < playerCollider.Length; i++)
        {
            charCtrl.playerTakeDamage();
            if (charCtrl.GetComponent<Rigidbody>())
            {
                charCtrl.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);
            }
        }
        //Instantiate(bossScript.p3MeteorFragments, bossScript.p3MeteorBullet.transform.position, Quaternion.identity);
        //fragments = new GameObject[10];

        for (int i = 0; i < ObjectPool.instance.meteorsToPool/2; i++)

        {
            if (!ObjectPool.instance.pooledMiniMeteors[i].activeInHierarchy)
            {
                currentFragment = ObjectPool.instance.pooledMiniMeteors[i];
                
            }
            else
            {
                currentFragment = ObjectPool.instance.pooledMiniMeteors[i + (ObjectPool.instance.meteorsToPool / 2)];
            }
            currentFragment.transform.position = transform.position;
            currentFragment.SetActive(true);
        }


        Invoke("Delay", 0.05f);
    }

    private void Delay()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        maxLifetime = resetLifetime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }

}
