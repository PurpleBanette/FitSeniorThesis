using UnityEngine.SceneManagement;
using UnityEngine;

<<<<<<< HEAD
=======
/*
>>>>>>> 8c00dd186f493d6fee1be1b5cedc8b0aba7f9081
public enum CheckMethod
{
    Distance,
    Trigger
}

public class CheckDistance : MonoBehaviour
{
<<<<<<< HEAD
=======
    
>>>>>>> 8c00dd186f493d6fee1be1b5cedc8b0aba7f9081
    public Transform player;
    public CheckMethod checkMethod;
    public float loadRange;

    // Start is called before the first frame update

    //Scene State

    private bool isLoaded;
    private bool loadNext;

    private void Start()
    {
        if (SceneManager.sceneCount > 2)
        {
            for (int i = 0; i < SceneManager.sceneCount; ++i)
            {
                Scene scene = SceneManager.GetSceneByName("");
                if (scene.name == gameObject.name)
                {
                    isLoaded = true;
                }

<<<<<<< HEAD
            }
        }
=======
                Debug.Log(scene);
            }
        }
        
>>>>>>> 8c00dd186f493d6fee1be1b5cedc8b0aba7f9081
    }
    // Update is called once per frame
    void Update()
    {
        TimerManager.mytimer -= Time.deltaTime;

        //Can Nest if's  as long as its not greater then set timer in script
        if (TimerManager.mytimer <= 166 && !TimerManager.switchedScene)
        {
            SceneManager.LoadScene("YB_Intro_Scene001");
            TimerManager.switchedScene = true;

           
        }
        Debug.Log(isLoaded);

        if (checkMethod == CheckMethod.Distance)
        {
            DistanceCheck();
        }
<<<<<<< HEAD
=======
        
>>>>>>> 8c00dd186f493d6fee1be1b5cedc8b0aba7f9081
        else if (checkMethod == CheckMethod.Trigger)
        {
           //TriggerCheck;
        }
    }

    void DistanceCheck()
    { 
        if (Vector3.Distance (player.position, transform.position) < loadRange)

        {
            LoadScene();
        }
<<<<<<< HEAD
=======

>>>>>>> 8c00dd186f493d6fee1be1b5cedc8b0aba7f9081
        else
        {
             UnLoadScene();
        }

    }
    void LoadScene()
    {
        if(!isLoaded)
        {
            SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive); // must be same name as prefabs
            isLoaded = true;
        }
    }
    void UnLoadScene()
    {
        if (!isLoaded)
        {
            SceneManager.UnloadSceneAsync(gameObject.name);
            isLoaded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            loadNext = false;
        }
    }

    void TriggerCheck()

    {
        if (loadNext)
        {
            LoadScene();
        }
        else
        {
            UnLoadScene();
        }
    }

}
<<<<<<< HEAD
=======
*/
>>>>>>> 8c00dd186f493d6fee1be1b5cedc8b0aba7f9081
