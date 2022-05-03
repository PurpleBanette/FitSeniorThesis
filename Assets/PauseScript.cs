using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    CharacterActions Action;
    public static bool Paused;

    private void Awake()
    {
        Action = new CharacterActions();
    }
    private void OnEnable()
    {
        Action.Enable();
    }
    private void OnDisable()
    {
        Action.Disable();
    }

    private void Start()
    {
        Action.Ui.Pause.performed += _ => DeterminePause();
    }

    void DeterminePause()
    {
        if (!Paused)
        {
            PauseGame();
            
        }
        else
        {
            //Debug.Log("Is this code even Running?");
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        Paused = true;
    }

    public void ResumeGame()
    {
        //Debug.Log("Resume You Kentucky Fried Fuck");
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        Paused = false;
    }
}
