using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class disableTimer : MonoBehaviour
{
    [SerializeField] float maxLifetime = 0.4f;
    VisualEffect vfx;
    //float lifetimeReset;

    private void Awake()
    {
        vfx = GetComponent<VisualEffect>();
        vfx.Stop();
    }

    private void OnEnable()
    {
        Invoke("Deactivate", maxLifetime);
        vfx.Play();
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        vfx.Stop();
        vfx.Reinit();
    }

    // Update is called once per frame
    /*
    void Update()
    {
        maxLifetime -= Time.deltaTime;

        if (maxLifetime <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        maxLifetime = lifetimeReset;
    }
    */
}
