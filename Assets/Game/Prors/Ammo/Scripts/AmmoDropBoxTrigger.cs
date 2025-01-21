using StarterAssets;
using UnityEngine;

public class AmmoDropBoxTrigger : MonoBehaviour
{
    public int _addAmmo;
    public AmmoBoxEnemyDrop AmmoBoxEnemyDrop;

    void Start()
    {
        AmmoBoxEnemyDrop = GetComponentInParent<AmmoBoxEnemyDrop>();
        _addAmmo = AmmoBoxEnemyDrop.Ammo;
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("������� �������");
        if (other.gameObject.tag == "Player")
        {
            if (other.GetComponent<My_Weapon_Controller>()._totalAmmo < other.GetComponent<My_Weapon_Controller>()._maxAmmo)
            {
                Debug.Log("����� ������ ������� +" + _addAmmo.ToString() + "��!");
                other.GetComponent<My_Weapon_Controller>().AddAmmo(_addAmmo);
                AmmoBoxEnemyDrop.Destroy();
            }
        }
    }
}