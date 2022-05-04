using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelSelection : MonoBehaviour
{
    private bool isLoaded;
    
    // Start is called before the first frame update

    private void Start()
    {
        if (SceneManager.sceneCount >= 2)
        {
            for (int i = 0; i < SceneManager.sceneCount; ++i)
            {
                Scene scene = SceneManager.GetSceneByName("");
                if (scene.name == gameObject.name)
                {
                    isLoaded = true;
                }

                Debug.Log(scene);
            }
        }
    }
        void LoadScene()
    {
        if (!isLoaded)
        {
            SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive); // must be same name as prefabs
            isLoaded = true;
        }
    }
}
