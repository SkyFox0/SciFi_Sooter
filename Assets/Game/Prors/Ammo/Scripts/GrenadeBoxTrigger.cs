using UnityEngine;
using StarterAssets;

public class GrenadeBoxTrigger : MonoBehaviour
{
    public int _grenade; // сколько гранат в ящике
    public int _addGrenadeMax;  // сколько гранат может взять игрок
    //public int _addGrenade;  // сколько гранат взял игрок по факту
    public GrenadeBox GrenadeBox;
    private bool _isActivate;  // датчик срабатывания
    public enum TypeGrenade
    {
        EMP,
        Explosive,
        Smoke,
        Flame,
        Bomb
    };
    public TypeGrenade _typeGrenade = TypeGrenade.EMP;

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
            if (!_isActivate)
            {
                _isActivate = true;
                if (other.GetComponent<My_Weapon_Controller>()._totalEMPGrenades < other.GetComponent<My_Weapon_Controller>()._maxGrenades)
                {
                    _addGrenadeMax = other.GetComponent<My_Weapon_Controller>()._maxGrenades - other.GetComponent<My_Weapon_Controller>()._totalEMPGrenades;
                    if (_addGrenadeMax >= _grenade)
                    {
                        //_addGrenade = _grenade;
                        Debug.Log("Игрок поднял гранаты +" + _grenade.ToString() + "шт!");
                        other.GetComponent<My_Weapon_Controller>().AddEMPGrenades(_grenade);
                        GrenadeBox.Destroy();
                    }
                    else
                    {
                        //_addGrenade = _addGrenadeMax;
                        _grenade -= _addGrenadeMax;
                        GrenadeBox.DecriceGrenade(_addGrenadeMax);

                        Debug.Log("Игрок поднял не все гранаты => +" + _addGrenadeMax.ToString() + "шт!");
                        other.GetComponent<My_Weapon_Controller>().AddEMPGrenades(_addGrenadeMax);
                        other.GetComponent<My_Weapon_Controller>().AddAmmoFull(1);  // теперь повоюем!
                    }
                }
                else
                {
                    other.GetComponent<My_Weapon_Controller>().AddAmmoFull(2);  // некуда ложить!
                }
            }            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isActivate = false;
        }
    }

}
