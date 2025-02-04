using UnityEngine;
using UnityEngine.UI;

public class AmmoBox : MonoBehaviour
{
    public int Ammo = 20;
    public Text _text;
    public AmmoSpawnSystem AmmoSpawnSystem;
    public GameObject SpawnPoint;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _text.text = Ammo.ToString();
        if (AmmoSpawnSystem == null )
        {
            AmmoSpawnSystem = GetComponentInParent<AmmoSpawnSystem>();
            //AmmoSpawnSystem = GetComponent<AmmoSpawnSystem>();
            //AmmoSpawnSystem = GameObject.FindGameObjectWithTag
        }
        
    }

    public void DecriceAmmo(int _kol)
    {
        Ammo -= _kol;
        _text.text = Ammo.ToString();
    }

    // Update is called once per frame
    public void Destroy()
    {
        //AmmoSpawnSystem.Spawn();
        try
        {
            AmmoSpawnSystem.SpawnAmmo(SpawnPoint, transform.gameObject);
        }
        catch { }
        
        Destroy(gameObject);
    }
}
