using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    CharacterActions Action;
    public static bool Paused;
    [SerializeField] GameObject TheCauseOfMyPain;
    

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
        TheCauseOfMyPain.SetActive(true);
        Paused = true;
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void ResumeGame()
    {
        //Debug.Log("Resume You Kentucky Fried Fuck");
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        TheCauseOfMyPain.SetActive(false);
        Paused = false;
        if (Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void ExitButton()

    {
        Application.Quit();
    }
    public void MainMenu( string _mainMenu)
    {
        SceneManager.LoadScene("YB_MainMenu");
    }

    public void theFunny()
    {
        Debug.Log("End my suffering");
    }
}
