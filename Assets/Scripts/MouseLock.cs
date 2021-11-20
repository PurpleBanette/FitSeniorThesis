using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLock : MonoBehaviour
{
    CharacterActions controller;

    private void Awake()
    {
        controller = new CharacterActions();
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //Locks the cursor to the center
        Cursor.visible = false; //Cursor is invisible
    }

    private void OnEnable()
    {
        controller.Player.MouseLock.performed += lockUnlock;
        controller.Player.MouseLock.Enable();
        //customCharActions.Player.Jump.performed += DoJump;
        //customCharActions.Player.Jump.Enable();
    }

    private void lockUnlock(InputAction.CallbackContext obj)
    {
        Lock();
    }

    void Lock()
    {
       
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            if (Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

    }
    
    

}
