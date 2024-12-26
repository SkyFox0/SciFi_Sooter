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
            //Debug.Log("������� �������");
            if (other.gameObject.tag == "Player")
            {
                Debug.Log("����� ������ ������� +" + _addAmmo.ToString() + "��!");
                other.GetComponent<My_Weapon_Controller>().AddAmmo(_addAmmo);
                //other.GetComponent<My_Weapon_Controller>().Healing.Play();    

                AmmoBox.Destroy();
            }
        }

    }

