using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class _Tutorial_Manager : MonoBehaviour
   
{
    [SerializeField]
    CharacterActions customCharActions;

    public GameObject[] popUps;
    private int popUpIndex;
     ModifiedTPC Cont;
    

    private void Update()
    {
        for (int i = 0; i < popUps.Length; i++)
        {
            if (i == popUpIndex)
            {
                popUps[popUpIndex].SetActive(true);
            }
            else
            {
                popUps[popUpIndex].SetActive(false);
            }
        }


       if(popUpIndex == 0)
          {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                popUpIndex++;
            }
            else if (popUpIndex == 1)
                if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                    popUpIndex++;
            }
                else if (popUpIndex == 2)
                {

                }
          }
            
        
    }
}
