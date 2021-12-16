using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEmmitter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject obj;
    float timer = 0.5f;
    
    private void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            Instantiate(obj);
            timer = 0.5f;
        }
    }
    // Update is called once per frame
    IEnumerator Fire()
    {
        Instantiate(obj);
        yield return new WaitForSeconds(2);
    }
}
