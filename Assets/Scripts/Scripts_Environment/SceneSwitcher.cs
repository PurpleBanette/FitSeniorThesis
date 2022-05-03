using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

 public class SceneSwitcher : MonoBehaviour 
 {
   


    // Update is called once per frame
    void Update()
    {
       //Connected to TimeMangerScript
        TimerManager.mytimer -= Time.deltaTime;

        
        //Can Nest if's  as long as its not greater then set timer in script
        if (TimerManager.mytimer <= 76 && !TimerManager.switchedScene )
        {
            SceneManager.LoadScene("BossRobocapo");
            TimerManager.switchedScene = true;
            
            
        }
        
        if (bossAiRobocapoRemake.instance.bossHealth == 0)
        {
            //TimerManager.switchedScene = false;
            SceneManager.LoadScene("IntroVIdeoEnding");
           

        }
       // if (bossAiObsidian.instance.bossHealth == 0)
       // {
            //TimerManager.switchedScene = false;
            //SceneManager.LoadScene("");


       // }




        Debug.Log(TimerManager.mytimer);




    }

    /* else if (TimerManager.switchedScene == true)
        {
            TimerManager.switchedScene = false;
        }
    */
    // if (TimerManager.mytimer <= 20 )
    //{

    // SceneManager.LoadScene("IntroVideoEnding");
    // }
    // bossAiRobocapoRemake bossAiRobocapoRemake;
    //[SerializeField] private bossAiRobocapoRemake bossAiRobocapoRemake;
}
