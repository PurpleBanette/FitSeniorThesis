using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMoTracker : MonoBehaviour
{
    public static SlowMoTracker instance;

    [SerializeField] float slowMoTimeScale = 0.2f;
    [SerializeField] float slowMoDuration = 1f;
    public IEnumerator activateSlowMo()
    {
        Time.timeScale = slowMoTimeScale;
        yield return new WaitForSecondsRealtime(slowMoDuration);
        Time.timeScale = 1;
    }
}
