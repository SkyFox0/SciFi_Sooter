using UnityEngine;

public class SpawnAmmo : MonoBehaviour
{
    public GameObject Ammo;
    private GameObject Instance;
    public GameObject SpawnPoint;
    public AmmoSpawnSystem AmmoSpawnSystem;
    public float _timeToSpawn = 15f;
    public AmmoBox AmmoScript;
    public bool _isSpawn = true;

    void Start()
    {
        if (AmmoSpawnSystem == null)
        {
            AmmoSpawnSystem = GetComponentInParent<AmmoSpawnSystem>();
        }
        _timeToSpawn = AmmoSpawnSystem._timeToSpawn;
        Ammo = AmmoSpawnSystem.Ammo;
    }

    public void AutoSpawn(GameObject NewSpawnPoint)
    {
        if (_isSpawn)
        {
            SpawnPoint = NewSpawnPoint;
            Invoke("SpawnAmmoNow", 1f);
        }
    }

    public void Spawn(GameObject NewSpawnPoint)
    {
        if (_isSpawn)
        {
            SpawnPoint = NewSpawnPoint;
            Invoke("SpawnAmmoNow", _timeToSpawn);
        }
    }
    public void SpawnAmmoNow()
    {
        //Debug.Log("Спавн патронов - " + SpawnPoint.name);
        SpawnPoint = this.gameObject;
        Instance = Instantiate(Ammo, SpawnPoint.transform.position, transform.rotation);
        Instance.transform.parent = AmmoSpawnSystem.transform;
        AmmoScript = Instance.GetComponent<AmmoBox>();
        AmmoScript.SpawnPoint = this.gameObject; //SpawnPoint;
    }
}
