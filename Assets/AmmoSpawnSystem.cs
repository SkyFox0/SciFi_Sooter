using UnityEngine;

public class AmmoSpawnSystem : MonoBehaviour
{
    public GameObject Ammo;
    private GameObject Instance;    
    public GameObject[] SpawnPoints;    
    private GameObject SpawnPoint;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
    //    Destroy(Instance);
    //Player = GameObject.Find("Player");

    //    for (int i = 0; i < SpawnPoints.Length; i++)
    //    {
    //       SpawnPoint = SpawnPoints[i];

    //       Instance = Instantiate(Megkit, SpawnPoint.transform.position, transform.rotation);
    //EnemyMovement = Instance.GetComponent<EnemyMovement>();
    //EnemyMovement.Player = Player.transform;
    //  }        
    // }

    // Update is called once per frame

    public void Spawn()
    {
        //Destroy(Instance);
        SpawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];


        Instance = Instantiate(Ammo, SpawnPoint.transform.position, transform.rotation);
        // помещаем клон аптечки с систему спавна
        Instance.transform.parent = transform;

        //EnemyMovement = Instance.GetComponent<EnemyMovement>();
        //EnemyMovement.Player = Player.transform;
    }
}
