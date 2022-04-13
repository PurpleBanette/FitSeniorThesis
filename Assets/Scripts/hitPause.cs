using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitPause : MonoBehaviour
{
    float duration = 0.1f;
    float freezeTimer = 0f;
    bool Frozen;
    public static hitPause instance;

    private void Awake()
    {
        instance = this;
        freezeTimer = duration;
    }

    
    // Update is called once per frame
    void Update()
    {
        if (Frozen)
        {
            //Debug.Log("Lets Get This Bread");
            freezeTimer -= Time.unscaledDeltaTime;
            //Debug.Log(sloMoTimer);
            if (freezeTimer <= 0)
            {
                //Debug.Log("Bread Gotten");
                Time.timeScale = 1;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                freezeTimer = duration;
                Frozen = false;
            }

        }
    }

    public void INevarFreeze()
    {
        Frozen = true;
        Time.timeScale = 0;
    }    
}
