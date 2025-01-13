using UnityEngine;

public class SpawnMedkit : MonoBehaviour
{
    public GameObject Megkit;
    private GameObject Instance;
    public GameObject SpawnPoint;
    public MedkitSpawnSystem MedkitSpawnSystem;
    public float _timeToSpawn = 15f;
    public Medkit MedkitScript;
    public bool _isSpawn = true;

    void Start()
    {
        if (MedkitSpawnSystem == null)
        {
            MedkitSpawnSystem = GetComponentInParent<MedkitSpawnSystem>();
        }
        _timeToSpawn = MedkitSpawnSystem._timeToSpawn;
        Megkit = MedkitSpawnSystem.Megkit;
    }

    public void Spawn(GameObject NewSpawnPoint)
    {
        if (_isSpawn)
        {
            SpawnPoint = NewSpawnPoint;
            Invoke("SpawnMedkitNow", _timeToSpawn);
        }
        
    }
    public void SpawnMedkitNow()
    {
        //Debug.Log("Спавн аптечки - " + SpawnPoint.name);
        SpawnPoint = this.gameObject;
        Instance = Instantiate(Megkit, SpawnPoint.transform.position, transform.rotation);        
        Instance.transform.parent = MedkitSpawnSystem.transform;
        MedkitScript = Instance.GetComponent<Medkit>();
        MedkitScript.SpawnPoint = this.gameObject; //SpawnPoint;
        
    }

}
