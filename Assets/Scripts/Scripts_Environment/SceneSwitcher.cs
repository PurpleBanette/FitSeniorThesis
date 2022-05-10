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
        if (TimerManager.mytimer <= 70 && !TimerManager.switchedScene)
        {
            SceneManager.LoadScene("BossRobocapo");
            TimerManager.switchedScene = true;

        }

        // if robo is dead switch to 2nd introscene 

        if (bossAiRobocapoRemake.instance.bossHealth <= 0)
        {

            SceneManager.LoadScene("IntroVideo2");
           
        }

        //when video over load obsne scene 

        if (bossAiRobocapoRemake.instance.bossHealth <= 0)
        {
            //SceneManager.LoadScene("IntroVideo2");
            SceneManager.LoadScene("BossObsidian");
        }
        

        if (TimerManager.switchedScene == true && !TimerManager.switchedScene)
         {

            TimerManager.switchedScene = false;
            }
       // else if (TimerManager.mytimer == 0)
       // {

            
        //}


        //Debug.Log(TimerManager.mytimer);




    }
   
 }
