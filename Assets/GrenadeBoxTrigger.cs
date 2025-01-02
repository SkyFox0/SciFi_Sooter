using UnityEngine;
using StarterAssets;

public class GrenadeBoxTrigger : MonoBehaviour
{
    public int _grenade; // ������� ������ � �����
    public int _addGrenadeMax;  // ������� ������ ����� ����� �����
    public int _addGrenade;  // ������� ������ ���� ����� �� �����
    public GrenadeBox GrenadeBox;

    void Start()
    {
        GrenadeBox = GetComponentInParent<GrenadeBox>();
        _grenade = GrenadeBox._grenade;
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("������� �������");
        if (other.gameObject.tag == "Player")
        {
            if (other.GetComponent<My_Weapon_Controller>()._totalGrenades < other.GetComponent<My_Weapon_Controller>()._maxGrenades)
            {
                _addGrenadeMax = other.GetComponent<My_Weapon_Controller>()._maxGrenades - other.GetComponent<My_Weapon_Controller>()._totalGrenades;
                if (_grenade <= _addGrenadeMax)
                {                    
                    _addGrenade = _grenade;
                    Debug.Log("����� ������ ������� +" + _addGrenade.ToString() + "��!");
                    other.GetComponent<My_Weapon_Controller>().AddGrenades(_addGrenade);
                    GrenadeBox.Destroy();
                }
                else
                {
                    _addGrenade = _addGrenadeMax;
                    _grenade -= _addGrenade;

                    GrenadeBox._grenade = _grenade;
                    GrenadeBox._text.text = GrenadeBox._grenade.ToString();
                    Debug.Log("����� ������ ������� +" + _addGrenade.ToString() + "��!");
                    other.GetComponent<My_Weapon_Controller>().AddGrenades(_addGrenade);
                }
                
            }
        }
    }
}
