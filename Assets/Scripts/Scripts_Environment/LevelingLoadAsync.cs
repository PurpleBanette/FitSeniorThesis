using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelingLoadAsync : MonoBehaviour
{
    /*
    [Header("Statchecker")]
    public bool loadLevel; //check what level is loaded
    public string levelName;
    // Update is called once per frame
    void Update()

    {       
        //Connected to TimeMangerScript
        //TimerManager.mytimer -= Time.deltaTime;


        //Can Nest if's  as long as its not greater then set timer in script
        if (loadLevel == true)
        {
            loadLevel = false;
           // StartCoroutine (routine:LoadLevelAsync());
            var progressAsyncOperation = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
            progressAsyncOperation.completed += (opAsyncOperation) => Debug.Log(message: "Level Loading Done");
           // SceneManager.LoadSceneAsync("BossRobocapo");
           // TimerManager.switchedScene = true;
           
            
        }
        
    }
    private IEnumerable LoadLevelAsync()
    {
        var progressAsyncOperation = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        while(!progressAsyncOperation.isDone)
        {
            yield return null;
        }
        Debug.Log(message: "Level Loaded");
    }
      */  
}
