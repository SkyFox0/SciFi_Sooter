using StarterAssets;
using UnityEngine;

namespace StarterAssets
{
    public class InteractiveZoneTrigger : MonoBehaviour
    {
        private bool _isActivate;  // ������ ������������
        public My_Weapon_Controller My_Weapon_Controller;

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("������� �������");
            if (other.gameObject.tag == "Player")
            {
                if (!_isActivate)
                {
                    _isActivate = true;
                    other.GetComponent<My_Weapon_Controller>().AutoArmedOff();                    
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                _isActivate = false;
                other.GetComponent<My_Weapon_Controller>().AutoArmedOn();
            }
        }
    }
}
