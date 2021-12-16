using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeBox : MonoBehaviour
{

    [SerializeField] float disabletimerReset = 0.75f;
    float timer;
    GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
    }
    private void OnEnable()
    {
        transform.position = player.transform.position;
        timer = disabletimerReset;
        
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        //Debug.Log(timer);
        if(timer <= 0f)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "EnemyProjectile")
        {
            Debug.Log("successfull Dodge");
            //StartCoroutine(GameMaster.gm.activateSlowMo());
            GameMaster.gm.activateSlowMo();
            gameObject.SetActive(false);
        }
    }

    void disableDodgeBox()
    {
        gameObject.SetActive(false);
    }
}
