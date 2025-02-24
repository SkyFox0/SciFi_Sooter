using UnityEngine;

public class SpawnGrenade : MonoBehaviour
{
    public GameObject Grenade;
    private GameObject Instance;
    public GameObject SpawnPoint;
    public GrenadeSpawnSystem GrenadeSpawnSystem;
    public float _timeToSpawn = 15f;
    public GrenadeBox GrenadeScript;
    public bool _isSpawn = true;

    void Start()
    {
        if (GrenadeSpawnSystem == null)
        {
            GrenadeSpawnSystem = GetComponentInParent<GrenadeSpawnSystem>();
        }
        _timeToSpawn = GrenadeSpawnSystem._timeToSpawn;
        Grenade = GrenadeSpawnSystem.GrenadeBox;
    }

    public void AutoSpawn(GameObject NewSpawnPoint)
    {
        if (_isSpawn)
        {
            SpawnPoint = NewSpawnPoint;
            Invoke("SpawnGrenadeNow", 1f);
        }
    }

    public void Spawn(GameObject NewSpawnPoint)
    {
        if (_isSpawn)
        {
            SpawnPoint = NewSpawnPoint;
            Invoke("SpawnGrenadeNow", _timeToSpawn);
        }
    }
    public void SpawnGrenadeNow()
    {
        //Debug.Log("Спавн патронов - " + SpawnPoint.name);
        SpawnPoint = this.gameObject;
        Instance = Instantiate(Grenade, SpawnPoint.transform.position, transform.rotation);
        Instance.transform.parent = GrenadeSpawnSystem.transform;
        GrenadeScript = Instance.GetComponent<GrenadeBox>();
        GrenadeScript.SpawnPoint = this.gameObject; //SpawnPoint;
    }
}