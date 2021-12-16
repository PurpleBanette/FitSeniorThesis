using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    GameObject player;
    Rigidbody rb;
    ModifiedTPC charCtrl;
    float destroyTimer = 0f;
    Vector3 targetPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        charCtrl = player.GetComponent<ModifiedTPC>();
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
        //transform localscale fixes a bug that makes the bullets too tiny when they instantiate
        transform.localScale = new Vector3(1f, 1f, 1f);
        //Use the below vector3 for LookAt if you dont want the bullets to go up or down
        //targetPosition = new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z);
        transform.LookAt(player.transform);
        rb.AddForce(transform.forward * 1000f);
    }

    // Update is called once per frame
    void Update()
    {
        destroyTimer += Time.deltaTime;
        if(destroyTimer >= 10f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            //take damage function
            //charCtrl.playerTakeDamage();
            //always make sure the Destroy statement is last, any code beneath the destroy wont execute
            Destroy(this.gameObject);
        }

        //always make sure the Destroy statement is last, any code beneath the destroy wont execute
        Destroy(this.gameObject);
    }
}
