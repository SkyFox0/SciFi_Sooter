using UnityEngine;
using UnityEngine.UI;

public class GrenadeBox : MonoBehaviour
{
    public int _grenade = 5;//  сколько гранат в ящике
    public Text _text;
    public GrenadeSpawnSystem GrenadeSpawnSystem;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _text.text = _grenade.ToString();
        if (GrenadeSpawnSystem == null)
        {
            GrenadeSpawnSystem = GetComponentInParent<GrenadeSpawnSystem>();
        }
        //AmmoSpawnSystem = GetComponentInParent<AmmoSpawnSystem>();

    }

    public void DecriceGrenade(int _kol)
    {
        _grenade -= _kol;
        _text.text = _grenade.ToString();
    }

    // Update is called once per frame
    public void Destroy()
    {
        GrenadeSpawnSystem.Spawn();
        Destroy(gameObject);

    }
}
