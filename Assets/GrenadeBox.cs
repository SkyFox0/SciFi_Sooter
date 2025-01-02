using UnityEngine;
using UnityEngine.UI;

public class GrenadeBox : MonoBehaviour
{
    public int _grenade = 3;//  сколько гранат в ящике
    public Text _text;
    //public AmmoSpawnSystem AmmoSpawnSystem;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _text.text = _grenade.ToString();
        //AmmoSpawnSystem = GetComponentInParent<AmmoSpawnSystem>();
    }

    // Update is called once per frame
    public void Destroy()
    {
        //AmmoSpawnSystem.Spawn();
        Destroy(gameObject);

    }
}
