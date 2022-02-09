using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RC_ObjectPool : MonoBehaviour
{
    public static RC_ObjectPool instance;

    public List<GameObject> pooledBullets;
    public List<GameObject> pooledBuckshot;


    [SerializeField] GameObject Bullets;
    [SerializeField] GameObject Buckshot;

    public int bulletsToPool;
    public int buckshotToPool;

    public GameObject L_defaultTarget;
    public GameObject R_defaultTarget;

    private void Awake()
    {
        instance = this;

        poolSetup(Bullets, pooledBullets, bulletsToPool);
        poolSetup(Buckshot, pooledBuckshot, buckshotToPool);
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
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
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

    public void LaunchObject(GameObject CurrentBullet, Transform playerTarget, Transform projectileSpawnPoint, float spreadValue, List<GameObject> particlepool, float shotForce)
    {
        
        Vector3 bulletDirection = playerTarget.transform.position - projectileSpawnPoint.transform.position;

        //calculate direction from weapon to player
        //Vector3 bulletDirection = playerTarget.transform.position - p2BulletPosition.transform.position;

        //calculate spread
        float bulletspreadX = Random.Range(-spreadValue, spreadValue);
        float bulletspreadY = Random.Range(-spreadValue, spreadValue);

        //calculate new direction with spread
        Vector3 bulletDirectionSpread = bulletDirection + new Vector3(bulletspreadX, bulletspreadY, 0);
        //Set to null if no particles are used
        if (particlepool != null)
        {
            GameObject particle = GetPooledObject(particlepool, true);
            particle.transform.position = projectileSpawnPoint.position;
            particle.transform.rotation = projectileSpawnPoint.rotation;
            particle.SetActive(true);
        }

        //rotate bullet/projectile to shoot direction
        CurrentBullet.transform.forward = bulletDirectionSpread.normalized;

        //add forces to bullet/projectile
        CurrentBullet.GetComponent<Rigidbody>().AddForce(bulletDirectionSpread.normalized * shotForce, ForceMode.Impulse);

    }

    public int GetPooledObjectManaged(List<GameObject> pooledObjects, int objectManager, Transform playerTarget, Transform projectileSpawnPoint, float spreadValue, List<GameObject> particlepool, float shotForce)
    {
        Vector3 bulletDirection;
        
        bulletDirection = playerTarget.transform.position - projectileSpawnPoint.transform.position;
        

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
            GameObject particle = GetPooledObject(particlepool, true);
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
