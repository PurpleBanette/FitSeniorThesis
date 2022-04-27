using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager
{
    [Header("Timer")]
    
    public static float mytimer = 80f;
    public static bool switchedScene = false;

    //when boss health reach XXX return to scene  into time
    void Update()
    {
        if(bossAiRobocapoRemake.instance.bossHealth <= 0)
        {

        }
    }
    

}
