using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class cutsceneTrigger : MonoBehaviour
{
    public static cutsceneTrigger instance;
    public PlayableDirector director;
    public GameObject controlPanel;
    // Start is called before the first frame update
    void Awake()
    {
        director = GetComponent<PlayableDirector>();
        director.played += Director_Played;
        director.stopped += Director_Stopped;
    }

    public void Director_Played(PlayableDirector obj)
    {
        controlPanel.SetActive(false);
    }
    public void Director_Stopped(PlayableDirector obj)
    {
        controlPanel.SetActive(true);
    }
    public void StartTimeline()
    {
        director.Play();
    }
}
