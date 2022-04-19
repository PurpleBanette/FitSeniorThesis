using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisiscript : MonoBehaviour
{
    private void OnBecameInvisible()
    {
        Debug.Log("cantSee" + gameObject);
        this.gameObject.SetActive(false);
        //enabled = false;
        //Destroy(this.gameObject);
    }

    private void OnBecameVisible()
    {
        Debug.Log("See" + gameObject);
        this.gameObject.SetActive(true);
    }

}
