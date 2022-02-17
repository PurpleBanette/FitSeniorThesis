using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obsidianShockwave : MonoBehaviour
{
    ModifiedTPC charCtrl;
    GameObject player;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        charCtrl = player.GetComponent<ModifiedTPC>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            //take damage function
            charCtrl.playerTakeDamage();
        }
    }
}