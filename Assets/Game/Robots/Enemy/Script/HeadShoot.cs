using UnityEngine;

namespace StarterAssets
{
    public class HeadShoot : MonoBehaviour
    {
        [Header("Damage")]
       // public GameObject Head;
        public EnemyHealthComponent EnemyHealthComponent;


        public void TakeHeadShoot(int _damage)
        {
            EnemyHealthComponent.HeadShot(_damage);
            //Debug.Log("уедьнр3!!!");            
        }
    }
}
