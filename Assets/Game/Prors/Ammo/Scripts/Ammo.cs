using UnityEngine;

public class Ammo_Box : MonoBehaviour
{
    public int _Ammo = 20;
    public AmmoSpawnSystem AmmoSpawnSystem;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AmmoSpawnSystem = GetComponentInParent<AmmoSpawnSystem>();
    }

    // Update is called once per frame
    public void Destroy()
    {
        AmmoSpawnSystem.Spawn();
        Destroy(gameObject);

    }
}
