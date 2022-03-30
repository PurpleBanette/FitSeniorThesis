using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obsidianBullets : MonoBehaviour
{
    ModifiedTPC charCtrl;
    GameObject player;
    Rigidbody rb;
    List<GameObject> terrain;
    public GameObject impactParticle;
    TrailRenderer tr;

    float destroyTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        charCtrl = player.GetComponent<ModifiedTPC>();
        tr = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        destroyTimer += Time.deltaTime;
        if (destroyTimer >= 5f)
        {
            gameObject.SetActive(false);
            //rb.velocity = Vector3.zero;
        }
    }

    private void OnEnable()
    {
        if (tr != null)
        {
            tr.emitting = true;
        }
    }

    private void OnDisable()
    {
        destroyTimer = 0f;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (tr != null)
        {
            tr.emitting = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (impactParticle != null) Instantiate(impactParticle, transform.position, Quaternion.identity);

        if (other.transform.tag != "roofRevolverTrigger")
        {
            gameObject.SetActive(false);
        }
        
        //rb.velocity = Vector3.zero;
    }
}