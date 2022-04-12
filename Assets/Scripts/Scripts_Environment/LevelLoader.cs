using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    // Update is called once per frame
    void Update()
    {
        if (transitionTime == 0)
        {
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine("LoadLevel(SceneManager.GetActiveScene().buildIndex + 1)");
        
    }

    IEnumerator LoadLevel(int levelIndex)

    {
        //Play Animation
        transition.SetTrigger("Fade_to_Black_Start");
        //Wait
        yield return new WaitForSeconds(transitionTime);
        //NextScene OR Shot

        SceneManager.LoadScene(levelIndex);


    }
}
