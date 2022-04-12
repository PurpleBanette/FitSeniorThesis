using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossColorOverride : MonoBehaviour
{
    public Color defaultColor;
    public Color damagedColor;
    public float colorFade = 0.0f;
    public Material overrideMat;
    public GameObject targetMesh;
    public Color newOverlay;
    // Start is called before the first frame update
    void Start()
    {
        overrideMat = targetMesh.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (colorFade > 0)
        {
            colorFade -= 0.02f;
            newOverlay = Color.Lerp(defaultColor, damagedColor, colorFade);
            overrideMat.SetColor("_OverrideColorRef", newOverlay);  
            if (colorFade <= 0)
            {
                overrideMat.SetColor("_OverrideColorRef", defaultColor);
                colorFade = 0;
            }
        }
    }
}
