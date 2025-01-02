using UnityEngine;
using StarterAssets;

public class GrenadeBoxTrigger : MonoBehaviour
{
    public int _grenade; // сколько гранат в ящике
    public int _addGrenadeMax;  // сколько гранат может взять игрок
    public int _addGrenade;  // сколько гранат взял игрок по факту
    public GrenadeBox GrenadeBox;

    void Start()
    {
        GrenadeBox = GetComponentInParent<GrenadeBox>();
        _grenade = GrenadeBox._grenade;
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Подняты гранаты");
        if (other.gameObject.tag == "Player")
        {
            if (other.GetComponent<My_Weapon_Controller>()._totalGrenades < other.GetComponent<My_Weapon_Controller>()._maxGrenades)
            {
                _addGrenadeMax = other.GetComponent<My_Weapon_Controller>()._maxGrenades - other.GetComponent<My_Weapon_Controller>()._totalGrenades;
                if (_grenade <= _addGrenadeMax)
                {                    
                    _addGrenade = _grenade;
                    Debug.Log("Игрок поднял гранаты +" + _addGrenade.ToString() + "шт!");
                    other.GetComponent<My_Weapon_Controller>().AddGrenades(_addGrenade);
                    GrenadeBox.Destroy();
                }
                else
                {
                    _addGrenade = _addGrenadeMax;
                    _grenade -= _addGrenade;

                    GrenadeBox._grenade = _grenade;
                    GrenadeBox._text.text = GrenadeBox._grenade.ToString();
                    Debug.Log("Игрок поднял гранаты +" + _addGrenade.ToString() + "шт!");
                    other.GetComponent<My_Weapon_Controller>().AddGrenades(_addGrenade);
                }
                
            }
        }
    }
}
