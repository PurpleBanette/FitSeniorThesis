using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionTimer : MonoBehaviour
{

    public float maxLifetime;

    void Start()
    {
        
    }


    void Update()
    {
        maxLifetime -= Time.deltaTime;

        if (maxLifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
