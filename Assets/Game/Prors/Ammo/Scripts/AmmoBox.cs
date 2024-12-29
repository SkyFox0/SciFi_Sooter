using UnityEngine;
using UnityEngine.UI;

public class AmmoBox : MonoBehaviour
{
    public int Ammo = 20;
    public Text _text;
    public AmmoSpawnSystem AmmoSpawnSystem;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _text.text = Ammo.ToString();   
        AmmoSpawnSystem = GetComponentInParent<AmmoSpawnSystem>();
    }

    // Update is called once per frame
    public void Destroy()
    {
        AmmoSpawnSystem.Spawn();
        Destroy(gameObject);

    }
}
