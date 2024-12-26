using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public int Ammo = 50;
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
