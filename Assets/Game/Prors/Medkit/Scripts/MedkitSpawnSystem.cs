using UnityEngine;
using System.Collections.Generic;
//using System;

public class MedkitSpawnSystem : MonoBehaviour
{
    public GameObject Megkit;
    private GameObject Instance;
    public bool _isAutoSpawnOn;
    public int _autoSpawnCount;
    public GameObject SpawnPoint;
    public GameObject[] SpawnPoints;

    public float _timer;
    public int _spawnNumber;
    public int _spawnPoint;

    public Medkit MedkitScript;

    //public GameObject[] InstanceArr = new GameObject[3];
    //public List<GameObject> InstanceList = new List<GameObject>();
    //public int CountList;





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

        Instance = Instantiate(Megkit, SpawnPoint.transform.position, transform.rotation);
        Instance.transform.parent = transform;
        MedkitScript = Instance.GetComponent<Medkit>();
        MedkitScript.SpawnPoint = SpawnPoint;



        //EnemyMovement = Instance.GetComponent<EnemyMovement>();
        //EnemyMovement.Player = Player;//.transform;
        //}
        if (_spawnNumber == _autoSpawnCount)
        {
            _isAutoSpawnOn = false;
            _spawnNumber = 0;
        }
    }


    public void SpawnMedkit(GameObject NewSpawnPoint)
    {
        Debug.Log("Взял аптечку " + NewSpawnPoint.name);
        SpawnPoint = NewSpawnPoint;
        //Destroy(Instance);
        //SpawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];

        //InstanceArr[0] = Instantiate(Megkit, SpawnPoint.transform.position, transform.rotation);

        //Instance = Instantiate(Megkit, SpawnPoint.transform.position, transform.rotation);
        //Instance.transform.parent = transform;

        Invoke("SpawnMedkitNow", 5f);
        //InstanceList.Add(Instantiate(Megkit, SpawnPoint.transform.position, transform.rotation));
        //InstanceList[InstanceList.Count-1].transform.parent = transform;
        //InstanceList.Sort();
        //InstanceList.RemoveAll(x => x == null);

    // помещаем клон аптечки с систему спавна
    //InstanceArr[0].transform.parent = transform;
    //CountList = InstanceList.Count;
    //Sort();
    }

    public void SpawnMedkitNow()
    {
        Debug.Log("Спавн аптечки - " + SpawnPoint.name);
        Instance = Instantiate(Megkit, SpawnPoint.transform.position, transform.rotation);
        Instance.transform.parent = transform;
        MedkitScript = Instance.GetComponent<Medkit>();
        MedkitScript.SpawnPoint = SpawnPoint;
    }


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
