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
        if (TimerManager.mytimer <= 166 && !TimerManager.switchedScene)
        {
            SceneManager.LoadScene("YB_Intro_Scene001");
            TimerManager.switchedScene = true;

        }
        //will load next sceene ass delay 
        if (TimerManager.mytimer <= 128 )
          {
            
            SceneManager.LoadScene("YB_Intro_Scene002 1");
        }
        if (TimerManager.mytimer <= 80)
        {

            SceneManager.LoadScene("Firstboss");
        }



        Debug.Log(TimerManager.mytimer);




    }
   
 }
