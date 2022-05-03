using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PauseUnpause : MonoBehaviour
{
    CharacterActions customCharActions;
    InputAction MouseLock;

    public bool isPaused;
    
    private void OnAwake()
    {
       
        customCharActions = new CharacterActions();
        
    }
    private void OnEnable()
    {
        MouseLock = customCharActions.Player.MouseLock;
        MouseLock.Enable();
   
      /*  if ()
        {
            
        }
      */
    }


    private void InMainMenu()

    {
       
    }
    private void OnDisable()
    {
        MouseLock.Disable();
    }

    private void OnPausePreform(InputAction.CallbackContext context)
    {
        //bool isPaused = context.ReadValue<bool>();
        //Debug.Log($"Move Input: (ispaused)");
    }
    private void DoPause(InputAction.CallbackContext context)
    {
        if (isPaused == true)
        {
            isPaused = false;
        }
        if (isPaused == false)
        {
            isPaused = true;
        }
        Debug.Log("Is Paused");
    }
}