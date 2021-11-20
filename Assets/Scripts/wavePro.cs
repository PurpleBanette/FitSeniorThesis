using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wavePro : MonoBehaviour
{
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        transform.parent = null;
        //transform localscale fixes a bug that makes the bullets too tiny when they instantiate
        //transform.localScale = new Vector3(1, 1, 1);
        rb.AddForce(transform.forward * 1000f);
    }

}
