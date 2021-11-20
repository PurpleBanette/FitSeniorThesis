using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFindPlayer : MonoBehaviour
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
        //Cinemachine.Cinemachine3rdPersonFollow(player.transform);
    }
}
