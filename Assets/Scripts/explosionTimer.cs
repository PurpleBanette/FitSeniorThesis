using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionTimer : MonoBehaviour
{

    public float maxLifetime;
    public AudioSource soundExplosion;

    void Start()
    {
        soundExplosion.Play();
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
