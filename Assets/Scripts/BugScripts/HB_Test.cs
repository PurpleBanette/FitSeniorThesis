using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HB_Test : MonoBehaviour
{
    [SerializeField] bool DotheThing;
    
    void Update()
    {
        if (DotheThing)
        {
            ModifiedTPC.instance.playerTakeDamage();
            DotheThing = false;   
        }
    }
}
