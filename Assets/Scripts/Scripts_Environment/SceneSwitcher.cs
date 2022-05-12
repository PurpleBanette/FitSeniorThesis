using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

 public class SceneSwitcher : MonoBehaviour 
 {
    public static SceneSwitcher switcher;

    private void Awake()
    {
        {
            SceneManager.GetActiveScene();
            switcher = this;
        }

        if (TimerManager.switchedScene == true && !TimerManager.switchedScene)
         {

            TimerManager.switchedScene = false;
         }
        
    }
    void FixedUpdate()
    {
        /*  if robo is dead switch to 2nd introscene 
            //if (bossAiRobocapoRemake.instance.bossHealth == 0)
        {
           IntroVideoOver();
        }
        */
        // Debug.Log(TimerManager.mytimer);
    }
    public void IntroVideoOver()
    {
        SceneManager.LoadScene("IntroVideo2", LoadSceneMode.Single);
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void ReloadOnDeath()
    {
        Invoke("ReloadScene", 5f);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
