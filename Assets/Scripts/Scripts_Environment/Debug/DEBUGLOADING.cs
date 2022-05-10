using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DEBUGLOADING : MonoBehaviour
{
   // AsyncOperation operation;
    Scene sceneoB;
    // Start is called before the first frame update
    void Start()
    {
        //operation = SceneManager.LoadSceneAsync(4);
        sceneoB = SceneManager.GetActiveScene();
        Debug.Log("Active Scene is '" + sceneoB.name + "'.");

    }

    // Update is called once per frame
    void Update()
    {
        
        
       // Debug.Log(operation.progress + " LoadingProgress.. ");
    }
}
