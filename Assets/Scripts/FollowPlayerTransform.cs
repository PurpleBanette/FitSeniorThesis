using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerTransform : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = player.transform.position;
    }
}
