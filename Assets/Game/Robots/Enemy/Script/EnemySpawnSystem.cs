using UnityEngine;

public class EnemySpawnSystem : MonoBehaviour
{
    public bool _isAutoSpawnOn;
    public int _spawnCount;
    public float _timeToSpawn = 10f;
    public GameObject Enemy;
    private GameObject Instance;
    private EnemyMovement EnemyMovement;
    public GameObject Player;
    public GameObject[] SpawnPoints;
    //public GameObject SpawnPoint_1;
    //public GameObject SpawnPoint_2;
    //public GameObject SpawnPoint_3;
    private GameObject SpawnPoint;
    public float _timer;
    public int _spawnNumber;
    public int _spawnPoint;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {       
        if (_isAutoSpawnOn && _spawnCount > 0) 
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            _spawnNumber = 0;
            _spawnPoint = 0;
            _timer = 0f;
            Destroy(Instance);
            //AvtoSpavn(_autoSpawnCount); 
        }
    }

    private void Update()
    {
        if (_isAutoSpawnOn && (_spawnNumber < _spawnCount))
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

    // Update is called once per frame
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
            

            Instance = Instantiate(Enemy, SpawnPoint.transform.position, transform.rotation);
            // помещаем клон врага с систему спавна
            Instance.transform.parent = transform;


            EnemyMovement = Instance.GetComponent<EnemyMovement>();
            EnemyMovement.Player = Player;//.transform;
        //}
        /*if (_spawnNumber == _spawnCount)
        {
            _isAutoSpawnOn = false;
            _spawnNumber = 0;
        }*/
    }

    public void ReSpawn()
    {
        _isAutoSpawnOn = true;
        _spawnNumber = 0;
        _timer = 0f;
    }


    public void Spawn()
    {        
        Invoke("SpawnEnemy", (_timeToSpawn + Random.Range(-5f, 5f)));        
    }

    public void SpawnEnemy()
    {
        Debug.Log("Спавн нового врага");
        //Destroy(Instance);
        SpawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        Instance = Instantiate(Enemy, SpawnPoint.transform.position, transform.rotation);
        // помещаем клон врага с систему спавна
        Instance.transform.parent = transform;

        EnemyMovement = Instance.GetComponent<EnemyMovement>();
        EnemyMovement.Player = Player;//.transform;
    }
}
