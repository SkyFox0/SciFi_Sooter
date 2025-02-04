using UnityEngine;
using System.Collections.Generic;


public class MedkitSpawnSystem : MonoBehaviour
{
    public GameObject Megkit;
    private GameObject Instance;
    public bool _isAutoSpawnOn;
    public int _autoSpawnCount;
    public float _timeToSpawn = 15f;
    public GameObject SpawnPoint;
    public GameObject[] SpawnPoints;

    public float _timer;
    public int _spawnNumber;
    public int _spawnPoint;

    public Medkit MedkitScript;
    public SpawnMedkit SpawnMedkitScript;


    //Start is called once before the first execution of Update after the MonoBehaviour is created
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

    // Update is called once per frame

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

        SpawnMedkitScript = SpawnPoint.GetComponent<SpawnMedkit>();
        SpawnMedkitScript.AutoSpawn(SpawnPoint);

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


    public void SpawnMedkit(GameObject NewSpawnPoint, GameObject Medkit)
    {
        Debug.Log("Взял аптечку " + NewSpawnPoint.name + Medkit.ToString());
        SpawnPoint = NewSpawnPoint;
        SpawnMedkitScript = NewSpawnPoint.GetComponent<SpawnMedkit>();
        SpawnMedkitScript.Spawn(SpawnPoint);        

        //Invoke("SpawnMedkitNow", _timeToSpawn);        
    }

    /*public void SpawnMedkitNow()
    {
        Debug.Log("Спавн аптечки - " + SpawnPoint.name);
        Instance = Instantiate(Megkit, SpawnPoint.transform.position, transform.rotation);
        Instance.transform.parent = transform;
        MedkitScript = Instance.GetComponent<Medkit>();
        MedkitScript.SpawnPoint = SpawnPoint;
    }*/


    /*public void Sort()
    {
        for (int i = 0; i < InstanceList.Count-1; i++)
        {
            if (InstanceList[i] == null && i < InstanceList.Count)
            {
                Debug.Log("i = " + i.ToString());
                if (InstanceList[i + 1] != null)
                {
                    InstanceList[i] = InstanceList[i + 1];
                    InstanceList[i + 1] = null;
                }
                
                //InstanceList[i] = InstanceList[i + 1];
                //InstanceList[i + 1] = null;
            }
        }
        //InstanceList.RemoveAll();

    }*/

}
