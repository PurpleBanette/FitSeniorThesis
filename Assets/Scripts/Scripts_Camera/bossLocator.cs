using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bossLocator : MonoBehaviour
{
    public static bossLocator instance;
    public Image bossMarker;
    public Transform markerTarget;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        float minX = bossMarker.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = bossMarker.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        Vector2 pos = Camera.main.WorldToScreenPoint(markerTarget.position);

        if(Vector3.Dot((markerTarget.position - transform.position), transform.forward) < 0)
        {
            //Marker is behind the player
            //bossMarker.enabled = true;
            if(pos.x < Screen.width / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }
        else
        {
            //bossMarker.enabled = false;
        }

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        bossMarker.transform.position = pos;
    }
}
