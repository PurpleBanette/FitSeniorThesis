using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_damageZapper : MonoBehaviour
{
    [SerializeField] bool Zapper;

    private void Update()
    {
        if (Zapper)
        {
            ModifiedTPC.instance.imHit = true;
            ModifiedTPC.instance.playerTakeDamage();
            Zapper = false;
            
        }
    }
}
