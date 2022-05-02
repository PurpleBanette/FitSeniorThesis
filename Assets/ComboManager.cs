using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class ComboManager : MonoBehaviour
{
    public static ComboManager instance;

    bool canRecieveInput;
    bool inputRecieved;
 
    void Awake()
    {
        instance = this;
    }

    void Attack(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (canRecieveInput)
            {
                inputRecieved = true;
                canRecieveInput = false;
            }
        }
    }
}
