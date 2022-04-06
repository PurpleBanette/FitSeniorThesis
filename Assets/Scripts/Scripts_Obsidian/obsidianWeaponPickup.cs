using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obsidianWeaponPickup : MonoBehaviour
{
    public static obsidianWeaponPickup instance;
    // Start is called before the first frame update
    [SerializeField] GameObject wepParticle;
    float bossWeaponRadius = 5f;
    public bool playerInWeaponRange = false;
    [SerializeField] LayerMask bossPickupDetector;
    public GameObject weaponEmpty;
    public Rigidbody weprb;

    public Vector3 itemPosition;
    public Vector3 itemRotation;
    public Vector3 itemScale;

    void Awake()
    {
        instance = this;
        weprb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        //bossRef = GetComponent<bossAiObsidian>();
        StartParticle();
    }

    // Update is called once per frame
    void Update()
    {
        playerInWeaponRange = Physics.CheckSphere(weaponEmpty.transform.position, bossWeaponRadius, bossPickupDetector);
        if (playerInWeaponRange) GiveWeapon();
    }

    public void StartParticle()
    {
        Instantiate(wepParticle, this.transform.position, Quaternion.identity);
        wepParticle.SetActive(true);
    }
    public void GiveWeapon()
    {
        if (bossAiObsidian.instance.dead)
        {
            ModifiedTPC.instance.PlayerPickupWeapon();
            wepParticle.SetActive(false);
        }
    }
}
