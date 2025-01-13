using UnityEngine;

public class Medkit : MonoBehaviour
{
    public int Health = 50;
    public MedkitSpawnSystem MedkitSpawnSystem;
    public GameObject SpawnPoint;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (MedkitSpawnSystem == null)
        {
            MedkitSpawnSystem = GetComponentInParent<MedkitSpawnSystem>();
        }
    }

    // Update is called once per frame
    public void Destroy()        
    {
        Debug.Log("”ничтожение аптечки # " + SpawnPoint.name);
        MedkitSpawnSystem.SpawnMedkit(SpawnPoint, transform.gameObject); 
        Destroy(gameObject);

    }
}
