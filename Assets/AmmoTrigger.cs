using StarterAssets;
using UnityEngine;

    public class AmmoTrigger : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public int _addAmmo;
        public AmmoBox AmmoBox;        

        void Start()
        {
            AmmoBox = GetComponentInParent<AmmoBox>();
            _addAmmo = AmmoBox.Ammo;
        }


        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("Подняты патроны");
            if (other.gameObject.tag == "Player")
            {   
                if (other.GetComponent<My_Weapon_Controller>()._totalAmmo < other.GetComponent<My_Weapon_Controller>()._maxAmmo)
                {
                    Debug.Log("Игрок поднял патроны +" + _addAmmo.ToString() + "шт!");
                    other.GetComponent<My_Weapon_Controller>().AddAmmo(_addAmmo);
                    AmmoBox.Destroy();
                }                  
            }
        }
    }

