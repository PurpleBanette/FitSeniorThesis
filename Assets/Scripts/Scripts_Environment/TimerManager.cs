using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TimerManager
{
    [Header("Timer")]
    
    public static float mytimer = 380f;
    public static bool switchedScene = false;
   // bossAiRobocapoRemake bossAiRobocapoRemake ;

    //when boss health reach XXX return to scene  into time
    void Update()


    {
        if (bossAiRobocapoRemake.instance.bossHealth == 0)
        {
          
          //SceneManager.LoadScene("BossObsidian");


        }
    }


}
