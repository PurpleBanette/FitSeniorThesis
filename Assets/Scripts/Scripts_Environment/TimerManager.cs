using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerManager
{
    [Header("Timer")]
    
    public static float mytimer = 308f;
    public static bool switchedScene = false;
    public static bool bossbattle = false;

    //when boss health reach XXX return to scene  into time
    void Update()
    {
        if (bossbattle == true && bossAiRobocapoRemake.instance.bossHealth == 0)
        {
            bossbattle = false;
          
        }
        if (bossbattle == true && bossAiObsidian.instance.bossHealth == 0)
        {
            bossbattle = false;

        }
    }
    

}
