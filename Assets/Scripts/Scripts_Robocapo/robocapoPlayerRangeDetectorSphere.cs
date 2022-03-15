using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robocapoPlayerRangeDetectorSphere : MonoBehaviour
{
    public static robocapoPlayerRangeDetectorSphere instance;
    [Tooltip("A bool that checks if the player is outside of the sphere's trigger collider")]
    public bool playerTooFarAway;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && bossAiRobocapo.instance.currentphase == 2)
        {
            playerTooFarAway = false;
            //Changes the bool in the animator, which allows the boss to use the animation that the bool is attatched to.
            bossAiRobocapo.instance.bossAnimator.SetBool("playerTooFar", false);
            bossAiRobocapo.instance.bossAnimator.SetBool("ShotLoop", false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player" && bossAiRobocapo.instance.currentphase == 2)
        {
            playerTooFarAway = true;
            //Changes the bool in the animator, which allows the boss to use the animation that the bool is attatched to.
            bossAiRobocapo.instance.bossAnimator.SetBool("playerTooFar", true);
        }
    }
}
