using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCam : MonoBehaviour
{
    /*
    public Transform target;
    public GameObject player;
    float dampTime = 0.5f;
    private Vector3 velocity = Vector3.zero;
    GameObject objgm;
    GameMaster gm;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        objgm = GameObject.FindGameObjectWithTag("GameController");
        gm = objgm.GetComponent<GameMaster>();
        target = player.transform;
    }
    void Update()
    {
        var camera = Camera.main;
        if (target)
        {
            if (!gm.twoDmode)
            {

            }
            Vector3 point = camera.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 15f));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
    }
    */
}
