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
        if (TimerManager.mytimer <= 40 && !TimerManager.switchedScene)
        {
            SceneManager.LoadScene("IntroVideo");
            TimerManager.switchedScene = true;

        }
       
        if (TimerManager.mytimer <= 20 )
          {
            
            SceneManager.LoadScene("BossRobocapo");
        }
        

        Debug.Log(TimerManager.mytimer);




    }
   
 }
