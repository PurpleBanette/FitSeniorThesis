using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obsidianWeaponPickup : MonoBehaviour
{
    bossAiObsidian bossRef;
    // Start is called before the first frame update
    [SerializeField] GameObject wepParticle;

    void Awake()
    {
        
    }

    void Start()
    {
        bossRef = GetComponent<bossAiObsidian>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartParticle()
    {
        Instantiate(wepParticle, this.transform.position, Quaternion.identity);
    }
    public void DeleteParticle()
    {
        Destroy(wepParticle);
    }
}
