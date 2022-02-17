using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempParent : MonoBehaviour
{

    /*void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "xxx")
        {
            transform.parent = other.transform;
        }
    }
    void OnTrggerExit(Collider other)
    {
        if (other.gameObject.tag == "xxx")
        {
            transform.parent = null;
        }
    }*/

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "xxx")
        {
            transform.parent = other.transform;
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "xxx")
        {
            transform.parent = null;
        }
    }
}
