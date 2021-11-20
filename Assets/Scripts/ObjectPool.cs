using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    public List<GameObject> pooledBullets;
    public List<GameObject> pooledMiniMeteors;
    public List<GameObject> pooledGrenades;
    public List<GameObject> pooledBarrageL;
    public List<GameObject> pooledBarrageR;
    public List<GameObject> pooledBlasterBullets;

    public List<GameObject> pooledmuzzleFlash;
    public List<GameObject> pooledblastermuzzleFlash;
    public List<GameObject> pooledbarragemuzzleFlashL;
    public List<GameObject> pooledbarragemuzzleFlashR;
    public List<GameObject> pooledmeteorVfxL;
    public List<GameObject> pooledmeteorVfxR;
    public List<GameObject> chargeMuzzleflashes;
    public List<GameObject> jumpMuzzleflashes;

    [SerializeField]
    GameObject phase2Bullets;
    [SerializeField]
    GameObject miniMeteors;
    [SerializeField] GameObject barrageProjectile;
    [SerializeField]
    GameObject grenades;
    [SerializeField] GameObject blasterBullets;
    public GameObject MeteorL;
    public GameObject MeteorR;

    public int bulletsToPool = 60;
    public int blasterToPool = 60;
    public int meteorsToPool = 20;
    public int grenadestopool = 10;
    public int barragePool = 10;

    public GameObject rapidLaserParticle;
    public GameObject blasterParticle;
    [SerializeField] GameObject barrageParticle;
    [SerializeField] GameObject meteorParticle;
    [SerializeField] GameObject chargeMuzzleParticle;
    [SerializeField] GameObject jumpMuzzleParticle;

    /*
    [HideInInspector] public GameObject leftMeteorPart;
    [HideInInspector] public GameObject rightMeteorPart;
    [HideInInspector] public GameObject leftBarragePart;
    [HideInInspector] public GameObject rightBarragePart;
    */
    [SerializeField] bool canExpand = false;

    void Awake()
    {
        instance = this;

        MeteorL = Instantiate(MeteorL);
        MeteorL.SetActive(false);
        MeteorR = Instantiate(MeteorR);
        MeteorR.SetActive(false);

        poolSetup(phase2Bullets, pooledBullets, bulletsToPool);
        poolSetup(miniMeteors, pooledMiniMeteors, meteorsToPool);
        poolSetup(grenades, pooledGrenades, grenadestopool);
        poolSetup(barrageProjectile, pooledBarrageR, barragePool);
        poolSetup(barrageProjectile, pooledBarrageL, barragePool);
        poolSetup(blasterBullets, pooledBlasterBullets, blasterToPool);
        
        poolSetup(rapidLaserParticle, pooledmuzzleFlash, 4);
        poolSetup(blasterParticle, pooledblastermuzzleFlash, 4);
        poolSetup(meteorParticle, pooledmeteorVfxL, 2);
        poolSetup(meteorParticle, pooledmeteorVfxR, 2);
        poolSetup(barrageParticle, pooledbarragemuzzleFlashL, 2);
        poolSetup(barrageParticle, pooledbarragemuzzleFlashR, 2);
        poolSetup(chargeMuzzleParticle, chargeMuzzleflashes, 2);
        poolSetup(jumpMuzzleParticle, jumpMuzzleflashes, 2);



    }


    void poolSetup(GameObject objectToPool, List<GameObject> pooledObjects, int amountToPool)
    {
        //pooledObjects = new List<GameObject>();
        GameObject obj;
        for (int i = 0; i < amountToPool; i++)
        {
            obj = Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject(List<GameObject> pooledObjects, bool expandable)
    {
        for(int i = 0; i < pooledObjects.Count; i++)
        {
            if(!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        if (expandable)
        {
            GameObject obj = pooledObjects[0];
            Instantiate(obj);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            return obj;
        }
        else
        {
            return null;
        }
        
    }
    public int GetPooledObjectManaged(List<GameObject> pooledObjects, int objectManager, Transform playerTarget, Transform projectileSpawnPoint, float spreadValue, List<GameObject> particlepool, float shotForce)
    {
        Vector3 bulletDirection = playerTarget.transform.position - projectileSpawnPoint.transform.position;
        
        //calculate direction from weapon to player
        //Vector3 bulletDirection = playerTarget.transform.position - p2BulletPosition.transform.position;

        //calculate spread
        float bulletspreadX = Random.Range(-spreadValue, spreadValue);
        float bulletspreadY = Random.Range(-spreadValue, spreadValue);

        //calculate new direction with spread
        Vector3 bulletDirectionSpread = bulletDirection + new Vector3(bulletspreadX, bulletspreadY, 0);


        //sets the bullet to active and positions it correctly
        GameObject CurrentBullet = pooledObjects[objectManager];
        if (!CurrentBullet.activeInHierarchy)
        {

            CurrentBullet.SetActive(true);
            CurrentBullet.transform.position = projectileSpawnPoint.position;
            CurrentBullet.transform.rotation = projectileSpawnPoint.rotation;
            
        }
        else
        {
            GameObject obj = Instantiate(pooledObjects[0], projectileSpawnPoint);
            pooledObjects.Add(obj);
            Debug.Log("Needs More Bullets");
        }
        

        //only instantiates the particle if one is present, prevents compiler error if one is not
        if (particlepool != null)
        {
            GameObject particle = GetPooledObject(particlepool,true);
            particle.transform.position = projectileSpawnPoint.position;
            particle.transform.rotation = projectileSpawnPoint.rotation;
            particle.SetActive(true);
        }

        //rotate bullet/projectile to shoot direction
        CurrentBullet.transform.forward = bulletDirectionSpread.normalized;

        //add forces to bullet/projectile
        CurrentBullet.GetComponent<Rigidbody>().AddForce(bulletDirectionSpread.normalized * shotForce, ForceMode.Impulse);
        
        objectManager++;
        //Debug.Log(bulletPoolManager);
        if (objectManager >= pooledObjects.Count)
        {
            objectManager = 0;
        }

        return objectManager;
    }
    public void retrievePooledObject(GameObject obj, Transform spawnPoint)
    {
        obj.SetActive(false);
        obj.transform.position = spawnPoint.position;
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}