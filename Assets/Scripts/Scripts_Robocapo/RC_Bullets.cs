using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RC_Bullets : MonoBehaviour
{
    
    Rigidbody rb;
    public GameObject impactParticle;
    TrailRenderer tr;

    float destroyTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
        if(other.tag == "Player" || other.tag == "ground")
        {
            if (impactParticle != null) Instantiate(impactParticle, transform.position, Quaternion.identity);

            gameObject.SetActive(false);
            //rb.velocity = Vector3.zero;
        }

    }
}
