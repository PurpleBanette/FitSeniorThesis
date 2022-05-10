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
        SceneManager.GetActiveScene();
        switcher = this;
    }

    // Update is called once per frame

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
    
}
