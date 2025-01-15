using UnityEngine;

public class GrenadeSpawnSystem : MonoBehaviour
{
    public GameObject GrenadeBox;
    private GameObject Instance;
    public GameObject[] SpawnPoints;
    private GameObject SpawnPoint;
    public float _timeToSpawn = 15f;

    public void Spawn()
    {
        Invoke("SpawnGrenade", _timeToSpawn);
    }

    public void SpawnGrenade()
    {
        //Destroy(Instance);
        SpawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];

        Instance = Instantiate(GrenadeBox, SpawnPoint.transform.position, transform.rotation);
        // помещаем клон аптечки с систему спавна
        Instance.transform.parent = transform;
    }
}
