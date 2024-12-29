using UnityEngine;
using UnityEngine.UI;

public class AmmoBoxEnemyDrop : MonoBehaviour
{
    public int Ammo;
    public Text _text;
    //public AmmoSpawnSystem AmmoSpawnSystem;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Ammo = Random.Range(5, 15);
        _text.text = Ammo.ToString();
        //AmmoSpawnSystem = GetComponentInParent<AmmoSpawnSystem>();
    }

    // Update is called once per frame
    public void Destroy()
    {
        //AmmoSpawnSystem.Spawn();
       Destroy(gameObject);

    }
}
