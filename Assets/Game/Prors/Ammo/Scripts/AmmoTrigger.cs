using StarterAssets;
using UnityEngine;

    public class AmmoTrigger : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public int _ammo;        // ������� �������� ���� � �����
        public int _addAmmoMax;  // ������� �������� ����� ����� �����
        //public int _addAmmo;     // ������� �������� ���� ����� �� �����
        public AmmoBox AmmoBox;        

        void Start()
        {
            //AmmoBox = GetComponentInParent<AmmoBox>();
            _ammo = AmmoBox.Ammo; // ������� �������� ���� � �����
        }


        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("������� �������");
            if (other.gameObject.tag == "Player")
            {   
                if (other.GetComponent<My_Weapon_Controller>()._totalAmmo < other.GetComponent<My_Weapon_Controller>()._maxAmmo)
                {
                    _addAmmoMax = other.GetComponent<My_Weapon_Controller>()._maxAmmo - other.GetComponent<My_Weapon_Controller>()._totalAmmo;
                    if (_addAmmoMax >= _ammo)
                    {
                        Debug.Log("����� ������ ������� +" + _ammo.ToString() + "��!");
                        other.GetComponent<My_Weapon_Controller>().AddAmmo(_ammo);
                        AmmoBox.Destroy();
                    }
                    else
                    {                    
                        _ammo -= _addAmmoMax;
                        AmmoBox.DecriceAmmo(_addAmmoMax);
                        Debug.Log("����� ������ ������� +" + _addAmmoMax.ToString() + "��!");
                        other.GetComponent<My_Weapon_Controller>().AddAmmo(_addAmmoMax);
                        other.GetComponent<My_Weapon_Controller>().AddAmmoFull(1);
                    }
                }
                else
                {
                    //�������� ������� �����
                    other.GetComponent<My_Weapon_Controller>().AddAmmoFull(2);
                }
            }
        }
    }

