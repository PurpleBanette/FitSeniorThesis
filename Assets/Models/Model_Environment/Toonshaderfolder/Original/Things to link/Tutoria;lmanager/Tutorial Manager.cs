using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
   public GameObject [] popUps;
    private int popUpIndex;
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
            if (Input.GetAxis("Horizontal") == Input.GetAxis("Vertical"))
            {
                popUpIndex ++;
            }
            else if (popUpIndex == 1)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    popUpIndex++;
                }
            }
            else if (popUpIndex == 2)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    popUpIndex++;
                }
            }
        }
    }
}
