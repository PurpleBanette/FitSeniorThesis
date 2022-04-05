using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outOfBounds : MonoBehaviour
{
    public static outOfBounds instance;
    public GameObject playerOobRespawn;
    public GameObject bossOobRespawn;

    [SerializeField] GameObject playerCatch;
    [SerializeField] GameObject bossCatch;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            playerCatch.transform.position = playerOobRespawn.transform.position;
        }
        if (other.transform.tag == "Boss")
        {
            bossCatch.transform.position = bossOobRespawn.transform.position;
        }
    }
}
