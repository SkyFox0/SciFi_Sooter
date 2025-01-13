using UnityEngine;

public class AmmoSpawnSystem : MonoBehaviour
{
    public GameObject Ammo;
    private GameObject Instance;    
    public GameObject[] SpawnPoints;    
    private GameObject SpawnPoint;
    public float _timeToSpawn = 15f;
      

    public void Spawn()
    {
        Invoke("Spawnammo", _timeToSpawn);
    }

    public void SpawnAmmo()
    {
        //Destroy(Instance);
        SpawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        Instance = Instantiate(Ammo, SpawnPoint.transform.position, transform.rotation);
        // помещаем клон аптечки с систему спавна
        Instance.transform.parent = transform;
    }

    

}
