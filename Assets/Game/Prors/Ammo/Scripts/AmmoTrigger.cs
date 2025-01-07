using StarterAssets;
using UnityEngine;

    public class AmmoTrigger : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public int _ammo;        // сколько патронов было в €щике
        public int _addAmmoMax;  // сколько патронов может вз€ть игрок
        //public int _addAmmo;     // сколько патронов вз€л игрок по факту
        public AmmoBox AmmoBox;        

        void Start()
        {
            //AmmoBox = GetComponentInParent<AmmoBox>();
            _ammo = AmmoBox.Ammo; // сколько патронов было в €щике
        }


        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("ѕодн€ты патроны");
            if (other.gameObject.tag == "Player")
            {   
                if (other.GetComponent<My_Weapon_Controller>()._totalAmmo < other.GetComponent<My_Weapon_Controller>()._maxAmmo)
                {
                    _addAmmoMax = other.GetComponent<My_Weapon_Controller>()._maxAmmo - other.GetComponent<My_Weapon_Controller>()._totalAmmo;
                    if (_addAmmoMax >= _ammo)
                    {
                        Debug.Log("»грок подн€л патроны +" + _ammo.ToString() + "шт!");
                        other.GetComponent<My_Weapon_Controller>().AddAmmo(_ammo);
                        AmmoBox.Destroy();
                    }
                    else
                    {                    
                        _ammo -= _addAmmoMax;
                        AmmoBox.DecriceAmmo(_addAmmoMax);
                        Debug.Log("»грок подн€л патроны +" + _addAmmoMax.ToString() + "шт!");
                        other.GetComponent<My_Weapon_Controller>().AddAmmo(_addAmmoMax);
                        other.GetComponent<My_Weapon_Controller>().AddAmmoFull(1);
                    }
                }
                else
                {
                    //патронов слишком много
                    other.GetComponent<My_Weapon_Controller>().AddAmmoFull(2);
                }
            }
        }
    }

