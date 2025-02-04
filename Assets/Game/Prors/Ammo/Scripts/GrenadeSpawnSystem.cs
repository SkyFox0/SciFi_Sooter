using UnityEngine;

public class GrenadeSpawnSystem : MonoBehaviour
{
    public GameObject GrenadeBox;
    private GameObject Instance;
    public bool _isAutoSpawnOn;
    public int _autoSpawnCount;
    public GameObject[] SpawnPoints;
    private GameObject SpawnPoint;
    public float _timeToSpawn = 15f;

    public float _timer;
    public int _spawnNumber;
    public int _spawnPoint;

    public GrenadeBox GrenadeScript;
    public SpawnGrenade SpawnGrenadeScript;

    void Start()
    {
        if (_isAutoSpawnOn && _autoSpawnCount > 0)
        {
            _spawnNumber = 0;
            _spawnPoint = 0;
            _timer = 0f;
            Destroy(Instance);
            //AvtoSpavn(_autoSpawnCount); 
        }
    }

    private void Update()
    {
        if (_isAutoSpawnOn && (_spawnNumber < _autoSpawnCount))
        {
            _timer = _timer + Time.deltaTime;
            if (_timer > 1)
            {
                _timer = 0;
                _spawnNumber += 1;
                AvtoSpavn();

            }
        }
    }

    public void AvtoSpavn()
    {
        //for (int i = 0; i < _autoSpawnCount; i++)
        //for (int i = 0; i < SpawnPoints.Length; i++)
        //{
        if (_spawnPoint < SpawnPoints.Length)
        {
            SpawnPoint = SpawnPoints[_spawnPoint];
            _spawnPoint += 1;
        }
        else
        {
            _spawnPoint = _spawnPoint - SpawnPoints.Length;
            SpawnPoint = SpawnPoints[_spawnPoint];
            _spawnPoint += 1;
        }

        SpawnGrenadeScript = SpawnPoint.GetComponent<SpawnGrenade>();
        SpawnGrenadeScript.AutoSpawn(SpawnPoint);

        /*        Instance = Instantiate(Megkit, SpawnPoint.transform.position, transform.rotation);
                Instance.transform.parent = transform;
                MedkitScript = Instance.GetComponent<Medkit>();
                MedkitScript.SpawnPoint = SpawnPoint;
        */


        //EnemyMovement = Instance.GetComponent<EnemyMovement>();
        //EnemyMovement.Player = Player;//.transform;
        //}
        if (_spawnNumber == _autoSpawnCount)
        {
            _isAutoSpawnOn = false;
            _spawnNumber = 0;
        }
    }

    public void Spawn()
    {
        Invoke("SpawnGrenade", _timeToSpawn);
    }

/*    public void SpawnGrenade()
    {
        //Destroy(Instance);
        SpawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];

        Instance = Instantiate(GrenadeBox, SpawnPoint.transform.position, transform.rotation);
        // помещаем клон аптечки с систему спавна
        Instance.transform.parent = transform;
    }*/

    public void SpawnGrenade(GameObject NewSpawnPoint, GameObject Grensde)
    {
        SpawnPoint = NewSpawnPoint;
        SpawnGrenadeScript = NewSpawnPoint.GetComponent<SpawnGrenade>();
        SpawnGrenadeScript.Spawn(SpawnPoint);
    }
}
