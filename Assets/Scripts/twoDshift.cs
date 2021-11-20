using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class twoDshift : MonoBehaviour
{
    
    public GameMaster gm;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            gm.twoDmode = false;
            Destroy(this.gameObject);
        }
    }
}
