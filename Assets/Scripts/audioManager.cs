using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    private static audioManager instance = null;

    public AudioSource boss2Music;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
            return;
        }
        else if (boss2Music == this) return;
        Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        boss2Music.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
