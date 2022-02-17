using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Scaleable : MonoBehaviour
{
    public float time = 0f;
    public float growTime = 6f;
    public float maxWidth = 2f;
    public float maxHeight = 2f;
    public float maxDepth = 2f;

    public bool isMaxSize = false;

    void Start()
  {
    if(isMaxSize == false)
        {
            StartCoroutine(Grow());
        }
  }
    private IEnumerator Grow()
    {
        Vector2 startScale = transform.localScale;
        Vector2 maxScale = new Vector3(maxWidth, maxHeight, maxDepth);

        do
        {
            //Grow
            transform.localScale = Vector3.Lerp(startScale, maxScale, time / growTime);
            //Increment timer
            time += Time.deltaTime;
            //yeild
            yield return null;
        }
        while (time < growTime);
        isMaxSize = true;
    }
    
    
}