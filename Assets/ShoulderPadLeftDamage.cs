using UnityEngine;

namespace StarterAssets
{

    public class ShoulderPadLeftDamage : MonoBehaviour
    {
        [Header("Damage")]
        // public GameObject Head;
        public EnemyHealthComponent EnemyHealthComponent;


        public void TakeDamage(int _damage)
        {
            EnemyHealthComponent.TakeDamage(_damage);
            Debug.Log("Урон по левому наплечнику!");
        }
    }
}
